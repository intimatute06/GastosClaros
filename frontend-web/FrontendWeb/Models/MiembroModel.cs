namespace FrontendWeb.Models
{
    public class MiembroModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int GrupoId { get; set; }
    }

    public class CrearMiembroModel
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int GrupoId { get; set; }
    }
}
