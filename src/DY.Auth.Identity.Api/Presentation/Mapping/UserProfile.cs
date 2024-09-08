using AutoMapper;

using DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.AuthenticateUser;
using DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.CreateUser;
using DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.UpdateUser;
using DY.Auth.Identity.Api.ApplicationLogic.Services.User.Queries.GetUserById;
using DY.Auth.Identity.Api.Core.Constants;
using DY.Auth.Identity.Api.Presentation.Models.DTO.User;

using System;

namespace DY.Auth.Identity.Api.Presentation.Mapping;

/// <summary>
/// User profile mapping.
/// </summary>
public class UserProfile : Profile
{
    /// <summary>
    /// Gets mapper context key for confirmation user email value.
    /// </summary>
    internal const string ConfirmUserEmailContextKey = "ConfirmEmailImmediately";

    /// <summary>
    /// Initializes a new instance of the <see cref="UserProfile"/> class.
    /// </summary>
    public UserProfile()
    {
        this.CreateMap<UserSignInDto, AuthenticateUserCommand>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

        this.CreateMap<UserRegistrationDto, CreateUserCommand>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(_ => UserRoleConstants.User));

        this.CreateMap<CreateUserDto, CreateUserCommand>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole))
            .ForMember(dest => dest.ConfirmEmailImmediately, opt => opt.MapFrom((_, _, _, context) => Convert.ToBoolean(context.Items[ConfirmUserEmailContextKey])));

        this.CreateMap<CreateUserResult, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp))
            .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => new[] { src.UserRole }));

        this.CreateMap<CreateUserResult.UserRoleResult, UserRoleDto>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        this.CreateMap<GetUserByIdResult, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp))
            .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles));

        this.CreateMap<GetUserByIdResult.UserRoleResult, UserRoleDto>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        this.CreateMap<UpdateUserDto, UpdateUserCommand>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp));

        this.CreateMap<UpdateUserResult, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp))
            .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles));

        this.CreateMap<UpdateUserResult.UserRoleResult, UserRoleDto>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }
}
