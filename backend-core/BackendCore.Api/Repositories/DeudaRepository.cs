using BackendCore.Api.Data;
using BackendCore.Api.Models;
using BackendCore.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendCore.Api.Repositories
{
    public class DeudaRepository : IDeudaRepository
    {
        private readonly AppDbContext _context;

        public DeudaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Deuda>> GetAllAsync()
        {
            return await _context.Deudas.ToListAsync();
        }

        public async Task<Deuda?> GetByIdAsync(int id)
        {
            return await _context.Deudas.FindAsync(id);
        }

        public async Task<Deuda> CreateAsync(Deuda deuda)
        {
            _context.Deudas.Add(deuda);
            await _context.SaveChangesAsync();
            return deuda;
        }

        public async Task<bool> MiembroExisteAsync(int miembroId)
        {
            return await _context.Miembros.AnyAsync(m => m.Id == miembroId);
        }

        public async Task<Deuda?> ActualizarEstadoAsync(int id, EstadoDeuda estado, string? referenciaTransaccion)
        {
            var deuda = await _context.Deudas.FindAsync(id);
            if (deuda == null) return null;

            deuda.Estado = estado;
            deuda.ReferenciaTransaccion = referenciaTransaccion;
            if (estado == EstadoDeuda.Saldada)
            {
                deuda.FechaSaldada = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return deuda;
        }
    }
}
