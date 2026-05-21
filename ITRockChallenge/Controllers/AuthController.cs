using ITRockChallenge.DTOs;
using ITRockChallenge.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITRockChallenge.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public AuthController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login(LoginRequestDto request)
        {
            if (request.Username != "admin" ||
                request.Password != "password123")
            {
                return Unauthorized(new
                {
                    message = "Credenciales inválidas"
                });
            }

            var token = _jwtService.GenerateToken(request.Username);

            return Ok(new
            {
                token
            });
        }
    }
}
