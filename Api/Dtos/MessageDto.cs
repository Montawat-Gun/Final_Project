using System;

namespace Api.Dtos
{
    public class MessageDto
    {
        public int MessageId { get; set; }
        public string SenderId { get; set; }
        public virtual UserToList Sender { get; set; }
        public string RecipientId { get; set; }
        public virtual UserToList Recipient { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime TimeSend { get; set; }
    }
}