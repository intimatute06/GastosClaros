using FrontendWeb.Models;

namespace FrontendWeb.Services.Interfaces
{
    public interface IBackendCoreApi
    {
        Task<List<GrupoModel>> GetGruposAsync();
        Task<GrupoModel?> GetGrupoAsync(int id);
        Task<GrupoModel?> CrearGrupoAsync(CrearGrupoModel dto);

        Task<List<MiembroModel>> GetMiembrosPorGrupoAsync(int grupoId);
        Task<MiembroModel?> CrearMiembroAsync(CrearMiembroModel dto);

        Task<List<GastoModel>> GetGastosPorGrupoAsync(int grupoId);
        Task<GastoModel?> CrearGastoAsync(CrearGastoModel dto);

        Task<List<SaldoModel>> GetSaldosPorGrupoAsync(int grupoId);

        Task<List<DeudaModel>> GetDeudasAsync();
        Task<DeudaModel?> CrearDeudaAsync(CrearDeudaModel dto);
        Task<DeudaModel?> SaldarDeudaAsync(int id);
    }
}
