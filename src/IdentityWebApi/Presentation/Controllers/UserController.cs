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
using Microsoft.AspNetCore.Http;
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
    /// <response code="200">User has been found.</response>
    /// <response code="404">Unable to find user.</response>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.
    /// </returns>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(UserResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResultDto>> GetUserByIdentity()
    {
        var userIdResult = ClaimsService.GetUserIdFromIdentityUser(this.User);

        if (userIdResult.IsResultFailed)
        {
            return this.CreateResponseByServiceResult(userIdResult);
        }

        var userId = userIdResult.Data;

        var query = new GetUserByIdQuery(userId);
        var userResult = await this.Mediator.Send(query);

        if (userResult.IsResultFailed)
        {
            return this.CreateResponseByServiceResult(userResult);
        }

        return userResult.Data;
    }

    /// <summary>
    /// Returns information about user by id.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <response code="200">User has been found.</response>
    /// <response code="404">Unable to find user.</response>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.
    /// </returns>
    [Authorize]
    [HttpGet("id/{id:guid}")]
    [ProducesResponseType(typeof(UserResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResultDto>> GetUser(Guid id)
    {
        var query = new GetUserByIdQuery(id);
        var userResult = await this.Mediator.Send(query);

        if (userResult.IsResultFailed)
        {
            return this.CreateResponseByServiceResult(userResult);
        }

        return userResult.Data;
    }

    /// <summary>
    /// Creates user entity.
    /// </summary>
    /// <param name="user"><see cref="UserDto"/>.</param>
    /// <response code="201">User has been created.</response>
    /// <response code="404">Role to assign to user is not found.</response>
    /// <response code="500">Unable to create user due to internal issues.</response>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.
    /// </returns>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(UserResultDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserResultDto>> CreateUser([FromBody, BindRequired] UserDto user)
    {
        // todo: extract from body
        var command = new CreateUserCommand(user, false);
        var userCreationResult = await this.Mediator.Send(command);

        if (userCreationResult.IsResultFailed)
        {
            return this.CreateResponseByServiceResult(userCreationResult);
        }

        return this.CreatedAtAction(nameof(this.CreateUser), userCreationResult.Data);
    }

    /// <summary>
    /// Updates user entity.
    /// </summary>
    /// <param name="user"><see cref="UserDto"/>.</param>
    /// <response code="200">User details have been updated.</response>
    /// <response code="404">Unable to find user.</response>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.
    /// </returns>
    [Authorize(Roles = UserRoleConstants.Admin)]
    [HttpPut]
    [ProducesResponseType(typeof(UserResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResultDto>> UpdateUser([FromBody, BindRequired] UserDto user)
    {
        var command = new UpdateUserCommand(user);
        var userUpdateResult = await this.Mediator.Send(command);

        if (userUpdateResult.IsResultFailed)
        {
            return this.CreateResponseByServiceResult(userUpdateResult);
        }

        return userUpdateResult.Data;
    }

    /// <summary>
    /// Updates user entity with "IsDeleted=true".
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <response code="204">User status "IsDeleted" has been set to true.</response>
    /// <response code="404">Unable to find user.</response>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.
    /// </returns>
    [Authorize(Roles = UserRoleConstants.Admin)]
    [HttpDelete("id/{id:guid}/soft-remove")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> SoftRemoveUser(Guid id)
    {
        var command = new SoftRemoveUserByIdCommand(id);
        var userRemoveResult = await this.Mediator.Send(command);

        if (userRemoveResult.IsResultFailed)
        {
            return this.CreateResponseByServiceResult(userRemoveResult);
        }

        return this.NoContent();
    }

    /// <summary>
    /// Removes user entity from DB.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <response code="204">User has been removed from DB.</response>
    /// <response code="404">Unable to find user.</response>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.
    /// </returns>
    [Authorize(Roles = UserRoleConstants.Admin)]
    [HttpDelete("id/{id:guid}/hard-remove")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> HardRemoveUser(Guid id)
    {
        var command = new HardRemoveUserByIdCommand(id);
        var userRemoveResult = await this.Mediator.Send(command);

        if (userRemoveResult.IsResultFailed)
        {
            return this.CreateResponseByServiceResult(userRemoveResult);
        }

        return this.NoContent();
    }
}