using BackendCore.Api.Data;
using BackendCore.Api.Models;
using BackendCore.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendCore.Api.Repositories
{
    public class SaldoRepository : ISaldoRepository
    {
        private readonly AppDbContext _context;

        public SaldoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Gasto>> GetGastosByGrupoIdAsync(int grupoId)
        {
            return await _context.Gastos
                .Where(g => g.GrupoId == grupoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Miembro>> GetMiembrosByGrupoIdAsync(int grupoId)
        {
            return await _context.Miembros
                .Where(m => m.GrupoId == grupoId)
                .ToListAsync();
        }

        public async Task LimpiarSaldosDeGrupoAsync(int grupoId)
        {
            var saldosExistentes = await _context.Saldos
                .Where(s => s.GrupoId == grupoId)
                .ToListAsync();

            _context.Saldos.RemoveRange(saldosExistentes);
            await _context.SaveChangesAsync();
        }

        public async Task GuardarSaldosAsync(IEnumerable<Saldo> saldos)
        {
            _context.Saldos.AddRange(saldos);
            await _context.SaveChangesAsync();
        }
    }
}
