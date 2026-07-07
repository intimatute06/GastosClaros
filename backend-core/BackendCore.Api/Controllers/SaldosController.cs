using BackendCore.Api.DTOs;
using BackendCore.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackendCore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaldosController : ControllerBase
    {
        private readonly ISaldoService _service;

        public SaldosController(ISaldoService service)
        {
            _service = service;
        }

        [HttpGet("grupo/{grupoId}")]
        public async Task<ActionResult<IEnumerable<SaldoDto>>> GetByGrupo(int grupoId)
        {
            var saldos = await _service.ObtenerAsync(grupoId);
            return Ok(saldos);
        }

        [HttpPost("grupo/{grupoId}/recalcular")]
        public async Task<ActionResult<IEnumerable<SaldoDto>>> Recalcular(int grupoId)
        {
            var saldos = await _service.RecalcularYObtenerAsync(grupoId);
            return Ok(saldos);
        }
    }
}
