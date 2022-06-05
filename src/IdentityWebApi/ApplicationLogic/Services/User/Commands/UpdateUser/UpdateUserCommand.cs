using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Results;

using MediatR;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.UpdateUser;

public record UpdateUserCommand : IRequest<ServiceResult<UserResultDto>>;