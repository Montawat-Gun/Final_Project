using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Api.Models
{
    public partial class User : IdentityUser
    {
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string ImgUrl { get; set; }
        public DateTime TimeCreate { get; set; }

        public ICollection<Interest> Interests { get; set; }
        public ICollection<Follow> Follower { get; set; }
        public ICollection<Follow> Following { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}