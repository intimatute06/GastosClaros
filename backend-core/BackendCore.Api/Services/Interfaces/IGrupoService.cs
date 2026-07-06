using BackendCore.Api.DTOs;

namespace BackendCore.Api.Services.Interfaces
{
    public interface IGrupoService
    {
        Task<IEnumerable<GrupoDto>> GetAllAsync();
        Task<GrupoDto?> GetByIdAsync(int id);
        Task<GrupoDto> CreateAsync(CrearGrupoDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
