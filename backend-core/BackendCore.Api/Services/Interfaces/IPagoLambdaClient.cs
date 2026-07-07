namespace BackendCore.Api.Services.Interfaces
{
    public record ResultadoPagoLambda(bool Exitoso, string Estado, string? ReferenciaTransaccion, string Mensaje);

    public interface IPagoLambdaClient
    {
        Task<ResultadoPagoLambda> ProcesarPagoAsync(int deudorId, int acreedorId, decimal monto);
    }
}
