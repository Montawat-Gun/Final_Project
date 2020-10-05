using System;

namespace Api.Dtos
{
    public class UserEditRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
    }
}