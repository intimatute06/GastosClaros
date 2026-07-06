namespace BackendCore.Api.Models
{
    public class Gasto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        // Categoria asignada por el modulo de categoria (nullable: puede llegar despues via cola)
        public string? Categoria { get; set; }

        public int GrupoId { get; set; }
        public Grupo? Grupo { get; set; }

        // Miembro que pago el gasto
        public int PagadoPorId { get; set; }
        public Miembro? PagadoPor { get; set; }
    }
}
