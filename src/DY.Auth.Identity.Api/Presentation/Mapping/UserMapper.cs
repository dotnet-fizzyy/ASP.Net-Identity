using DY.Auth.Identity.Api.ApplicationLogic.User.Commands.AuthenticateUser;
using DY.Auth.Identity.Api.ApplicationLogic.User.Commands.CreateUser;
using DY.Auth.Identity.Api.ApplicationLogic.User.Commands.UpdateUser;
using DY.Auth.Identity.Api.ApplicationLogic.User.Queries.GetUserById;
using DY.Auth.Identity.Api.Core.Constants;
using DY.Auth.Identity.Api.Presentation.Models.DTO.User;

using System;
using System.Linq;

namespace DY.Auth.Identity.Api.Presentation.Mapping;

internal static class UserMapper
{
    internal static AuthenticateUserCommand ToAuthenticateUserCommand(UserSignInDto source) =>
        new ()
        {
            Email = source.Email,
            Password = source.Password,
        };

    internal static CreateUserCommand ToCreateUserCommand(UserRegistrationDto source) =>
        new ()
        {
            UserName = source.UserName,
            Email = source.Email,
            Password = source.Password,
            UserRole = UserRoleConstants.User,
        };

    internal static CreateUserCommand ToCreateUserCommand(CreateUserDto source, bool confirmEmailImmediately) =>
        new ()
        {
            UserName = source.UserName,
            Password = source.Password,
            Email = source.Email,
            PhoneNumber = source.PhoneNumber,
            UserRole = source.UserRole,
            ConfirmEmailImmediately = confirmEmailImmediately,
        };

    internal static UserDto ToUserDto(CreateUserResult source) =>
        new ()
        {
            Id = source.UserId,
            Email = source.Email,
            UserName = source.UserName,
            PhoneNumber = source.PhoneNumber,
            ConcurrencyStamp = source.ConcurrencyStamp,
            Password = source.Password,
            UserRoles = new[] { ToUserRoleDto(source.UserRole) },
        };

    internal static UserRoleDto ToUserRoleDto(CreateUserResult.UserRoleResult source) =>
        source == null
            ? null
            : new UserRoleDto
            {
                RoleId = source.RoleId,
                Name = source.Name,
            };

    internal static UserDto ToUserDto(GetUserByIdResult source) =>
        new ()
        {
            Id = source.UserId,
            Email = source.Email,
            UserName = source.UserName,
            PhoneNumber = source.PhoneNumber,
            ConcurrencyStamp = source.ConcurrencyStamp,
            UserRoles = source.UserRoles?
                .Select(ToUserRoleDto)
                .ToArray() ?? Array.Empty<UserRoleDto>(),
        };

    internal static UserRoleDto ToUserRoleDto(GetUserByIdResult.UserRoleResult source) =>
        source == null
            ? null
            : new UserRoleDto
            {
                RoleId = source.RoleId,
                Name = source.Name,
            };

    internal static UpdateUserCommand ToUpdateUserCommand(UpdateUserDto source) =>
        new ()
        {
            Id = source.Id,
            Email = source.Email,
            UserName = source.UserName,
            PhoneNumber = source.PhoneNumber,
            ConcurrencyStamp = source.ConcurrencyStamp,
        };

    internal static UpdateUserCommand ToUpdateUserCommand(UserDto source) =>
        new ()
        {
            Id = source.Id,
            Email = source.Email,
            UserName = source.UserName,
            PhoneNumber = source.PhoneNumber,
            ConcurrencyStamp = source.ConcurrencyStamp,
        };

    internal static UserDto ToUserDto(UpdateUserResult source) =>
        new ()
        {
            Id = source.UserId,
            UserName = source.UserName,
            Email = source.Email,
            PhoneNumber = source.PhoneNumber,
            ConcurrencyStamp = source.ConcurrencyStamp,
            Password = source.Password,
            UserRoles = source.UserRoles?
                .Select(ToUserRoleDto)
                .ToArray() ?? Array.Empty<UserRoleDto>(),
        };

    internal static UserRoleDto ToUserRoleDto(UpdateUserResult.UserRoleResult source) =>
        source == null
            ? null
            : new UserRoleDto
            {
                RoleId = source.RoleId,
                Name = source.Name,
            };
}
