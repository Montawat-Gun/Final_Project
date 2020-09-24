using System.Threading.Tasks;
using Api.Dtos;

namespace Api.Services
{
    public interface IImageService
    {
        Task<ImageResponse> GetUserImage(string userId);
        Task<ImageResponse> AddUserImage(ImageUserRequest request);
        Task<ImageResponse> UpdateUserImage(ImageUserRequest request);
        Task<ImageResponse> DeleteUserImage(string userId);
    }
}