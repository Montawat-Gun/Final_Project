using System;
using System.Collections.Generic;

namespace Api.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime TimePost { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int GameId { get; set; }
        public virtual Game Game { get; set; }

        public virtual PostImage Image { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}