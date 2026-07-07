using System.Net.Http.Json;
using FrontendWeb.Models;
using FrontendWeb.Services.Interfaces;

namespace FrontendWeb.Services
{
    public class BackendCoreApi : IBackendCoreApi
    {
        private readonly HttpClient _http;
        private readonly ILogger<BackendCoreApi> _logger;

        public BackendCoreApi(HttpClient http, ILogger<BackendCoreApi> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<List<GrupoModel>> GetGruposAsync()
        {
            return await _http.GetFromJsonAsync<List<GrupoModel>>("api/Grupos") ?? new List<GrupoModel>();
        }

        public async Task<GrupoModel?> GetGrupoAsync(int id)
        {
            var response = await _http.GetAsync($"api/Grupos/{id}");
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<GrupoModel>();
        }

        public async Task<GrupoModel?> CrearGrupoAsync(CrearGrupoModel dto)
        {
            var response = await _http.PostAsJsonAsync("api/Grupos", dto);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<GrupoModel>();
        }

        public async Task<List<MiembroModel>> GetMiembrosPorGrupoAsync(int grupoId)
        {
            return await _http.GetFromJsonAsync<List<MiembroModel>>($"api/Miembros/grupo/{grupoId}") ?? new List<MiembroModel>();
        }

        public async Task<MiembroModel?> CrearMiembroAsync(CrearMiembroModel dto)
        {
            var response = await _http.PostAsJsonAsync("api/Miembros", dto);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<MiembroModel>();
        }

        public async Task<List<GastoModel>> GetGastosPorGrupoAsync(int grupoId)
        {
            return await _http.GetFromJsonAsync<List<GastoModel>>($"api/Gastos/grupo/{grupoId}") ?? new List<GastoModel>();
        }

        public async Task<GastoModel?> CrearGastoAsync(CrearGastoModel dto)
        {
            var response = await _http.PostAsJsonAsync("api/Gastos", dto);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<GastoModel>();
        }
        public async Task<bool> ActualizarCategoriaGastoAsync(int gastoId, string categoria)
        {
            var body = new { categoria };
            var response = await _http.PatchAsJsonAsync($"api/Gastos/{gastoId}/categoria", body);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<SaldoModel>> GetSaldosPorGrupoAsync(int grupoId)
        {
            return await _http.GetFromJsonAsync<List<SaldoModel>>($"api/Saldos/grupo/{grupoId}") ?? new List<SaldoModel>();
        }

        public async Task<List<DeudaModel>> GetDeudasAsync()
        {
            return await _http.GetFromJsonAsync<List<DeudaModel>>("api/Deudas") ?? new List<DeudaModel>();
        }

        public async Task<DeudaModel?> CrearDeudaAsync(CrearDeudaModel dto)
        {
            var response = await _http.PostAsJsonAsync("api/Deudas", dto);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<DeudaModel>();
        }

        public async Task<DeudaModel?> SaldarDeudaAsync(int id)
        {
            var response = await _http.PostAsync($"api/Deudas/{id}/saldar", null);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("No se pudo saldar la deuda {Id}. Status: {Status}", id, response.StatusCode);
            }
            return await response.Content.ReadFromJsonAsync<DeudaModel>();
        }
    }
}
