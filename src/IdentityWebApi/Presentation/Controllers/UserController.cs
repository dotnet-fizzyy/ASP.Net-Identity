using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.ApplicationLogic.Services.User.Commands.CreateUser;
using IdentityWebApi.ApplicationLogic.Services.User.Commands.HardRemoveUserById;
using IdentityWebApi.ApplicationLogic.Services.User.Commands.SoftRemoveUserById;
using IdentityWebApi.ApplicationLogic.Services.User.Commands.UpdateUser;
using IdentityWebApi.ApplicationLogic.Services.User.Queries.GetUserById;
using IdentityWebApi.Core.Constants;
using IdentityWebApi.Presentation.Services;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.Presentation.Controllers;

/// <summary>
/// User controller.
/// </summary>
public class UserController : ControllerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </summary>
    /// <param name="mediator"><see cref="IMediator"/>.</param>
    public UserController(IMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Returns information about user by User HTTP context.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserResultDto>> GetUserByToken()
    {
        var userIdResult = ClaimsService.GetUserIdFromIdentityUser(this.User);

        if (userIdResult.IsResultFailed)
        {
            return this.CreateFailedResponseByServiceResult(userIdResult);
        }

        return await this.GetUser(userIdResult.Data);
    }

    /// <summary>
    /// Returns information about user.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [Authorize]
    [HttpGet("id/{id:guid}")]
    public async Task<ActionResult<UserResultDto>> GetUser(Guid id)
    {
        var query = new GetUserByIdQuery(id);
        var userResult = await this.Mediator.Send(query);

        if (userResult.IsResultFailed)
        {
            return this.CreateFailedResponseByServiceResult(userResult);
        }

        return userResult.Data;
    }

    /// <summary>
    /// Creates user entity.
    /// </summary>
    /// <param name="user"><see cref="UserDto"/>.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<UserResultDto>> CreateUser([FromBody, BindRequired] UserDto user)
    {
        // todo: extract from body
        var command = new CreateUserCommand(user, false);
        var userCreationResult = await this.Mediator.Send(command);

        if (userCreationResult.IsResultFailed)
        {
            return this.CreateFailedResponseByServiceResult(userCreationResult);
        }

        return this.CreatedAtAction(nameof(this.CreateUser), userCreationResult.Data);
    }

    /// <summary>
    /// Updates user entity.
    /// </summary>
    /// <param name="user"><see cref="UserDto"/>.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [Authorize(Roles = UserRoleConstants.Admin)]
    [HttpPut]
    public async Task<ActionResult<UserResultDto>> UpdateUser([FromBody, BindRequired] UserDto user)
    {
        var command = new UpdateUserCommand(user);
        var userUpdateResult = await this.Mediator.Send(command);

        if (userUpdateResult.IsResultFailed)
        {
            return this.CreateFailedResponseByServiceResult(userUpdateResult);
        }

        return userUpdateResult.Data;
    }

    /// <summary>
    /// Updates user entity with "IsDeleted=true".
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [Authorize(Roles = UserRoleConstants.Admin)]
    [HttpDelete("id/{id:guid}/soft-remove")]
    public async Task<ActionResult> SoftRemoveUser(Guid id)
    {
        var command = new SoftRemoveUserByIdCommand(id);
        var userRemoveResult = await this.Mediator.Send(command);

        if (userRemoveResult.IsResultFailed)
        {
            return this.CreateFailedResponseByServiceResult(userRemoveResult);
        }

        return this.NoContent();
    }

    /// <summary>
    /// Removes user entity from DB.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [Authorize(Roles = UserRoleConstants.Admin)]
    [HttpDelete("id/{id:guid}/hard-remove")]
    public async Task<ActionResult> HardRemoveUser(Guid id)
    {
        var command = new HardRemoveUserByIdCommand(id);
        var userRemoveResult = await this.Mediator.Send(command);

        if (userRemoveResult.IsResultFailed)
        {
            return this.CreateFailedResponseByServiceResult(userRemoveResult);
        }

        return this.NoContent();
    }
}