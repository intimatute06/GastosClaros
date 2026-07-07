using System.Text;
using System.Text.Json;
using ModuloCategoria.Worker.Models;
using ModuloCategoria.Worker.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ModuloCategoria.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;
    private const string ExchangeName = "gastosclaros.eventos";
    private const string QueueName = "categoria.gastos";
    private const string RoutingKey = "gasto.registrado";

    private IConnection? _connection;
    private IChannel? _channel;

    public Worker(ILogger<Worker> logger, IConfiguration configuration, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _configuration = configuration;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
            Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
            UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest"
        };

        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
        await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(QueueName, ExchangeName, RoutingKey, cancellationToken: stoppingToken);

        _logger.LogInformation("Escuchando cola '{Queue}' enlazada a '{Exchange}' con routing key '{RoutingKey}'", QueueName, ExchangeName, RoutingKey);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var evento = JsonSerializer.Deserialize<GastoRegistradoEvento>(json);

                if (evento != null)
                {
                    _logger.LogInformation("Evento recibido: Gasto {GastoId} - '{Descripcion}' (${Monto})", evento.GastoId, evento.Descripcion, evento.Monto);

                    using var scope = _scopeFactory.CreateScope();
                    var clasificador = scope.ServiceProvider.GetRequiredService<IClasificadorService>();
                    var backendClient = scope.ServiceProvider.GetRequiredService<IBackendCoreClient>();

                    var categoria = clasificador.Clasificar(evento.Descripcion);
                    _logger.LogInformation("Gasto {GastoId} clasificado como '{Categoria}'", evento.GastoId, categoria);

                    await backendClient.ActualizarCategoriaAsync(evento.GastoId, categoria);
                }

                await _channel!.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando mensaje de la cola {Queue}", QueueName);
                await _channel!.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
            }
        };

        await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer, stoppingToken);

        // Mantener el worker vivo hasta que se solicite detenerlo
        await Task.Delay(Timeout.Infinite, stoppingToken).ContinueWith(_ => { });
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel != null) await _channel.CloseAsync(cancellationToken);
        if (_connection != null) await _connection.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}
