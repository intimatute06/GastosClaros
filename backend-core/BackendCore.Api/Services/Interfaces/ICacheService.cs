namespace BackendCore.Api.Services.Interfaces
{
    public interface ICacheService
    {
        Task<string?> GetAsync(string key);
        Task SetAsync(string key, string value, TimeSpan? expiracion = null);
        Task RemoveAsync(string key);
    }
}
