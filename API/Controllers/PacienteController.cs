using Microsoft.AspNetCore.Mvc;
using Models.Entidades;
using Data.Servicios;

namespace API.Controllers
{
    [ApiController]
    [Route("api/pacientes")]
    public class PacienteController : ControllerBase
    {
        private readonly PacienteService _pacienteService;

        public PacienteController(PacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        [HttpPost]
        public async Task<IActionResult> InsertarPaciente([FromBody] Paciente paciente)
        {
            // Validar el modelo recibido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Llamar al servicio para insertar el paciente
            var id = await _pacienteService.InsertarPacienteAsync(paciente.Documento, paciente.FechaNacimiento, paciente.Nombre, paciente.Apellidos, paciente.Telefono);

            // Devolver el ID del paciente insertado
            return Ok(new { Id = id });
        }


        [HttpGet("{documento}")]
        public async Task<IActionResult> ObtenerPaciente(string documento)
        {
            var paciente = await _pacienteService.ObtenerPacientePorDocumentoAsync(documento);
            if (paciente == null)
                return NotFound();

            return Ok(paciente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPaciente(int id, [FromBody] Paciente paciente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _pacienteService.ActualizarPacienteAsync(
                    id,
                    paciente.Documento,
                    paciente.FechaNacimiento,
                    paciente.Nombre,
                    paciente.Apellidos,
                    paciente.Telefono
                );

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al actualizar el paciente.", Details = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPaciente(int id)
        {
            await _pacienteService.EliminarPacienteAsync(id);
            return NoContent();
        }
    }
}

