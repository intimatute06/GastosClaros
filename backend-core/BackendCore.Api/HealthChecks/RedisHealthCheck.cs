using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace BackendCore.Api.HealthChecks
{
    public class RedisHealthCheck : IHealthCheck
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisHealthCheck(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var resultado = _redis.IsConnected
                    ? HealthCheckResult.Healthy("Redis conectado")
                    : HealthCheckResult.Unhealthy("Redis no conectado");
                return Task.FromResult(resultado);
            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Error verificando Redis", ex));
            }
        }
    }
}
