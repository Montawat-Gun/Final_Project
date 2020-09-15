using System.Threading.Tasks;
using Api.Services;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Api.Dtos;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var user = await _authService.Login(model);
            if (user != null)
                return Ok(user);
            return BadRequest("Incorrect username or password!");
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return Ok("Logout");
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            var result = await _authService.Register(model);
            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(result.Errors);
        }
    }
}