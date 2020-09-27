using System.Threading.Tasks;
using Api.Models;
using Api.Dtos;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserToListDto>> GetUsers();
        Task<UserResponse> GetUserById(string id);
        Task<UserResponse> GetUserByUsername(string username);
        Task<IdentityResult> UpdateUser(string userId, UserEditRequest userToEdit);
        Task<ICollection<UserToListDto>> Suggest(string username);
        Task<UserResponse> DeleteUser(string id);
    }
}