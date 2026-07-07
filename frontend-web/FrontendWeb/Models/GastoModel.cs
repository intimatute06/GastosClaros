namespace FrontendWeb.Models
{
    public class GastoModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string? Categoria { get; set; }
        public int GrupoId { get; set; }
        public int PagadoPorId { get; set; }
        public string PagadoPorNombre { get; set; } = string.Empty;
    }

    public class CrearGastoModel
    {
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public int GrupoId { get; set; }
        public int PagadoPorId { get; set; }
    }
}
