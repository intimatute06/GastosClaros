namespace BackendCore.Api.Models
{
    public enum EstadoDeuda
    {
        Pendiente,
        EnProceso,
        Saldada,
        Fallida
    }

    // Representa un intento de saldar una deuda entre dos miembros (via modulo de pago)
    public class Deuda
    {
        public int Id { get; set; }
        public int DeudorId { get; set; }
        public Miembro? Deudor { get; set; }

        public int AcreedorId { get; set; }
        public Miembro? Acreedor { get; set; }

        public decimal Monto { get; set; }
        public EstadoDeuda Estado { get; set; } = EstadoDeuda.Pendiente;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaSaldada { get; set; }

        // Referencia al identificador de transaccion que devuelva el modulo de pago (lambda)
        public string? ReferenciaTransaccion { get; set; }
    }
}
