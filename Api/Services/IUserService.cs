using System.Threading.Tasks;
using Api.Models;
using Api.Dtos;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserToList>> GetUsers(string userId);
        Task<UserDetail> GetUserById(string id);
        Task<UserDetail> GetUserByUsername(string username);
        Task<IdentityResult> UpdateUser(string userId, UserEditRequest userToEdit);
        Task<IdentityResult> UpdateUserPassword(string userId, UserEditPasswordRequest passwordToEdit);
        Task<ICollection<UserToList>> Suggest(string username);
        Task<UserDetail> DeleteUser(string id);
        Task<IEnumerable<UserToList>> SearchUser(string userId, string searchString);
    }
}