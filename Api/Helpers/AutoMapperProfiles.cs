using Api.Dtos;
using Api.Models;
using AutoMapper;

namespace Api.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserResponse>();
            CreateMap<User, UserToListDto>();
        }
    }
}