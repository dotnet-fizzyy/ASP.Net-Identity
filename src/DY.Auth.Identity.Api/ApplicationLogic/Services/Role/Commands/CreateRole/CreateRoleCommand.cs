using DY.Auth.Identity.Api.ApplicationLogic.Models.Output;
using DY.Auth.Identity.Api.Core.Results;

using MediatR;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.CreateRole;

/// <summary>
/// Create role CQRS command.
/// </summary>
public record CreateRoleCommand : IRequest<ServiceResult<RoleResult>>
{
    /// <summary>
    /// Gets role name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateRoleCommand"/> class.
    /// </summary>
    /// <param name="name">Role name.</param>
    public CreateRoleCommand(string name)
    {
        this.Name = name;
    }
}
