using DY.Auth.Identity.Api.ApplicationLogic.Role.Commands.CreateRole;
using DY.Auth.Identity.Api.ApplicationLogic.Role.Commands.UpdateRole;
using DY.Auth.Identity.Api.ApplicationLogic.Role.Queries.GetRoleById;
using DY.Auth.Identity.Api.Core.Entities;

namespace DY.Auth.Identity.Api.ApplicationLogic.Shared.Mapping;

/// <summary>
/// Role feature mapper.
/// </summary>
internal static class RoleMapper
{
    internal static AppRole ToAppRole(CreateRoleCommand source) =>
        new ()
        {
            Name = source.Name,
        };

    internal static CreateRoleResult ToCreateRoleResult(AppRole source) =>
        new ()
        {
            RoleId = source.Id,
            Name = source.Name,
            ConcurrencyStamp = source.ConcurrencyStamp,
            CreationDate = source.CreationDate,
        };

    internal static AppRole ToAppRole(UpdateRoleCommand source) =>
        new ()
        {
            Id = source.Id,
            Name = source.Name,
            ConcurrencyStamp = source.ConcurrencyStamp,
        };

    internal static UpdateRoleResult ToUpdateRoleResult(AppRole source) =>
        new ()
        {
            RoleId = source.Id,
            Name = source.Name,
            ConcurrencyStamp = source.ConcurrencyStamp,
            CreationDate = source.CreationDate,
        };

    internal static GetRoleByIdResult ToGetRoleByIdResult(AppRole source) =>
        new ()
        {
            RoleId = source.Id,
            Name = source.Name,
            ConcurrencyStamp = source.ConcurrencyStamp,
            CreationDate = source.CreationDate,
        };
}
