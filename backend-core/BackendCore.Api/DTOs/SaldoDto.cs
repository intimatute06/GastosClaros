namespace BackendCore.Api.DTOs
{
    public class SaldoDto
    {
        public int DeudorId { get; set; }
        public string DeudorNombre { get; set; } = string.Empty;
        public int AcreedorId { get; set; }
        public string AcreedorNombre { get; set; } = string.Empty;
        public decimal Monto { get; set; }
    }
}
