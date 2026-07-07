using BackendCore.Api.Models;

namespace BackendCore.Api.Repositories.Interfaces
{
    public interface ISaldoRepository
    {
        Task<IEnumerable<Gasto>> GetGastosByGrupoIdAsync(int grupoId);
        Task<IEnumerable<Miembro>> GetMiembrosByGrupoIdAsync(int grupoId);
        Task LimpiarSaldosDeGrupoAsync(int grupoId);
        Task GuardarSaldosAsync(IEnumerable<Saldo> saldos);
    }
}
