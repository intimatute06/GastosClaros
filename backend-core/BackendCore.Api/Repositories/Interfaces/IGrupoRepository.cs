using BackendCore.Api.Models;

namespace BackendCore.Api.Repositories.Interfaces
{
    public interface IGrupoRepository
    {
        Task<IEnumerable<Grupo>> GetAllAsync();
        Task<Grupo?> GetByIdAsync(int id);
        Task<Grupo> CreateAsync(Grupo grupo);
        Task<bool> DeleteAsync(int id);
    }
}
