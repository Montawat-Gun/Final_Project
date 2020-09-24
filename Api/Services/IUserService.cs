using System.Threading.Tasks;
using Api.Models;
using Api.Dtos;
using System.Collections.Generic;

namespace Api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserToListDto>> GetUsers();
        Task<UserResponse> GetUser(string username);
        Task<UserResponse> UpdateUser(string userId, UserEditRequest userToEdit);
        Task<ICollection<UserToListDto>> Suggest(string username);
        Task<UserResponse> DeleteUser(string id);
    }
}