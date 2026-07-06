using BackendCore.Api.DTOs;
using BackendCore.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackendCore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GruposController : ControllerBase
    {
        private readonly IGrupoService _service;

        public GruposController(IGrupoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GrupoDto>>> GetAll()
        {
            var grupos = await _service.GetAllAsync();
            return Ok(grupos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GrupoDto>> GetById(int id)
        {
            var grupo = await _service.GetByIdAsync(id);
            if (grupo == null) return NotFound();
            return Ok(grupo);
        }

        [HttpPost]
        public async Task<ActionResult<GrupoDto>> Create(CrearGrupoDto dto)
        {
            var creado = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _service.DeleteAsync(id);
            if (!eliminado) return NotFound();
            return NoContent();
        }
    }
}
