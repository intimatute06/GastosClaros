using BackendCore.Api.Data;
using BackendCore.Api.Models;
using BackendCore.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendCore.Api.Repositories
{
    public class GastoRepository : IGastoRepository
    {
        private readonly AppDbContext _context;

        public GastoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Gasto>> GetByGrupoIdAsync(int grupoId)
        {
            return await _context.Gastos
                .Include(g => g.PagadoPor)
                .Where(g => g.GrupoId == grupoId)
                .ToListAsync();
        }

        public async Task<Gasto?> GetByIdAsync(int id)
        {
            return await _context.Gastos
                .Include(g => g.PagadoPor)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Gasto> CreateAsync(Gasto gasto)
        {
            _context.Gastos.Add(gasto);
            await _context.SaveChangesAsync();

            // Recargamos con el miembro incluido para tener el nombre disponible al mapear el DTO
            await _context.Entry(gasto).Reference(g => g.PagadoPor).LoadAsync();

            return gasto;
        }

        public async Task<bool> GrupoExisteAsync(int grupoId)
        {
            return await _context.Grupos.AnyAsync(g => g.Id == grupoId);
        }

        public async Task<bool> MiembroExisteAsync(int miembroId)
        {
            return await _context.Miembros.AnyAsync(m => m.Id == miembroId);
        }
        public async Task<bool> ActualizarCategoriaAsync(int gastoId, string categoria)
        {
            var gasto = await _context.Gastos.FindAsync(gastoId);
            if (gasto == null) return false;

            gasto.Categoria = categoria;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
