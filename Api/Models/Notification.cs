using System;

namespace Api.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Content { get; set; }
        public DateTime TimeNotification { get; set; }
        public bool IsRead { get; set; }
    }
}