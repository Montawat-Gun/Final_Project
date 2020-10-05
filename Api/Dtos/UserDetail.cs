using System;
using System.Collections.Generic;

namespace Api.Dtos
{
    public class UserDetail
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int FollowingCount { get; set; }
        public int FollowerCount { get; set; }
        public ICollection<GamesToList> GameInterest { get; set; }
    }
}