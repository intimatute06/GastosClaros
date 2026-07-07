namespace ModuloCategoria.Worker.Services
{
    public interface IBackendCoreClient
    {
        Task ActualizarCategoriaAsync(int gastoId, string categoria);
    }
}
