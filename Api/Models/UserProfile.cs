using System;

namespace Api.Models
{
    public class UserProfile
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
    }
}