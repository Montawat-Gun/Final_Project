using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Api.Dtos;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IImageService _imageService;

        public UserController(IUserService userService, IImageService imageService)
        {
            _imageService = imageService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }

        [HttpGet("Id/{id}")]
        public async Task<ActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet("Username/{username}")]
        public async Task<ActionResult> GetUserByUsername(string username)
        {
            var user = await _userService.GetUserByUsername(username);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, UserEditRequest userToEdit)
        {
            var userResponse = await _userService.UpdateUser(userId, userToEdit);
            if (userResponse.Succeeded)
                return Ok(userToEdit);
            return BadRequest(userResponse.Errors);
        }

        [HttpGet("Suggest/{userId}")]
        public async Task<ActionResult> Suggest(string userId)
        {
            var users = await _userService.Suggest(userId);
            if (users != null)
                return Ok(users);
            else return BadRequest();
        }

        [HttpGet("{userId}/image", Name = "GetUserImage")]
        public async Task<IActionResult> GetUserImage(string userId)
        {
            var image = await _imageService.GetUserImage(userId);
            return Ok(image);
        }

        [HttpPost("{userId}/image")]
        public async Task<IActionResult> PostUserImage(string userId, [FromForm] ImageUserRequest imageRequest)
        {
            imageRequest.UserId = userId;
            var image = await _imageService.UpdateUserImage(imageRequest);
            return CreatedAtRoute("GetUserImage", new { userId = userId, id = image.ImageId }, image);
        }

        [HttpDelete("{userId}/image")]
        public async Task<IActionResult> DeleteUserImage(string userId)
        {
            var image = await _imageService.DeleteUserImage(userId);
            if (image == null)
            {
                return BadRequest("Can't delete image.");
            }
            return Ok(image);
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
