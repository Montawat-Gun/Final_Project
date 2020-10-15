using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Api.Models
{
    public class Role : IdentityRole
    {
        public ICollection<UserRole> UserRole { get; set; }
    }
}