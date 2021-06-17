using AutoMapper;
using IdentityWebApi.DAL.Entities;
using IdentityWebApi.PL.DTO;

namespace IdentityWebApi.BL.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}