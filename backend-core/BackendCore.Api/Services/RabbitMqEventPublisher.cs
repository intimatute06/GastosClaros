using System.Text;
using System.Text.Json;
using BackendCore.Api.Services.Interfaces;
using RabbitMQ.Client;

namespace BackendCore.Api.Services
{
    public class RabbitMqEventPublisher : IEventPublisher, IAsyncDisposable
    {
        private readonly IConfiguration _configuration;
        private IConnection? _connection;
        private IChannel? _channel;
        private const string ExchangeName = "gastosclaros.eventos";

        public RabbitMqEventPublisher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task<IChannel> GetChannelAsync()
        {
            if (_channel != null && _channel.IsOpen)
            {
                return _channel;
            }

            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
                Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
                UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
                Password = _configuration["RabbitMQ:Password"] ?? "guest"
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Topic, durable: true);

            return _channel;
        }

        public async Task PublishAsync<T>(string routingKey, T mensaje)
        {
            var channel = await GetChannelAsync();

            var json = JsonSerializer.Serialize(mensaje);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json"
            };

            await channel.BasicPublishAsync(
                exchange: ExchangeName,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: properties,
                body: body);
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel != null) await _channel.CloseAsync();
            if (_connection != null) await _connection.CloseAsync();
        }
    }
}
