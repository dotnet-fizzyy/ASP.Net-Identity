using DY.Auth.Identity.Api.ApplicationLogic.User.Commands.CreateUser;
using DY.Auth.Identity.Api.ApplicationLogic.User.Commands.UpdateUser;
using DY.Auth.Identity.Api.ApplicationLogic.User.Queries.GetUserById;
using DY.Auth.Identity.Api.Core.Entities;

using System.Linq;

namespace DY.Auth.Identity.Api.ApplicationLogic.Shared.Mapping;

/// <summary>
/// User feature mapper.
/// </summary>
internal static class UserMapper
{
    /// <summary>
    /// Maps <see cref="CreateUserCommand"/> to <see cref="AppUser"/>.
    /// </summary>
    /// <param name="source">The create-user command to map.</param>
    /// <returns>The mapped user entity, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static AppUser ToAppUser(CreateUserCommand source)
    {
        if (source is null)
        {
            return null;
        }

        return new AppUser
        {
            Id = source.Id,
            UserName = source.UserName,
            Email = source.Email,
            ConcurrencyStamp = source.ConcurrencyStamp,
            PhoneNumber = source.PhoneNumber,
        };
    }

    /// <summary>
    /// Maps <see cref="UpdateUserCommand"/> to <see cref="AppUser"/>.
    /// </summary>
    /// <param name="source">The update-user command to map.</param>
    /// <returns>The mapped user entity, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static AppUser ToAppUser(UpdateUserCommand source)
    {
        if (source is null)
        {
            return null;
        }

        return new AppUser
        {
            Id = source.Id,
            UserName = source.UserName,
            Email = source.Email,
            ConcurrencyStamp = source.ConcurrencyStamp,
            PhoneNumber = source.PhoneNumber,
        };
    }

    /// <summary>
    /// Maps <see cref="AppUser"/> to <see cref="CreateUserResult"/>.
    /// </summary>
    /// <param name="source">The user entity to map.</param>
    /// <returns>The mapped result, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static CreateUserResult ToCreateUserResult(AppUser source)
    {
        if (source is null)
        {
            return null;
        }

        return new CreateUserResult
        {
            UserId = source.Id,
            UserName = source.UserName,
            Email = source.Email,
            PhoneNumber = source.PhoneNumber,
            ConcurrencyStamp = source.ConcurrencyStamp,
            UserRole = source.UserRoles?.FirstOrDefault() is { } role
                ? ToCreateUserRoleResult(role)
                : null,
        };
    }

    /// <summary>
    /// Maps <see cref="AppUserRole"/> to <see cref="CreateUserResult.UserRoleResult"/>.
    /// </summary>
    /// <param name="source">The user role to map.</param>
    /// <returns>The mapped role result, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static CreateUserResult.UserRoleResult ToCreateUserRoleResult(AppUserRole source)
    {
        if (source is null)
        {
            return null;
        }

        return new CreateUserResult.UserRoleResult
        {
            RoleId = source.RoleId,
            Name = source.Role?.Name,
        };
    }

    /// <summary>
    /// Maps <see cref="AppUser"/> to <see cref="GetUserByIdResult"/>.
    /// </summary>
    /// <param name="source">The user entity to map.</param>
    /// <returns>The mapped result, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static GetUserByIdResult ToGetUserByIdResult(AppUser source)
    {
        if (source is null)
        {
            return null;
        }

        return new GetUserByIdResult
        {
            UserId = source.Id,
            UserName = source.UserName,
            Email = source.Email,
            PhoneNumber = source.PhoneNumber,
            ConcurrencyStamp = source.ConcurrencyStamp,
            UserRoles = source.UserRoles?
                .Select(ToGetUserRoleResult)
                .ToArray() ?? [],
        };
    }

    /// <summary>
    /// Maps <see cref="AppUserRole"/> to <see cref="GetUserByIdResult.UserRoleResult"/>.
    /// </summary>
    /// <param name="source">The user role to map.</param>
    /// <returns>The mapped role result, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static GetUserByIdResult.UserRoleResult ToGetUserRoleResult(AppUserRole source)
    {
        if (source is null)
        {
            return null;
        }

        return new GetUserByIdResult.UserRoleResult
        {
            RoleId = source.RoleId,
            Name = source.Role?.Name,
        };
    }

    /// <summary>
    /// Maps <see cref="AppUser"/> to <see cref="UpdateUserResult"/>.
    /// </summary>
    /// <param name="source">The user entity to map.</param>
    /// <returns>The mapped result, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static UpdateUserResult ToUpdateUserResult(AppUser source)
    {
        if (source is null)
        {
            return null;
        }

        return new UpdateUserResult
        {
            UserId = source.Id,
            UserName = source.UserName,
            Email = source.Email,
            PhoneNumber = source.PhoneNumber,
            ConcurrencyStamp = source.ConcurrencyStamp,
            UserRoles = source.UserRoles?
                .Select(ToUpdateUserRoleResult)
                .ToArray() ?? [],
        };
    }

    /// <summary>
    /// Maps <see cref="AppUserRole"/> to <see cref="UpdateUserResult.UserRoleResult"/>.
    /// </summary>
    /// <param name="source">The user role to map.</param>
    /// <returns>The mapped role result, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static UpdateUserResult.UserRoleResult ToUpdateUserRoleResult(AppUserRole source)
    {
        if (source is null)
        {
            return null;
        }

        return new UpdateUserResult.UserRoleResult
        {
            RoleId = source.RoleId,
            Name = source.Role?.Name,
        };
    }
}
