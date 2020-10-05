using System;

namespace Api.Dtos
{
    public class CommentDetail
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime TimeComment { get; set; }
        public UserToList User { get; set; }
    }
}