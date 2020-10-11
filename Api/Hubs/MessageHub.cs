using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        private static List<UserConnected> usersConnected = new List<UserConnected>();

        public MessageHub(IMessageService messageService, IMapper mapper)
        {
            _mapper = mapper;
            _messageService = messageService;
        }

        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Request.Query["userId"].ToString();
            var userConnected = usersConnected.Where(u => u.UserId == userId);
            if (userConnected.Any())
            {
                usersConnected.Remove(userConnected.FirstOrDefault());
            }
            var connect = new UserConnected
            {
                ConnectionID = Context.ConnectionId,
                UserId = userId
            };
            usersConnected.Add(connect);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(Message message)
        {
            if (await _messageService.AddMessage(message))
            {
                var newMessage = await _messageService.GetMessage(message.MessageId);
                var connectionIds = usersConnected.Where(x => x.UserId == message.RecipientId || x.UserId == message.SenderId)
                .Select(x => x.ConnectionID).ToList();
                if (connectionIds != null)
                    await Clients.Clients(connectionIds).SendAsync("NewMessage", newMessage);
            }
        }
    }
}