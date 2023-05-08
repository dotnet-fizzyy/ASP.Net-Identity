using IdentityWebApi.Core.Results;

using MediatR;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.RegisterUser;

/// <summary>
/// Register user CQRS command
/// </summary>
public record RegisterUserCommand : IRequest<ServiceResult<Unit>>
{
}
