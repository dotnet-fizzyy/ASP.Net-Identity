using System.Linq;
using AutoMapper;
using IdentityWebApi.DAL.Entities;
using IdentityWebApi.PL.Models.Action;
using IdentityWebApi.PL.Models.DTO;

namespace IdentityWebApi.BL.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, UserDto>()
                .ForMember(
                    dist => dist.UserRole, 
                    ex => ex.MapFrom(en => en.UserRoles.Any() ? en.UserRoles.FirstOrDefault().AppRole.Name : null)
                );
            CreateMap<UserDto, AppRole>();
            CreateMap<UserRegistrationActionModel, AppUser>();
        }
    }
}