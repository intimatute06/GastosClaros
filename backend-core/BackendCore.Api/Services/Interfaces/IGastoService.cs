using BackendCore.Api.DTOs;

namespace BackendCore.Api.Services.Interfaces
{
    public interface IGastoService
    {
        Task<IEnumerable<GastoDto>> GetByGrupoIdAsync(int grupoId);
        Task<GastoDto?> GetByIdAsync(int id);
        Task<(GastoDto? gasto, string? error)> CreateAsync(CrearGastoDto dto);
        Task<bool> ActualizarCategoriaAsync(int gastoId, string categoria);
    }
}
