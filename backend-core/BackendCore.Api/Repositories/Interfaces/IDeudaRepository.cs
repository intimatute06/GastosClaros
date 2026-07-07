using BackendCore.Api.Models;

namespace BackendCore.Api.Repositories.Interfaces
{
    public interface IDeudaRepository
    {
        Task<IEnumerable<Deuda>> GetAllAsync();
        Task<Deuda?> GetByIdAsync(int id);
        Task<Deuda> CreateAsync(Deuda deuda);
        Task<bool> MiembroExisteAsync(int miembroId);
        Task<Deuda?> ActualizarEstadoAsync(int id, EstadoDeuda estado, string? referenciaTransaccion);
    }
}
