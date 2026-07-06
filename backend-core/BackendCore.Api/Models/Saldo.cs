namespace BackendCore.Api.Models
{
    // Representa cuanto le debe un miembro a otro dentro de un grupo, en un momento dado
    public class Saldo
    {
        public int Id { get; set; }
        public int GrupoId { get; set; }
        public Grupo? Grupo { get; set; }

        public int DeudorId { get; set; }
        public Miembro? Deudor { get; set; }

        public int AcreedorId { get; set; }
        public Miembro? Acreedor { get; set; }

        public decimal Monto { get; set; }
        public DateTime UltimaActualizacion { get; set; } = DateTime.UtcNow;
    }
}
