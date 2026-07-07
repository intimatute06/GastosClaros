using BackendCore.Api.DTOs;
using BackendCore.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackendCore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeudasController : ControllerBase
    {
        private readonly IDeudaService _service;

        public DeudasController(IDeudaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeudaDto>>> GetAll()
        {
            var deudas = await _service.GetAllAsync();
            return Ok(deudas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeudaDto>> GetById(int id)
        {
            var deuda = await _service.GetByIdAsync(id);
            if (deuda == null) return NotFound();
            return Ok(deuda);
        }

        [HttpPost]
        public async Task<ActionResult<DeudaDto>> Create(CrearDeudaDto dto)
        {
            var (creada, error) = await _service.CreateAsync(dto);
            if (error != null) return BadRequest(new { mensaje = error });
            return CreatedAtAction(nameof(GetById), new { id = creada!.Id }, creada);
        }

        [HttpPost("{id}/saldar")]
        public async Task<ActionResult<DeudaDto>> Saldar(int id)
        {
            var (deuda, error) = await _service.SaldarAsync(id);

            if (deuda == null) return NotFound(new { mensaje = error });

            if (error != null)
            {
                // La deuda existe pero el pago fallo (estado de negocio, no error tecnico)
                return Ok(new { deuda, mensaje = error });
            }

            return Ok(deuda);
        }
    }
}
