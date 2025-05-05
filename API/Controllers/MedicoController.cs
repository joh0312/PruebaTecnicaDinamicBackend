using Microsoft.AspNetCore.Mvc;
using Models.Entidades;
using Data.Servicios;

namespace API.Controllers
{
    [ApiController]
    [Route("api/medicos")]
    public class MedicoController : ControllerBase
    {
        private readonly MedicoService _medicoService;

        public MedicoController(MedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        [HttpGet("especialidades/{especialidad}")]
        public async Task<IActionResult> ObtenerMedicosPorEspecializacion(string especialidad)
        {
            if (string.IsNullOrWhiteSpace(especialidad))
            {
                return BadRequest(new { Message = "La especialidad es obligatoria." });
            }

            var medicos = await _medicoService.ObtenerMedicosPorEspecializacionAsync(especialidad);

            if (medicos == null || medicos.Count == 0)
            {
                return NotFound(new { Message = "No se encontraron médicos para la especialidad proporcionada." });
            }

            return Ok(medicos);
        }
    }
}