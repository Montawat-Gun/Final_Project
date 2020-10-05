using Api.Dtos;
using Api.Models;
using AutoMapper;
using System.Linq;

namespace Api.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDetail>()
                .ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(src => src.Image.ImageUrl))
                .ForMember(dest => dest.GameInterest,
                opt => opt.MapFrom(src => src.Interests.Select(g => g.Game)));
            CreateMap<User, UserToList>()
                .ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(src => src.Image.ImageUrl));
            CreateMap<ImageUserRequest, UserImage>();
            CreateMap<ImagePostRequest, PostImage>();
            CreateMap<Image, ImageResponse>();
            CreateMap<UserEditRequest, User>();
            CreateMap<Tag, TagToReturn>();
            CreateMap<Game, GamesToList>()
                .ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(src => src.Image.ImageUrl));
            CreateMap<Post, PostToList>()
                .ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(src => src.Image.ImageUrl))
                .ForMember(dest => dest.User,
                opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Game,
                opt => opt.MapFrom(src => src.Game))
                .ForMember(dest => dest.LikeCount,
                opt => opt.MapFrom(scr => scr.Likes.Count))
                .ForMember(dest => dest.CommentCount,
                opt => opt.MapFrom(scr => scr.Comments.Count));
            CreateMap<Post, PostDetail>()
                .ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(src => src.Image.ImageUrl))
                .ForMember(dest => dest.User,
                opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Game,
                opt => opt.MapFrom(src => src.Game))
                .ForMember(dest => dest.LikeCount,
                opt => opt.MapFrom(scr => scr.Likes.Count))
                .ForMember(dest => dest.CommentCount,
                opt => opt.MapFrom(scr => scr.Comments.Count))
                .ForMember(dest => dest.Comments,
                opt => opt.MapFrom(scr => scr.Comments))
                .ForMember(dest => dest.Likes,
                opt => opt.MapFrom(scr => scr.Likes));

            CreateMap<Comment, CommentDetail>()
                .ForMember(dest => dest.User,
                opt => opt.MapFrom(src => src.User));
            CreateMap<Like, LikeDetail>()
                .ForMember(dest => dest.User,
                opt => opt.MapFrom(src => src.User));
        }
    }
}