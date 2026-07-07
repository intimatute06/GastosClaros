using System.Text;
using System.Text.Json;
using BackendCore.Api.Services.Interfaces;

namespace BackendCore.Api.Services
{
    public class PagoLambdaClient : IPagoLambdaClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PagoLambdaClient> _logger;

        public PagoLambdaClient(HttpClient httpClient, ILogger<PagoLambdaClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ResultadoPagoLambda> ProcesarPagoAsync(int deudorId, int acreedorId, decimal monto)
        {
            var body = JsonSerializer.Serialize(new { deudorId, acreedorId, monto });
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("/api/SaldarDeuda", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;

                var estado = root.GetProperty("estado").GetString() ?? "Fallida";
                var mensaje = root.TryGetProperty("mensaje", out var m) ? m.GetString() ?? "" : "";
                var referencia = root.TryGetProperty("referenciaTransaccion", out var r) ? r.GetString() : null;

                var exitoso = estado == "Saldada";
                return new ResultadoPagoLambda(exitoso, estado, referencia, mensaje);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al invocar el modulo de pago (Lambda)");
                return new ResultadoPagoLambda(false, "Fallida", null, "No se pudo contactar al modulo de pago.");
            }
        }
    }
}
