using Microsoft.AspNetCore.Mvc;
using Data.Servicios;

namespace API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var paciente = await _authService.LoginAsync(request.Documento, request.FechaNacimiento);

            if (paciente == null)
                return Unauthorized("Credenciales incorrectas");

            return Ok(paciente);
        }
    }

    public class LoginRequest
    {
        public string Documento { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}