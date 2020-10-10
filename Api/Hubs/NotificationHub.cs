using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private static List<UserConnected> usersConnected = new List<UserConnected>();

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
            var userConnected = usersConnected.Where(u => u.ConnectionID == Context.ConnectionId).FirstOrDefault();
            if (userConnected != null)
            {
                usersConnected.Remove(userConnected);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(string userId, string message)
        {
            var connectionId = usersConnected.Where(x => x.UserId == userId).FirstOrDefault().ConnectionID;
            if (connectionId != null)
                await Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
        }
    }
}