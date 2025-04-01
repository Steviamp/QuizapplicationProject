using Microsoft.AspNetCore.Mvc;
using quizapplication.Models;
using quizapplication.Services.Interfaces;

namespace quizapplication.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            if (await _authService.ValidateLoginAsync(user))
            {
                return Ok(new { success = true, message = "Login successful" });
            }

            return BadRequest(new { success = false, message = "Invalid username or password" });
        }
    }
}

