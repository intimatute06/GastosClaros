using BackendCore.Api.DTOs;

namespace BackendCore.Api.Services.Interfaces
{
    public interface IMiembroService
    {
        Task<IEnumerable<MiembroDto>> GetByGrupoIdAsync(int grupoId);
        Task<MiembroDto?> GetByIdAsync(int id);
        Task<(MiembroDto? miembro, string? error)> CreateAsync(CrearMiembroDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
