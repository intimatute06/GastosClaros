namespace FrontendWeb.Models
{
    public class GrupoModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public int CantidadMiembros { get; set; }
    }

    public class CrearGrupoModel
    {
        public string Nombre { get; set; } = string.Empty;
    }
}
