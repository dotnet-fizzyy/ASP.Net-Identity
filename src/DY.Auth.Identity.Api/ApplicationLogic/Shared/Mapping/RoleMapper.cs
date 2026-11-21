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
    /// <summary>
    /// Maps <see cref="CreateRoleCommand"/> to <see cref="AppRole"/>.
    /// </summary>
    /// <param name="source">The create-role command to map.</param>
    /// <returns>The mapped role entity, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static AppRole ToAppRole(CreateRoleCommand source)
    {
        if (source is null)
        {
            return null;
        }

        return new AppRole
        {
            Name = source.Name,
        };
    }

    /// <summary>
    /// Maps <see cref="AppRole"/> to <see cref="CreateRoleResult"/>.
    /// </summary>
    /// <param name="source">The role entity to map.</param>
    /// <returns>The mapped result, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static CreateRoleResult ToCreateRoleResult(AppRole source)
    {
        if (source is null)
        {
            return null;
        }

        return new CreateRoleResult
        {
            RoleId = source.Id,
            Name = source.Name,
            ConcurrencyStamp = source.ConcurrencyStamp,
            CreationDate = source.CreationDate,
        };
    }

    /// <summary>
    /// Maps <see cref="UpdateRoleCommand"/> to <see cref="AppRole"/>.
    /// </summary>
    /// <param name="source">The update-role command to map.</param>
    /// <returns>The mapped role entity, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static AppRole ToAppRole(UpdateRoleCommand source)
    {
        if (source is null)
        {
            return null;
        }

        return new AppRole
        {
            Id = source.Id,
            Name = source.Name,
            ConcurrencyStamp = source.ConcurrencyStamp,
        };
    }

    /// <summary>
    /// Maps <see cref="AppRole"/> to <see cref="UpdateRoleResult"/>.
    /// </summary>
    /// <param name="source">The role entity to map.</param>
    /// <returns>The mapped result, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static UpdateRoleResult ToUpdateRoleResult(AppRole source)
    {
        if (source is null)
        {
            return null;
        }

        return new UpdateRoleResult
        {
            RoleId = source.Id,
            Name = source.Name,
            ConcurrencyStamp = source.ConcurrencyStamp,
            CreationDate = source.CreationDate,
        };
    }

    /// <summary>
    /// Maps <see cref="AppRole"/> to <see cref="GetRoleByIdResult"/>.
    /// </summary>
    /// <param name="source">The role entity to map.</param>
    /// <returns>The mapped result, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static GetRoleByIdResult ToGetRoleByIdResult(AppRole source)
    {
        if (source is null)
        {
            return null;
        }

        return new GetRoleByIdResult
        {
            RoleId = source.Id,
            Name = source.Name,
            ConcurrencyStamp = source.ConcurrencyStamp,
            CreationDate = source.CreationDate,
        };
    }
}
