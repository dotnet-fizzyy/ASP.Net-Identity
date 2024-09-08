using AutoMapper;

using DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.CreateUser;
using DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.UpdateUser;
using DY.Auth.Identity.Api.ApplicationLogic.Services.User.Queries.GetUserById;
using DY.Auth.Identity.Api.Core.Entities;

namespace DY.Auth.Identity.Api.ApplicationLogic.Mapping;

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
        this.CreateMap<CreateUserCommand, AppUser>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

        this.CreateMap<UpdateUserCommand, AppUser>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp));

        this.CreateMap<AppUser, CreateUserResult>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp))
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRoles));

        this.CreateMap<AppUserRole, CreateUserResult.UserRoleResult>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.Name, opt =>
            {
                opt.PreCondition(src => src.Role != null);
                opt.MapFrom(src => src.Role.Name);
            });

        this.CreateMap<AppUser, GetUserByIdResult>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp))
            .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles));

        this.CreateMap<AppUserRole, GetUserByIdResult.UserRoleResult>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.Name, opt =>
            {
                opt.PreCondition(src => src.Role != null);
                opt.MapFrom(src => src.Role.Name);
            });

        this.CreateMap<AppUser, UpdateUserResult>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp))
            .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles));

        this.CreateMap<AppUserRole, UpdateUserResult.UserRoleResult>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.Name, opt =>
            {
                opt.PreCondition(src => src.Role != null);
                opt.MapFrom(src => src.Role.Name);
            });
    }
}
