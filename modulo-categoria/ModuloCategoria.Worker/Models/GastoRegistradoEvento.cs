namespace ModuloCategoria.Worker.Models
{
    public record GastoRegistradoEvento(int GastoId, int GrupoId, decimal Monto, string Descripcion, DateTime Fecha);
}
