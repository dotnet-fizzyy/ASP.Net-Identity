using DY.Auth.Identity.Api.ApplicationLogic.User.Commands.AuthenticateUser;
using DY.Auth.Identity.Api.ApplicationLogic.User.Commands.CreateUser;
using DY.Auth.Identity.Api.ApplicationLogic.User.Commands.UpdateUser;
using DY.Auth.Identity.Api.ApplicationLogic.User.Queries.GetUserById;
using DY.Auth.Identity.Api.Core.Constants;
using DY.Auth.Identity.Api.Presentation.Models.DTO.User;

using System;
using System.Linq;

namespace DY.Auth.Identity.Api.Presentation.Mapping;

/// <summary>
/// Maps user-related presentation models, application commands, and results.
/// </summary>
internal static class UserMapper
{
    /// <summary>
    /// Maps <see cref="UserSignInDto"/> to <see cref="AuthenticateUserCommand"/>.
    /// </summary>
    /// <param name="source">The sign-in DTO to map.</param>
    /// <returns>The mapped command, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static AuthenticateUserCommand ToAuthenticateUserCommand(UserSignInDto source)
    {
        if (source is null)
        {
            return null;
        }

        return new AuthenticateUserCommand
        {
            Email = source.Email,
            Password = source.Password,
        };
    }

    /// <summary>
    /// Maps <see cref="UserRegistrationDto"/> to <see cref="CreateUserCommand"/>.
    /// </summary>
    /// <param name="source">The registration DTO to map.</param>
    /// <returns>The mapped command, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static CreateUserCommand ToCreateUserCommand(UserRegistrationDto source)
    {
        if (source is null)
        {
            return null;
        }

        return new CreateUserCommand
        {
            UserName = source.UserName,
            Email = source.Email,
            Password = source.Password,
            UserRole = UserRoleConstants.User,
        };
    }

    /// <summary>
    /// Maps <see cref="CreateUserDto"/> to <see cref="CreateUserCommand"/>.
    /// </summary>
    /// <param name="source">The create-user DTO to map.</param>
    /// <param name="confirmEmailImmediately">Whether to confirm the user's email immediately.</param>
    /// <returns>The mapped command, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static CreateUserCommand ToCreateUserCommand(CreateUserDto source, bool confirmEmailImmediately)
    {
        if (source is null)
        {
            return null;
        }

        return new CreateUserCommand
        {
            UserName = source.UserName,
            Password = source.Password,
            Email = source.Email,
            PhoneNumber = source.PhoneNumber,
            UserRole = source.UserRole,
            ConfirmEmailImmediately = confirmEmailImmediately,
        };
    }

    /// <summary>
    /// Maps <see cref="CreateUserResult.UserRoleResult"/> to <see cref="UserRoleDto"/>.
    /// </summary>
    /// <param name="source">The create-user role result to map.</param>
    /// <returns>The mapped DTO, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static UserRoleDto ToUserRoleDto(CreateUserResult.UserRoleResult source) =>
        source == null
            ? null
            : new UserRoleDto
            {
                RoleId = source.RoleId,
                Name = source.Name,
            };

    /// <summary>
    /// Maps <see cref="CreateUserResult"/> to <see cref="UserDto"/>.
    /// </summary>
    /// <param name="source">The create-user result to map.</param>
    /// <returns>The mapped DTO, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static UserDto ToUserDto(CreateUserResult source)
    {
        if (source is null)
        {
            return null;
        }

        return new UserDto
        {
            Id = source.UserId,
            Email = source.Email,
            UserName = source.UserName,
            PhoneNumber = source.PhoneNumber,
            ConcurrencyStamp = source.ConcurrencyStamp,
            Password = source.Password,
            UserRoles = new[] { ToUserRoleDto(source.UserRole) },
        };
    }

    /// <summary>
    /// Maps <see cref="GetUserByIdResult"/> to <see cref="UserDto"/>.
    /// </summary>
    /// <param name="source">The user-by-id result to map.</param>
    /// <returns>The mapped DTO, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static UserDto ToUserDto(GetUserByIdResult source)
    {
        if (source is null)
        {
            return null;
        }

        return new UserDto
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
    }

    /// <summary>
    /// Maps <see cref="GetUserByIdResult.UserRoleResult"/> to <see cref="UserRoleDto"/>.
    /// </summary>
    /// <param name="source">The user-by-id role result to map.</param>
    /// <returns>The mapped DTO, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static UserRoleDto ToUserRoleDto(GetUserByIdResult.UserRoleResult source) =>
        source == null
            ? null
            : new UserRoleDto
            {
                RoleId = source.RoleId,
                Name = source.Name,
            };

    /// <summary>
    /// Maps <see cref="UpdateUserDto"/> to <see cref="UpdateUserCommand"/>.
    /// </summary>
    /// <param name="source">The update-user DTO to map.</param>
    /// <returns>The mapped command, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static UpdateUserCommand ToUpdateUserCommand(UpdateUserDto source)
    {
        if (source is null)
        {
            return null;
        }

        return new UpdateUserCommand
        {
            Id = source.Id,
            Email = source.Email,
            UserName = source.UserName,
            PhoneNumber = source.PhoneNumber,
            ConcurrencyStamp = source.ConcurrencyStamp,
        };
    }

    /// <summary>
    /// Maps <see cref="UserDto"/> to <see cref="UpdateUserCommand"/>.
    /// </summary>
    /// <param name="source">The user DTO to map.</param>
    /// <returns>The mapped command, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static UpdateUserCommand ToUpdateUserCommand(UserDto source)
    {
        if (source is null)
        {
            return null;
        }

        return new UpdateUserCommand
        {
            Id = source.Id,
            Email = source.Email,
            UserName = source.UserName,
            PhoneNumber = source.PhoneNumber,
            ConcurrencyStamp = source.ConcurrencyStamp,
        };
    }

    /// <summary>
    /// Maps <see cref="UpdateUserResult"/> to <see cref="UserDto"/>.
    /// </summary>
    /// <param name="source">The update-user result to map.</param>
    /// <returns>The mapped DTO, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static UserDto ToUserDto(UpdateUserResult source)
    {
        if (source is null)
        {
            return null;
        }

        return new UserDto
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
    }

    /// <summary>
    /// Maps <see cref="UpdateUserResult.UserRoleResult"/> to <see cref="UserRoleDto"/>.
    /// </summary>
    /// <param name="source">The update-user role result to map.</param>
    /// <returns>The mapped DTO, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static UserRoleDto ToUserRoleDto(UpdateUserResult.UserRoleResult source) =>
        source == null
            ? null
            : new UserRoleDto
            {
                RoleId = source.RoleId,
                Name = source.Name,
            };
}
