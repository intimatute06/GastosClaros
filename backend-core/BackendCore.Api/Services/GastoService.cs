using BackendCore.Api.DTOs;
using BackendCore.Api.Models;
using BackendCore.Api.Repositories.Interfaces;
using BackendCore.Api.Services.Interfaces;

namespace BackendCore.Api.Services
{
    // Evento que se publica a RabbitMQ cuando se registra un gasto
    public record GastoRegistradoEvento(int GastoId, int GrupoId, decimal Monto, string Descripcion, DateTime Fecha);

    public class GastoService : IGastoService
    {
        private readonly IGastoRepository _repository;
        private readonly IEventPublisher _eventPublisher;
        private const string RoutingKey = "gasto.registrado";

        public GastoService(IGastoRepository repository, IEventPublisher eventPublisher)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
        }

        public async Task<IEnumerable<GastoDto>> GetByGrupoIdAsync(int grupoId)
        {
            var gastos = await _repository.GetByGrupoIdAsync(grupoId);
            return gastos.Select(MapToDto);
        }

        public async Task<GastoDto?> GetByIdAsync(int id)
        {
            var gasto = await _repository.GetByIdAsync(id);
            return gasto == null ? null : MapToDto(gasto);
        }

        public async Task<(GastoDto? gasto, string? error)> CreateAsync(CrearGastoDto dto)
        {
            if (!await _repository.GrupoExisteAsync(dto.GrupoId))
            {
                return (null, "El grupo especificado no existe.");
            }

            if (!await _repository.MiembroExisteAsync(dto.PagadoPorId))
            {
                return (null, "El miembro que pago el gasto no existe.");
            }

            var gasto = new Gasto
            {
                Descripcion = dto.Descripcion,
                Monto = dto.Monto,
                GrupoId = dto.GrupoId,
                PagadoPorId = dto.PagadoPorId
            };

            var creado = await _repository.CreateAsync(gasto);

            var evento = new GastoRegistradoEvento(
                creado.Id, creado.GrupoId, creado.Monto, creado.Descripcion, creado.Fecha);

            await _eventPublisher.PublishAsync(RoutingKey, evento);

            return (MapToDto(creado), null);
        }

        private static GastoDto MapToDto(Gasto gasto)
        {
            return new GastoDto
            {
                Id = gasto.Id,
                Descripcion = gasto.Descripcion,
                Monto = gasto.Monto,
                Fecha = gasto.Fecha,
                Categoria = gasto.Categoria,
                GrupoId = gasto.GrupoId,
                PagadoPorId = gasto.PagadoPorId,
                PagadoPorNombre = gasto.PagadoPor?.Nombre ?? string.Empty
            };
        }
    }
}
