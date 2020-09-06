using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.Services
{
    public interface IUserService
    {
        Task<IdentityResult> Register(RegisterRequest model);
        Task<LoginResponse> Login(LoginRequest model);
        Task Logout();
        Task<bool> UserExists(string username);
        Task<UserProfile> GetUser(string username);
    }
}