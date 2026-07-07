using System.Text;
using System.Text.Json;

namespace ModuloCategoria.Worker.Services
{
    public class BackendCoreClient : IBackendCoreClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BackendCoreClient> _logger;

        public BackendCoreClient(HttpClient httpClient, ILogger<BackendCoreClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task ActualizarCategoriaAsync(int gastoId, string categoria)
        {
            var body = JsonSerializer.Serialize(new { categoria });
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await _httpClient.PatchAsync($"/api/Gastos/{gastoId}/categoria", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "No se pudo actualizar la categoria del gasto {GastoId}. Status: {Status}",
                    gastoId, response.StatusCode);
            }
        }
    }
}
