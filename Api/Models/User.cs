using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Api.Models
{
    public partial class User : IdentityUser
    {
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public DateTime TimeCreate { get; set; }
        public string Description { get; set; }

        public virtual UserImage Image { get; set; }
        public virtual ICollection<Interest> Interests { get; set; }
        public virtual ICollection<Follow> Follower { get; set; }
        public virtual ICollection<Follow> Following { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Message> MessagesSent { get; set; }
        public virtual ICollection<Message> MessagesReceived { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}