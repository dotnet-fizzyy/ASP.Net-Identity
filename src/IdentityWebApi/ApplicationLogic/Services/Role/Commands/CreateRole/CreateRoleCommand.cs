using IdentityWebApi.ApplicationLogic.Models.Output;
using IdentityWebApi.Core.Results;

using MediatR;

namespace IdentityWebApi.ApplicationLogic.Services.Role.Commands.CreateRole;

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
