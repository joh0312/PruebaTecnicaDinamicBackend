using Microsoft.AspNetCore.Mvc;
using Data.Servicios;

namespace API.Controllers
{
    [ApiController]
    [Route("api/citas")]
    public class CitaController : ControllerBase
    {
        private readonly CitaService _citaService;

        public CitaController(CitaService citaService)
        {
            _citaService = citaService;
        }

        [HttpPost("reservar")]
        public async Task<IActionResult> ReservarCita([FromBody] ReservarCitaRequest request)
        {
            try
            {
                await _citaService.ReservarCitaAsync(request.IdCita, request.IdPaciente);
                return Ok("Cita reservada con éxito");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al reservar la cita.", Details = ex.Message });
            }
        }

        public class ReservarCitaRequest
        {
            public int IdCita { get; set; }
            public int IdPaciente { get; set; }
        }
    }
}