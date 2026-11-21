using DY.Auth.Identity.Api.ApplicationLogic.Role.Commands.CreateRole;
using DY.Auth.Identity.Api.ApplicationLogic.Role.Commands.GrantRoleToUser;
using DY.Auth.Identity.Api.ApplicationLogic.Role.Commands.RevokeRoleFromUser;
using DY.Auth.Identity.Api.ApplicationLogic.Role.Commands.UpdateRole;
using DY.Auth.Identity.Api.ApplicationLogic.Role.Queries.GetRoleById;
using DY.Auth.Identity.Api.Presentation.Models.DTO.Role;

namespace DY.Auth.Identity.Api.Presentation.Mapping;

internal static class RoleMapper
{
    internal static CreateRoleCommand ToCreateRoleCommand(CreateRoleDto source) =>
        new (source.Name);

    internal static RoleDto ToRoleDto(CreateRoleResult source) =>
        new ()
        {
            Id = source.RoleId,
            Name = source.Name,
            ConcurrencyStamp = source.ConcurrencyStamp,
            CreationDate = source.CreationDate,
        };

    internal static UpdateRoleCommand ToUpdateRoleCommand(UpdateRoleDto source) =>
        new (source.Name, source.ConcurrencyStamp)
        {
            Id = source.RoleId,
        };

    internal static RoleDto ToRoleDto(UpdateRoleResult source) =>
        new ()
        {
            Id = source.RoleId,
            Name = source.Name,
            ConcurrencyStamp = source.ConcurrencyStamp,
            CreationDate = source.CreationDate,
        };

    internal static RoleDto ToRoleDto(GetRoleByIdResult source) =>
        new ()
        {
            Id = source.RoleId,
            Name = source.Name,
            ConcurrencyStamp = source.ConcurrencyStamp,
            CreationDate = source.CreationDate,
        };

    internal static GrantRoleToUserCommand ToGrantRoleToUserCommand(UserRoleDto source) =>
        new ()
        {
            RoleId = source.RoleId,
            UserId = source.UserId,
        };

    internal static RevokeRoleFromUserCommand ToRevokeRoleFromUserCommand(UserRoleDto source) =>
        new ()
        {
            RoleId = source.RoleId,
            UserId = source.UserId,
        };
}
