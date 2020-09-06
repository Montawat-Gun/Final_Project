using System.Threading.Tasks;
using Api.Services;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var user = await _userService.Login(model);
            if (user != null)
                return Ok(user);
            return BadRequest("Incorrect username or password!");
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            return Ok("Logout");
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            var result = await _userService.Register(model);
            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(string username)
        {
            var user = await _userService.GetUser(username);
            if (user != null)
            {
                return Ok(user);
            }
            else return BadRequest();
        }
    }
}