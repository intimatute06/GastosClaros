using BackendCore.Api.DTOs;
using BackendCore.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackendCore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MiembrosController : ControllerBase
    {
        private readonly IMiembroService _service;

        public MiembrosController(IMiembroService service)
        {
            _service = service;
        }

        [HttpGet("grupo/{grupoId}")]
        public async Task<ActionResult<IEnumerable<MiembroDto>>> GetByGrupo(int grupoId)
        {
            var miembros = await _service.GetByGrupoIdAsync(grupoId);
            return Ok(miembros);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MiembroDto>> GetById(int id)
        {
            var miembro = await _service.GetByIdAsync(id);
            if (miembro == null) return NotFound();
            return Ok(miembro);
        }

        [HttpPost]
        public async Task<ActionResult<MiembroDto>> Create(CrearMiembroDto dto)
        {
            var (creado, error) = await _service.CreateAsync(dto);
            if (error != null) return BadRequest(new { mensaje = error });
            return CreatedAtAction(nameof(GetById), new { id = creado!.Id }, creado);
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
