using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private static List<UserConnected> usersConnected = new List<UserConnected>();

        private readonly INotificationService _notificationService;

        public NotificationHub(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public override async Task OnConnectedAsync()
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
            var notificationCount = await _notificationService.GetNotificationCount(userId);
            await Clients.Caller.SendAsync("ReceiveNotificationCount", notificationCount);
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

        public async Task SendNotification(string recipientId, string senderId, string content, string destination)
        {
            var notification = new Notification
            {
                RecipientId = recipientId,
                SenderId = senderId,
                Content = content,
                Destination = destination,
                TimeNotification = DateTime.UtcNow
            };
            await _notificationService.AddNotification(notification);
            var connectionId = usersConnected.Where(x => x.UserId == recipientId).FirstOrDefault().ConnectionID;
            if (connectionId.Any())
                await Clients.Client(connectionId).SendAsync("ReceiveNotification", notification);
        }
    }
}