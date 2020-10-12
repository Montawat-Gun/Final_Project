using System.Threading.Tasks;
using Api.Data;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notification;

        public NotificationController(INotificationService notification)
        {
            _notification = notification;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetNotifications(string userId)
        {
            var notifications = await _notification.GetNotifications(userId);
            return Ok(notifications);
        }

        [HttpGet("markasread/{userId}")]
        public async Task<ActionResult> MarkAsRead(string userId)
        {
            await _notification.MarkAsRead(userId);
            return Ok();
        }

    }
}