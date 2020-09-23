using System;
namespace Api.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime TimeComment { get; set; }

        public virtual Image Image { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}