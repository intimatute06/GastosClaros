using BackendCore.Api.Data;
using BackendCore.Api.Models;
using BackendCore.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendCore.Api.Repositories
{
    public class GrupoRepository : IGrupoRepository
    {
        private readonly AppDbContext _context;

        public GrupoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Grupo>> GetAllAsync()
        {
            return await _context.Grupos
                .Include(g => g.Miembros)
                .ToListAsync();
        }

        public async Task<Grupo?> GetByIdAsync(int id)
        {
            return await _context.Grupos
                .Include(g => g.Miembros)
                .Include(g => g.Gastos)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Grupo> CreateAsync(Grupo grupo)
        {
            _context.Grupos.Add(grupo);
            await _context.SaveChangesAsync();
            return grupo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var grupo = await _context.Grupos.FindAsync(id);
            if (grupo == null) return false;

            _context.Grupos.Remove(grupo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
