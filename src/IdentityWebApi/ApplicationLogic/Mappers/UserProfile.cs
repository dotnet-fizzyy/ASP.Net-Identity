using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Output;
using IdentityWebApi.ApplicationLogic.Services.User.Commands.CreateUser;
using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Utilities;

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
        this.CreateMap<AppUser, UserResult>()
            .ForMember(
                dest => dest.Roles,
                opts =>
                {
                    opts.PreCondition(appUser => !appUser.UserRoles.IsNullOrEmpty());
                    opts.MapFrom(appUser => appUser.UserRoles.Select(appUserRole => appUserRole.Role.Name));
                });

        this.CreateMap<CreateUserCommand, AppUser>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
    }
}
