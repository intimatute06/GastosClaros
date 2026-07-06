using BackendCore.Api.Models;

namespace BackendCore.Api.Repositories.Interfaces
{
    public interface IMiembroRepository
    {
        Task<IEnumerable<Miembro>> GetByGrupoIdAsync(int grupoId);
        Task<Miembro?> GetByIdAsync(int id);
        Task<Miembro?> CreateAsync(Miembro miembro);
        Task<bool> DeleteAsync(int id);
        Task<bool> GrupoExisteAsync(int grupoId);
    }
}
