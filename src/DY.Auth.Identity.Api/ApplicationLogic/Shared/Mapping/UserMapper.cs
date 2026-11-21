using DY.Auth.Identity.Api.ApplicationLogic.User.Commands.CreateUser;
using DY.Auth.Identity.Api.ApplicationLogic.User.Commands.UpdateUser;
using DY.Auth.Identity.Api.ApplicationLogic.User.Queries.GetUserById;
using DY.Auth.Identity.Api.Core.Entities;

using System;
using System.Linq;

namespace DY.Auth.Identity.Api.ApplicationLogic.Shared.Mapping;

/// <summary>
/// User feature mapper.
/// </summary>
internal static class UserMapper
{
    internal static AppUser ToAppUser(CreateUserCommand source) =>
        new ()
        {
            Id = source.Id,
            UserName = source.UserName,
            Email = source.Email,
            ConcurrencyStamp = source.ConcurrencyStamp,
            PhoneNumber = source.PhoneNumber,
        };

    internal static AppUser ToAppUser(UpdateUserCommand source) =>
        new ()
        {
            Id = source.Id,
            UserName = source.UserName,
            Email = source.Email,
            ConcurrencyStamp = source.ConcurrencyStamp,
            PhoneNumber = source.PhoneNumber,
        };

    internal static CreateUserResult ToCreateUserResult(AppUser source) =>
        new ()
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

    internal static CreateUserResult.UserRoleResult ToCreateUserRoleResult(AppUserRole source) =>
        new ()
        {
            RoleId = source.RoleId,
            Name = source.Role?.Name,
        };

    internal static GetUserByIdResult ToGetUserByIdResult(AppUser source) =>
        new ()
        {
            UserId = source.Id,
            UserName = source.UserName,
            Email = source.Email,
            PhoneNumber = source.PhoneNumber,
            ConcurrencyStamp = source.ConcurrencyStamp,
            UserRoles = source.UserRoles?
                .Select(ToGetUserRoleResult)
                .ToArray() ?? Array.Empty<GetUserByIdResult.UserRoleResult>(),
        };

    internal static GetUserByIdResult.UserRoleResult ToGetUserRoleResult(AppUserRole source) =>
        new ()
        {
            RoleId = source.RoleId,
            Name = source.Role?.Name,
        };

    internal static UpdateUserResult ToUpdateUserResult(AppUser source) =>
        new ()
        {
            UserId = source.Id,
            UserName = source.UserName,
            Email = source.Email,
            PhoneNumber = source.PhoneNumber,
            ConcurrencyStamp = source.ConcurrencyStamp,
            UserRoles = source.UserRoles?
                .Select(ToUpdateUserRoleResult)
                .ToArray() ?? Array.Empty<UpdateUserResult.UserRoleResult>(),
        };

    internal static UpdateUserResult.UserRoleResult ToUpdateUserRoleResult(AppUserRole source) =>
        new ()
        {
            RoleId = source.RoleId,
            Name = source.Role?.Name,
        };
}
