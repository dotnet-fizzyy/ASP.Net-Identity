using IdentityWebApi.ApplicationLogic.Models.Output;
using IdentityWebApi.Core.Results;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services.Role.Commands.UpdateRole;

/// <summary>
/// Update role CQRS handler.
/// </summary>
public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, ServiceResult<RoleResult>>
{
    public async Task<ServiceResult<RoleResult>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}
