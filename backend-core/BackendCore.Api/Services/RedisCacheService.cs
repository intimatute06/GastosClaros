using BackendCore.Api.Services.Interfaces;
using StackExchange.Redis;

namespace BackendCore.Api.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IConnectionMultiplexer redis, ILogger<RedisCacheService> logger)
        {
            _redis = redis;
            _logger = logger;
        }

        public async Task<string?> GetAsync(string key)
        {
            try
            {
                var db = _redis.GetDatabase();
                return await db.StringGetAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo leer de Redis la clave {Key}. Se continua sin cache.", key);
                return null;
            }
        }

        public async Task SetAsync(string key, string value, TimeSpan? expiracion = null)
        {
            try
            {
                var db = _redis.GetDatabase();
                await db.StringSetAsync(key, value, expiracion ?? TimeSpan.FromMinutes(10));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo escribir en Redis la clave {Key}. Se continua sin cache.", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                var db = _redis.GetDatabase();
                await db.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo eliminar de Redis la clave {Key}.", key);
            }
        }
    }
}
