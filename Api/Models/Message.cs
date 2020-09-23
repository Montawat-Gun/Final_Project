using System;

namespace Api.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string SenderId { get; set; }
        public User Sender { get; set; }
        public string RecipientId { get; set; }
        public User Recipient { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime TimeSend { get; set; }
        public bool IsSenderDelete { get; set; }
        public bool IsRecipientDelete { get; set; }
    }
}