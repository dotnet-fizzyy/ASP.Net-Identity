using AutoMapper;

using IdentityWebApi.Core.Entities;
using IdentityWebApi.Presentation.Models.Action;
using IdentityWebApi.Presentation.Models.DTO;

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

        CreateMap<UserActionModel, AppUser>();
        CreateMap<UserRegistrationActionModel, AppUser>();
    }
}
