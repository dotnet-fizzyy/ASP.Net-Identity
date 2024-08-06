using AutoMapper;

using DY.Auth.Identity.Api.ApplicationLogic.Models.Action;
using DY.Auth.Identity.Api.ApplicationLogic.Models.Output;
using DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.CreateUser;
using DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.UpdateUser;
using DY.Auth.Identity.Api.Core.Constants;
using DY.Auth.Identity.Api.Core.Entities;
using DY.Auth.Identity.Api.Core.Utilities;

using System.Linq;

namespace DY.Auth.Identity.Api.ApplicationLogic.Mappers;

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

        this.CreateMap<UserRegistrationDto, CreateUserCommand>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(_ => UserRoleConstants.User));

        this.CreateMap<UserDto, CreateUserCommand>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

        this.CreateMap<UserDto, UpdateUserCommand>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp));

        this.CreateMap<UpdateUserCommand, AppUser>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp));
    }
}
