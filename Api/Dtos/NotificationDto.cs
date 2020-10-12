using System;

namespace Api.Dtos
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public string SenderId { get; set; }
        public UserToList Sender { get; set; }
        public string Content { get; set; }
        public string Destination { get; set; }
        public DateTime TimeNotification { get; set; }
        public bool IsRead { get; set; }
    }
}