namespace BackendCore.Api.DTOs
{
    public class GrupoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public int CantidadMiembros { get; set; }
    }

    public class CrearGrupoDto
    {
        public string Nombre { get; set; } = string.Empty;
    }
}
