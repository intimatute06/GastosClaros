using BackendCore.Api.DTOs;
using BackendCore.Api.Models;
using BackendCore.Api.Repositories.Interfaces;
using BackendCore.Api.Services.Interfaces;

namespace BackendCore.Api.Services
{
    // Evento publicado a RabbitMQ cuando se registra un gasto
    public record GastoRegistradoEvento(int GastoId, int GrupoId, decimal Monto, string Descripcion, DateTime Fecha);

    public class GastoService : IGastoService
    {
        private readonly IGastoRepository _repository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ISaldoService _saldoService;
        private const string RoutingKey = "gasto.registrado";

        public GastoService(IGastoRepository repository, IEventPublisher eventPublisher, ISaldoService saldoService)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
            _saldoService = saldoService;
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

            // Cada gasto nuevo cambia los saldos del grupo, asi que se recalculan de inmediato
            await _saldoService.RecalcularYObtenerAsync(creado.GrupoId);

            return (MapToDto(creado), null);
        }

        public async Task<bool> ActualizarCategoriaAsync(int gastoId, string categoria)
        {
            return await _repository.ActualizarCategoriaAsync(gastoId, categoria);
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
