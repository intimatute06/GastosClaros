namespace BackendCore.Api.Models
{
    public class Miembro
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public int GrupoId { get; set; }
        public Grupo? Grupo { get; set; }

        // Gastos que este miembro pago
        public ICollection<Gasto> GastosPagados { get; set; } = new List<Gasto>();
    }
}
