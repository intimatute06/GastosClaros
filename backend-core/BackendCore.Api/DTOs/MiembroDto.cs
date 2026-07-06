namespace BackendCore.Api.DTOs
{
    public class MiembroDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int GrupoId { get; set; }
    }

    public class CrearMiembroDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int GrupoId { get; set; }
    }
}
