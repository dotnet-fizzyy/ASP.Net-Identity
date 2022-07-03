using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Entities;

using System.Linq;

namespace IdentityWebApi.ApplicationLogic.Mappers;

/// <summary>
/// Configuration of User models and entities mapping.
/// </summary>
public class UserProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserProfile"/> class.
    /// </summary>
    public UserProfile()
    {
        this.CreateMap<AppUser, UserResultDto>()
            .ForMember(
                dist => dist.Roles,
                opts =>
                {
                    opts.PreCondition(en => en.UserRoles != null && en.UserRoles.Any());
                    opts.MapFrom(en => en.UserRoles.Select(x => x.Role.Name));
                });

        this.CreateMap<UserDto, AppUser>();
        this.CreateMap<UserRegistrationDto, AppUser>();
    }
}
