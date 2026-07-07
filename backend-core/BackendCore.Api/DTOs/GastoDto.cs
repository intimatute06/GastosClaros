namespace BackendCore.Api.DTOs
{
    public class GastoDto
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

    public class CrearGastoDto
    {
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public int GrupoId { get; set; }
        public int PagadoPorId { get; set; }
    }
    public class ActualizarCategoriaDto
    {
        public string Categoria { get; set; } = string.Empty;
    }
}
