using System.Linq;
using Api.Dtos;
using Api.Models;
using AutoMapper;

namespace Api.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(src => src.Image.ImageUrl));
            CreateMap<User, UserToListDto>()
                .ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(src => src.Image.ImageUrl));
            CreateMap<ImageUserRequest, UserImage>();
            CreateMap<Image, ImageResponse>();
            CreateMap<UserEditRequest, User>();
            CreateMap<Game,GameToReturn>()
                .ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(src => src.Image.ImageUrl));
        }
    }
}