using System.Threading.Tasks;
using Api.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Api.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> Register(RegisterRequest model);
        Task<LoginResponse> Login(LoginRequest model);
        Task Logout();
    }
}