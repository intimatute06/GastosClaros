using BackendCore.Api.DTOs;

namespace BackendCore.Api.Services.Interfaces
{
    public interface ISaldoService
    {
        Task<IEnumerable<SaldoDto>> RecalcularYObtenerAsync(int grupoId);
        Task<IEnumerable<SaldoDto>> ObtenerAsync(int grupoId);
    }
}
