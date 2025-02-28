using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Data;
using Api.Dtos;
using Api.Helpers;
using Api.Models;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Api.Services
{
    public class ImageService : IImageService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public ImageService(IOptions<CloudinarySettings> cloudinaryConfig, DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _cloudinaryConfig = cloudinaryConfig;

            Account account = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }

        public async Task<ImageResponse> GetUserImage(string userId)
        {
            var image = await _context.UserImages.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            return _mapper.Map<ImageResponse>(image);
        }

        public async Task<ImageResponse> AddUserImage(ImageUserRequest request)
        {
            var file = request.File;
            var uploadResult = await UploadImage(file);
            request.ImageUrl = uploadResult.Url.ToString();
            request.PublicId = uploadResult.PublicId;
            request.TimeImage = DateTime.UtcNow;
            var image = _mapper.Map<UserImage>(request);
            _context.UserImages.Add(image);
            await _context.SaveChangesAsync();
            return _mapper.Map<ImageResponse>(image);
        }

        public async Task<ImageResponse> UpdateUserImage(ImageUserRequest request)
        {
            var image = await _context.UserImages.Where(u => u.UserId == request.UserId).FirstOrDefaultAsync();
            if (image == null)
                return await AddUserImage(request);

            var file = request.File;
            var uploadResult = await UploadImage(file);
            request.ImageUrl = uploadResult.Url.ToString();
            request.PublicId = uploadResult.PublicId;
            request.TimeImage = DateTime.UtcNow;
            if (uploadResult.Error != null)
                return null;
            await DeleteImage(image.PublicId);
            _mapper.Map(request, image);
            _context.Entry(image).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return _mapper.Map<ImageResponse>(image);
        }

        public async Task<ImageResponse> DeleteUserImage(string userId)
        {
            var image = await _context.UserImages.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            if (image == null)
            {
                return null;
            }
            var result = await DeleteImage(image.PublicId);
            _context.UserImages.Remove(image);
            await _context.SaveChangesAsync();
            return _mapper.Map<ImageResponse>(image);
        }

        public async Task<ImageResponse> GetPostImage(int postId)
        {
            var image = await _context.PostImages.Where(p => p.PostId == postId).FirstOrDefaultAsync();
            return _mapper.Map<ImageResponse>(image);
        }

        public async Task<ImageResponse> AddPostImage(ImagePostRequest request)
        {
            var file = request.File;
            var uploadResult = await UploadImage(file);
            request.ImageUrl = uploadResult.Url.ToString();
            request.PublicId = uploadResult.PublicId;
            request.TimeImage = DateTime.UtcNow;
            var image = _mapper.Map<PostImage>(request);
            _context.PostImages.Add(image);
            await _context.SaveChangesAsync();
            return _mapper.Map<ImageResponse>(image);
        }

        public async Task<ImageResponse> DeletePostImage(int postId)
        {
            var image = await _context.PostImages.Where(p => p.PostId == postId).FirstOrDefaultAsync();
            if (image == null)
            {
                return null;
            }
            var result = await DeleteImage(image.PublicId);
            _context.PostImages.Remove(image);
            await _context.SaveChangesAsync();
            return _mapper.Map<ImageResponse>(image);
        }

        public async Task<ImageResponse> GetGameImage(int gameId)
        {
            var image = await _context.GameImages.Where(p => p.GameId == gameId).FirstOrDefaultAsync();
            return _mapper.Map<ImageResponse>(image);
        }

        public async Task<ImageResponse> AddGameImage(ImageGameRequest request)
        {
            var file = request.File;
            var uploadResult = await UploadImage(file);
            request.ImageUrl = uploadResult.Url.ToString();
            request.PublicId = uploadResult.PublicId;
            request.TimeImage = DateTime.UtcNow;
            var image = _mapper.Map<GameImage>(request);
            _context.GameImages.Add(image);
            await _context.SaveChangesAsync();
            return _mapper.Map<ImageResponse>(image);
        }

        public async Task<ImageResponse> UpdateGameImage(ImageGameRequest request)
        {
            var image = await _context.GameImages.Where(u => u.GameId == request.GameId).FirstOrDefaultAsync();
            if (image == null)
                return await AddGameImage(request);

            var file = request.File;
            var uploadResult = await UploadImage(file);
            request.ImageUrl = uploadResult.Url.ToString();
            request.PublicId = uploadResult.PublicId;
            request.TimeImage = DateTime.UtcNow;
            if (uploadResult.Error != null)
                return null;
            await DeleteImage(image.PublicId);
            _mapper.Map(request, image);
            _context.Entry(image).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return _mapper.Map<ImageResponse>(image);
        }

        public async Task<ImageResponse> DeleteGameImage(GameImage image)
        {
            var result = await DeleteImage(image.PublicId);
            return _mapper.Map<ImageResponse>(image);
        }

        public async Task<ImageUploadResult> UploadImage(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Quality("auto").FetchFormat("auto")
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
            }
            return uploadResult;
        }

        public async Task<DelResResult> DeleteImage(string publicId)
        {
            var result = await _cloudinary.DeleteResourcesAsync(ResourceType.Image, publicId);
            return result;
        }
    }
}