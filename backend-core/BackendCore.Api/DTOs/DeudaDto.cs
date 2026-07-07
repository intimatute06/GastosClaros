namespace BackendCore.Api.DTOs
{
    public class DeudaDto
    {
        public int Id { get; set; }
        public int DeudorId { get; set; }
        public int AcreedorId { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaSaldada { get; set; }
        public string? ReferenciaTransaccion { get; set; }
    }

    public class CrearDeudaDto
    {
        public int DeudorId { get; set; }
        public int AcreedorId { get; set; }
        public decimal Monto { get; set; }
    }
}
