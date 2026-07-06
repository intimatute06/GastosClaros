using BackendCore.Api.DTOs;
using BackendCore.Api.Models;
using BackendCore.Api.Repositories.Interfaces;
using BackendCore.Api.Services.Interfaces;

namespace BackendCore.Api.Services
{
    public class MiembroService : IMiembroService
    {
        private readonly IMiembroRepository _repository;

        public MiembroService(IMiembroRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MiembroDto>> GetByGrupoIdAsync(int grupoId)
        {
            var miembros = await _repository.GetByGrupoIdAsync(grupoId);
            return miembros.Select(MapToDto);
        }

        public async Task<MiembroDto?> GetByIdAsync(int id)
        {
            var miembro = await _repository.GetByIdAsync(id);
            return miembro == null ? null : MapToDto(miembro);
        }

        public async Task<(MiembroDto? miembro, string? error)> CreateAsync(CrearMiembroDto dto)
        {
            var grupoExiste = await _repository.GrupoExisteAsync(dto.GrupoId);
            if (!grupoExiste)
            {
                return (null, "El grupo especificado no existe.");
            }

            var miembro = new Miembro
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                GrupoId = dto.GrupoId
            };

            var creado = await _repository.CreateAsync(miembro);
            return (creado == null ? null : MapToDto(creado), null);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static MiembroDto MapToDto(Miembro miembro)
        {
            return new MiembroDto
            {
                Id = miembro.Id,
                Nombre = miembro.Nombre,
                Email = miembro.Email,
                GrupoId = miembro.GrupoId
            };
        }
    }
}
