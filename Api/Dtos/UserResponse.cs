using System;

namespace Api.Dtos
{
    public class UserResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
    }
}