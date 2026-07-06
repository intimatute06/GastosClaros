using BackendCore.Api.DTOs;
using BackendCore.Api.Models;
using BackendCore.Api.Repositories.Interfaces;
using BackendCore.Api.Services.Interfaces;

namespace BackendCore.Api.Services
{
    public class GrupoService : IGrupoService
    {
        private readonly IGrupoRepository _repository;

        public GrupoService(IGrupoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<GrupoDto>> GetAllAsync()
        {
            var grupos = await _repository.GetAllAsync();
            return grupos.Select(MapToDto);
        }

        public async Task<GrupoDto?> GetByIdAsync(int id)
        {
            var grupo = await _repository.GetByIdAsync(id);
            return grupo == null ? null : MapToDto(grupo);
        }

        public async Task<GrupoDto> CreateAsync(CrearGrupoDto dto)
        {
            var grupo = new Grupo { Nombre = dto.Nombre };
            var creado = await _repository.CreateAsync(grupo);
            return MapToDto(creado);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static GrupoDto MapToDto(Grupo grupo)
        {
            return new GrupoDto
            {
                Id = grupo.Id,
                Nombre = grupo.Nombre,
                FechaCreacion = grupo.FechaCreacion,
                CantidadMiembros = grupo.Miembros?.Count ?? 0
            };
        }
    }
}
