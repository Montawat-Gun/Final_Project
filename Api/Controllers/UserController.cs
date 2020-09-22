using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Api.Dtos;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly DataContext _context;

        public UserController(IUserService userService, DataContext context)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult> GetUser(string username)
        {
            var user = await _userService.GetUser(username);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet("Suggest/{userId}")]
        public async Task<ActionResult> Suggest(string userId)
        {
            var users = await _userService.Suggest(userId);
            if (users != null)
                return Ok(users);
            else return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var user = await _userService.DeleteUser(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
    }
}
