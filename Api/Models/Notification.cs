using System;

namespace Api.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string RecipientId { get; set; }
        public virtual User Recipient { get; set; }
        public string SenderId { get; set; }
        public virtual User Sender { get; set; }
        public string Content { get; set; }
        public string Destination { get; set; }
        public DateTime TimeNotification { get; set; }
        public bool IsRead { get; set; }
    }
}