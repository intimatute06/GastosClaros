using BackendCore.Api.DTOs;
using BackendCore.Api.Models;
using BackendCore.Api.Repositories.Interfaces;
using BackendCore.Api.Services.Interfaces;

namespace BackendCore.Api.Services
{
    public class DeudaService : IDeudaService
    {
        private readonly IDeudaRepository _repository;
        private readonly IPagoLambdaClient _pagoLambdaClient;

        public DeudaService(IDeudaRepository repository, IPagoLambdaClient pagoLambdaClient)
        {
            _repository = repository;
            _pagoLambdaClient = pagoLambdaClient;
        }

        public async Task<IEnumerable<DeudaDto>> GetAllAsync()
        {
            var deudas = await _repository.GetAllAsync();
            return deudas.Select(MapToDto);
        }

        public async Task<DeudaDto?> GetByIdAsync(int id)
        {
            var deuda = await _repository.GetByIdAsync(id);
            return deuda == null ? null : MapToDto(deuda);
        }

        public async Task<(DeudaDto? deuda, string? error)> CreateAsync(CrearDeudaDto dto)
        {
            if (dto.DeudorId == dto.AcreedorId)
            {
                return (null, "El deudor y el acreedor no pueden ser el mismo miembro.");
            }

            if (!await _repository.MiembroExisteAsync(dto.DeudorId))
            {
                return (null, "El deudor especificado no existe.");
            }

            if (!await _repository.MiembroExisteAsync(dto.AcreedorId))
            {
                return (null, "El acreedor especificado no existe.");
            }

            if (dto.Monto <= 0)
            {
                return (null, "El monto debe ser mayor a cero.");
            }

            var deuda = new Deuda
            {
                DeudorId = dto.DeudorId,
                AcreedorId = dto.AcreedorId,
                Monto = dto.Monto,
                Estado = EstadoDeuda.Pendiente
            };

            var creada = await _repository.CreateAsync(deuda);
            return (MapToDto(creada), null);
        }

        public async Task<(DeudaDto? deuda, string? error)> SaldarAsync(int id)
        {
            var deuda = await _repository.GetByIdAsync(id);
            if (deuda == null)
            {
                return (null, "La deuda especificada no existe.");
            }

            if (deuda.Estado == EstadoDeuda.Saldada)
            {
                return (null, "La deuda ya fue saldada previamente.");
            }

            await _repository.ActualizarEstadoAsync(id, EstadoDeuda.EnProceso, null);

            var resultado = await _pagoLambdaClient.ProcesarPagoAsync(deuda.DeudorId, deuda.AcreedorId, deuda.Monto);

            var nuevoEstado = resultado.Exitoso ? EstadoDeuda.Saldada : EstadoDeuda.Fallida;
            var actualizada = await _repository.ActualizarEstadoAsync(id, nuevoEstado, resultado.ReferenciaTransaccion);

            if (!resultado.Exitoso)
            {
                return (actualizada == null ? null : MapToDto(actualizada), resultado.Mensaje);
            }

            return (actualizada == null ? null : MapToDto(actualizada), null);
        }

        private static DeudaDto MapToDto(Deuda deuda)
        {
            return new DeudaDto
            {
                Id = deuda.Id,
                DeudorId = deuda.DeudorId,
                AcreedorId = deuda.AcreedorId,
                Monto = deuda.Monto,
                Estado = deuda.Estado.ToString(),
                FechaCreacion = deuda.FechaCreacion,
                FechaSaldada = deuda.FechaSaldada,
                ReferenciaTransaccion = deuda.ReferenciaTransaccion
            };
        }
    }
}
