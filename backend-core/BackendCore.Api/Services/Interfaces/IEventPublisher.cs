namespace BackendCore.Api.Services.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(string routingKey, T mensaje);
    }
}
