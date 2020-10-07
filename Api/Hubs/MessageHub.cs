using System.Threading.Tasks;
using Api.Services;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs
{
    public class MessageHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public MessageHub(IMessageService messageService, IMapper mapper)
        {
            _mapper = mapper;
            _messageService = messageService;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var currentUserId = httpContext.Request.Query["currentUserId"].ToString();
            var otherUserId = httpContext.Request.Query["otherUserId"].ToString();
            var groupName = GetGroupName(Context.User.Identity.Name, otherUserId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var messages = await _messageService.GetMessages(currentUserId, otherUserId);
            await Clients.Group(groupName).SendAsync("ReceiveMessages", messages);
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}