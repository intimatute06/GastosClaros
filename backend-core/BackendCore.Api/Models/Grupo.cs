namespace BackendCore.Api.Models
{
    public class Grupo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relacion 1-N: un grupo tiene varios miembros
        public ICollection<Miembro> Miembros { get; set; } = new List<Miembro>();

        // Relacion 1-N: un grupo tiene varios gastos
        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
    }
}
