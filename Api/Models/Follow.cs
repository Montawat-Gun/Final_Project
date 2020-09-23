using System;
using System.Collections.Generic;

namespace Api.Models
{
    public class Follow
    {
        public string FollowerId { get; set; }
        public virtual User Follower { get; set; }
        public string FollowingId { get; set; }
        public virtual User Following { get; set; }
        public DateTime TimeFollow { get; set; }
    }
}