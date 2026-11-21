using DY.Auth.Identity.Api.ApplicationLogic.Role.Commands.CreateRole;
using DY.Auth.Identity.Api.ApplicationLogic.Role.Commands.GrantRoleToUser;
using DY.Auth.Identity.Api.ApplicationLogic.Role.Commands.RevokeRoleFromUser;
using DY.Auth.Identity.Api.ApplicationLogic.Role.Commands.UpdateRole;
using DY.Auth.Identity.Api.ApplicationLogic.Role.Queries.GetRoleById;
using DY.Auth.Identity.Api.Presentation.Models.DTO.Role;

namespace DY.Auth.Identity.Api.Presentation.Mapping;

/// <summary>
/// Maps role-related presentation models, application commands, and results.
/// </summary>
internal static class RoleMapper
{
    /// <summary>
    /// Maps <see cref="CreateRoleDto"/> to <see cref="CreateRoleCommand"/>.
    /// </summary>
    /// <param name="source">The create-role DTO to map.</param>
    /// <returns>The mapped command, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static CreateRoleCommand ToCreateRoleCommand(CreateRoleDto source)
    {
        if (source is null)
        {
            return null;
        }

        return new CreateRoleCommand(source.Name);
    }

    /// <summary>
    /// Maps <see cref="CreateRoleResult"/> to <see cref="RoleDto"/>.
    /// </summary>
    /// <param name="source">The create-role result to map.</param>
    /// <returns>The mapped DTO, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static RoleDto ToRoleDto(CreateRoleResult source)
    {
        if (source is null)
        {
            return null;
        }

        return new RoleDto
        {
            Id = source.RoleId,
            Name = source.Name,
            ConcurrencyStamp = source.ConcurrencyStamp,
            CreationDate = source.CreationDate,
        };
    }

    /// <summary>
    /// Maps <see cref="UpdateRoleDto"/> to <see cref="UpdateRoleCommand"/>.
    /// </summary>
    /// <param name="source">The update-role DTO to map.</param>
    /// <returns>The mapped command, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static UpdateRoleCommand ToUpdateRoleCommand(UpdateRoleDto source)
    {
        if (source is null)
        {
            return null;
        }

        return new UpdateRoleCommand(source.Name, source.ConcurrencyStamp)
        {
            Id = source.RoleId,
        };
    }

    /// <summary>
    /// Maps <see cref="UpdateRoleResult"/> to <see cref="RoleDto"/>.
    /// </summary>
    /// <param name="source">The update-role result to map.</param>
    /// <returns>The mapped DTO, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static RoleDto ToRoleDto(UpdateRoleResult source)
    {
        if (source is null)
        {
            return null;
        }

        return new RoleDto
        {
            Id = source.RoleId,
            Name = source.Name,
            ConcurrencyStamp = source.ConcurrencyStamp,
            CreationDate = source.CreationDate,
        };
    }

    /// <summary>
    /// Maps <see cref="GetRoleByIdResult"/> to <see cref="RoleDto"/>.
    /// </summary>
    /// <param name="source">The role-by-id result to map.</param>
    /// <returns>The mapped DTO, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static RoleDto ToRoleDto(GetRoleByIdResult source)
    {
        if (source is null)
        {
            return null;
        }

        return new RoleDto
        {
            Id = source.RoleId,
            Name = source.Name,
            ConcurrencyStamp = source.ConcurrencyStamp,
            CreationDate = source.CreationDate,
        };
    }

    /// <summary>
    /// Maps <see cref="UserRoleDto"/> to <see cref="GrantRoleToUserCommand"/>.
    /// </summary>
    /// <param name="source">The user-role DTO to map.</param>
    /// <returns>The mapped command, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static GrantRoleToUserCommand ToGrantRoleToUserCommand(UserRoleDto source)
    {
        if (source is null)
        {
            return null;
        }

        return new GrantRoleToUserCommand
        {
            RoleId = source.RoleId,
            UserId = source.UserId,
        };
    }

    /// <summary>
    /// Maps <see cref="UserRoleDto"/> to <see cref="RevokeRoleFromUserCommand"/>.
    /// </summary>
    /// <param name="source">The user-role DTO to map.</param>
    /// <returns>The mapped command, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static RevokeRoleFromUserCommand ToRevokeRoleFromUserCommand(UserRoleDto source)
    {
        if (source is null)
        {
            return null;
        }

        return new RevokeRoleFromUserCommand
        {
            RoleId = source.RoleId,
            UserId = source.UserId,
        };
    }
}
