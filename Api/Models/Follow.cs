using System;
using System.Collections.Generic;

namespace Api.Models
{
    public class Follow
    {
        public string FollowerId { get; set; }
        public User Follower { get; set; }
        public string FollowingId { get; set; }
        public User Following { get; set; }
        public DateTime TimeFollow { get; set; }
    }
}