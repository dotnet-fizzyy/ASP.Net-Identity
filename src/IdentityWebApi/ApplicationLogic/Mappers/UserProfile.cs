using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Entities;

using System.Linq;

namespace IdentityWebApi.ApplicationLogic.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AppUser, UserResultDto>()
            .ForMember(
                dist => dist.Roles,
                ex => ex.MapFrom(en => en.UserRoles.Select(x => x.Role.Name))
            );

        CreateMap<UserDto, AppUser>();
        CreateMap<UserRegistrationDto, AppUser>();
    }
}
