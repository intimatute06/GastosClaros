using BackendCore.Api.DTOs;

namespace BackendCore.Api.Services.Interfaces
{
    public interface IDeudaService
    {
        Task<IEnumerable<DeudaDto>> GetAllAsync();
        Task<DeudaDto?> GetByIdAsync(int id);
        Task<(DeudaDto? deuda, string? error)> CreateAsync(CrearDeudaDto dto);
        Task<(DeudaDto? deuda, string? error)> SaldarAsync(int id);
    }
}
