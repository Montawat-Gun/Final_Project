using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Api.Models
{
    public partial class User : IdentityUser
    {
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public ICollection<Interest> Interests { get; set; }
    }
}