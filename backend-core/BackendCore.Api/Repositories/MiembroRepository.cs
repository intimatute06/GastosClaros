using BackendCore.Api.Data;
using BackendCore.Api.Models;
using BackendCore.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendCore.Api.Repositories
{
    public class MiembroRepository : IMiembroRepository
    {
        private readonly AppDbContext _context;

        public MiembroRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Miembro>> GetByGrupoIdAsync(int grupoId)
        {
            return await _context.Miembros
                .Where(m => m.GrupoId == grupoId)
                .ToListAsync();
        }

        public async Task<Miembro?> GetByIdAsync(int id)
        {
            return await _context.Miembros.FindAsync(id);
        }

        public async Task<Miembro?> CreateAsync(Miembro miembro)
        {
            _context.Miembros.Add(miembro);
            await _context.SaveChangesAsync();
            return miembro;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var miembro = await _context.Miembros.FindAsync(id);
            if (miembro == null) return false;

            _context.Miembros.Remove(miembro);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> GrupoExisteAsync(int grupoId)
        {
            return await _context.Grupos.AnyAsync(g => g.Id == grupoId);
        }
    }
}
