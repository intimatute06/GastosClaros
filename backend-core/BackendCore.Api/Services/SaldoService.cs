using System.Text.Json;
using BackendCore.Api.DTOs;
using BackendCore.Api.Models;
using BackendCore.Api.Repositories.Interfaces;
using BackendCore.Api.Services.Interfaces;

namespace BackendCore.Api.Services
{
    public class SaldoService : ISaldoService
    {
        private readonly ISaldoRepository _repository;
        private readonly ICacheService _cache;
        private static readonly TimeSpan DuracionCache = TimeSpan.FromMinutes(10);

        public SaldoService(ISaldoRepository repository, ICacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<IEnumerable<SaldoDto>> ObtenerAsync(int grupoId)
        {
            var cacheKey = ClaveCache(grupoId);
            var cacheado = await _cache.GetAsync(cacheKey);

            if (cacheado != null)
            {
                var saldosCacheados = JsonSerializer.Deserialize<List<SaldoDto>>(cacheado);
                if (saldosCacheados != null) return saldosCacheados;
            }

            return await RecalcularYObtenerAsync(grupoId);
        }

        public async Task<IEnumerable<SaldoDto>> RecalcularYObtenerAsync(int grupoId)
        {
            var gastos = (await _repository.GetGastosByGrupoIdAsync(grupoId)).ToList();
            var miembros = (await _repository.GetMiembrosByGrupoIdAsync(grupoId)).ToList();

            if (miembros.Count == 0)
            {
                return Enumerable.Empty<SaldoDto>();
            }

            // Paso 1: calcular cuanto debe cada miembro a quien pago, por cada gasto
            // deudasBrutas[deudorId][acreedorId] = monto acumulado
            var deudasBrutas = new Dictionary<int, Dictionary<int, decimal>>();

            foreach (var gasto in gastos)
            {
                var cuotaPorPersona = gasto.Monto / miembros.Count;

                foreach (var miembro in miembros)
                {
                    if (miembro.Id == gasto.PagadoPorId) continue;

                    if (!deudasBrutas.ContainsKey(miembro.Id))
                        deudasBrutas[miembro.Id] = new Dictionary<int, decimal>();

                    if (!deudasBrutas[miembro.Id].ContainsKey(gasto.PagadoPorId))
                        deudasBrutas[miembro.Id][gasto.PagadoPorId] = 0;

                    deudasBrutas[miembro.Id][gasto.PagadoPorId] += cuotaPorPersona;
                }
            }

            // Paso 2: netear deudas cruzadas entre cada par (A debe a B, B debe a A -> un solo saldo neto)
            var saldosNeteados = new List<Saldo>();
            var paresYaProcesados = new HashSet<(int, int)>();

            foreach (var deudorId in deudasBrutas.Keys)
            {
                foreach (var acreedorId in deudasBrutas[deudorId].Keys)
                {
                    var par = deudorId < acreedorId ? (deudorId, acreedorId) : (acreedorId, deudorId);
                    if (paresYaProcesados.Contains(par)) continue;
                    paresYaProcesados.Add(par);

                    var deudaAB = deudasBrutas.ContainsKey(deudorId) && deudasBrutas[deudorId].ContainsKey(acreedorId)
                        ? deudasBrutas[deudorId][acreedorId] : 0;

                    var deudaBA = deudasBrutas.ContainsKey(acreedorId) && deudasBrutas[acreedorId].ContainsKey(deudorId)
                        ? deudasBrutas[acreedorId][deudorId] : 0;

                    var neto = deudaAB - deudaBA;

                    if (neto > 0)
                    {
                        saldosNeteados.Add(new Saldo { GrupoId = grupoId, DeudorId = deudorId, AcreedorId = acreedorId, Monto = Math.Round(neto, 2) });
                    }
                    else if (neto < 0)
                    {
                        saldosNeteados.Add(new Saldo { GrupoId = grupoId, DeudorId = acreedorId, AcreedorId = deudorId, Monto = Math.Round(-neto, 2) });
                    }
                }
            }

            // Paso 3: persistir en PostgreSQL (recalculo completo)
            await _repository.LimpiarSaldosDeGrupoAsync(grupoId);
            if (saldosNeteados.Count > 0)
            {
                await _repository.GuardarSaldosAsync(saldosNeteados);
            }

            // Paso 4: mapear a DTO con nombres, y actualizar cache
            var miembrosPorId = miembros.ToDictionary(m => m.Id, m => m.Nombre);
            var dtos = saldosNeteados.Select(s => new SaldoDto
            {
                DeudorId = s.DeudorId,
                DeudorNombre = miembrosPorId.GetValueOrDefault(s.DeudorId, "Desconocido"),
                AcreedorId = s.AcreedorId,
                AcreedorNombre = miembrosPorId.GetValueOrDefault(s.AcreedorId, "Desconocido"),
                Monto = s.Monto
            }).ToList();

            await _cache.SetAsync(ClaveCache(grupoId), JsonSerializer.Serialize(dtos), DuracionCache);

            return dtos;
        }

        private static string ClaveCache(int grupoId) => $"saldos:grupo:{grupoId}";
    }
}
