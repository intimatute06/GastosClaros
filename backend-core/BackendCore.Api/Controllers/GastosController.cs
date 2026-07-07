using BackendCore.Api.DTOs;
using BackendCore.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackendCore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GastosController : ControllerBase
    {
        private readonly IGastoService _service;

        public GastosController(IGastoService service)
        {
            _service = service;
        }

        [HttpGet("grupo/{grupoId}")]
        public async Task<ActionResult<IEnumerable<GastoDto>>> GetByGrupo(int grupoId)
        {
            var gastos = await _service.GetByGrupoIdAsync(grupoId);
            return Ok(gastos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GastoDto>> GetById(int id)
        {
            var gasto = await _service.GetByIdAsync(id);
            if (gasto == null) return NotFound();
            return Ok(gasto);
        }

        [HttpPost]
        public async Task<ActionResult<GastoDto>> Create(CrearGastoDto dto)
        {
            var (creado, error) = await _service.CreateAsync(dto);
            if (error != null) return BadRequest(new { mensaje = error });
            return CreatedAtAction(nameof(GetById), new { id = creado!.Id }, creado);
        }
    }
}
