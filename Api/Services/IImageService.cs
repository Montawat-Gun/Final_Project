using System.Threading.Tasks;
using Api.Dtos;
using Api.Models;

namespace Api.Services
{
    public interface IImageService
    {
        Task<ImageResponse> GetUserImage(string userId);
        Task<ImageResponse> AddUserImage(ImageUserRequest request);
        Task<ImageResponse> UpdateUserImage(ImageUserRequest request);
        Task<ImageResponse> DeleteUserImage(string userId);
        Task<ImageResponse> GetPostImage(int postId);
        Task<ImageResponse> AddPostImage(ImagePostRequest request);
        Task<ImageResponse> DeletePostImage(int postId);
        Task<ImageResponse> GetGameImage(int gameId);
        Task<ImageResponse> AddGameImage(ImageGameRequest request);
        Task<ImageResponse> UpdateGameImage(ImageGameRequest request);
        Task<ImageResponse> DeleteGameImage(GameImage image);
    }
}