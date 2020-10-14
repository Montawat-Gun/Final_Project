using Api.Data;
using Api.Models;
using Api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Web;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _message;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MessageController(DataContext context, IMapper mapper, IMessageService message)
        {
            _mapper = mapper;
            _context = context;
            _message = message;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetMessage(int id)
        {
            var message = await _message.GetMessage(id);
            if (message == null)
                return NotFound();
            return Ok(message);
        }

        [HttpGet("{userId}/{otherUserId}")]
        public async Task<ActionResult> GetMessages(string userId, string otherUserId)
        {
            var messages = await _message.GetMessages(userId, otherUserId);
            if (messages == null)
                return NotFound();
            return Ok(messages);
        }

        [HttpGet("contact/{userId}")]
        public async Task<ActionResult> GetContact(string userId)
        {
            var contacts = await _message.GetUserContacts(userId);
            return Ok(contacts);
        }

        [HttpPost]
        public async Task<ActionResult> PostMessage(Message message)
        {
            await _message.AddMessage(message);
            return CreatedAtAction("GetMessage", new { id = message.MessageId }, message);
        }

        [HttpGet("markasread/{userId}/{otherUserId}")]
        public async Task<ActionResult> MarkAsRead(string userId, string otherUserId)
        {
            var result = await _message.MarkAsRead(userId, otherUserId);
            return Ok();
        }

        [HttpDelete("{currentUserId}/{otherUserId}")]
        public async Task<ActionResult> DeleteMessage(string currentUserId, string otherUserId)
        {
            await _message.DeleteMessages(currentUserId, otherUserId);
            return Ok();
        }
    }
}