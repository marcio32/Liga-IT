using Liga_IT.Application.DTOs;
using Liga_IT.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Liga_IT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService): ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequestDto authRequestDto)
        {
            var response = await authService.LoginAsync(authRequestDto);
            return Ok(response);
        }
    }
}
