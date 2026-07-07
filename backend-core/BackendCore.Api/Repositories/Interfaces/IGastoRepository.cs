using BackendCore.Api.Models;

namespace BackendCore.Api.Repositories.Interfaces
{
    public interface IGastoRepository
    {
        Task<IEnumerable<Gasto>> GetByGrupoIdAsync(int grupoId);
        Task<Gasto?> GetByIdAsync(int id);
        Task<Gasto> CreateAsync(Gasto gasto);
        Task<bool> GrupoExisteAsync(int grupoId);
        Task<bool> MiembroExisteAsync(int miembroId);
    }
}
