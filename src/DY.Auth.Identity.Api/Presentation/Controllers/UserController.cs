using DY.Auth.Identity.Api.ApplicationLogic.User.Commands.RemoveUserById;
using DY.Auth.Identity.Api.ApplicationLogic.User.Commands.SoftRemoveUserById;
using DY.Auth.Identity.Api.ApplicationLogic.User.Queries.GetUserById;
using DY.Auth.Identity.Api.Core.Constants;
using DY.Auth.Identity.Api.Core.Interfaces.Presentation;
using DY.Auth.Identity.Api.Presentation.Mapping;
using DY.Auth.Identity.Api.Presentation.Models.DTO.User;
using DY.Auth.Identity.Api.Presentation.Services;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.Presentation.Controllers;

/// <summary>
/// User controller.
/// </summary>
/// <param name="mediator">The instance of <see cref="IMediator"/>.</param>
/// <param name="controllersLinkService">The instance of <see cref="IControllersLinkService"/>.</param>
[Authorize]
public class UserController(
    IMediator mediator,
    IControllersLinkService controllersLinkService) : ControllerBase(mediator)
{
    private readonly IControllersLinkService controllersLinkService = controllersLinkService ?? throw new ArgumentNullException(nameof(controllersLinkService));

    /// <summary>
    /// Returns information about user by User HTTP context.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">User has been found.</response>
    /// <response code="404">Unable to find user.</response>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetUserByIdentity(CancellationToken cancellationToken)
    {
        var userIdResult = ClaimsService.GetUserIdFromIdentityUser(this.User);

        if (userIdResult.IsResultFailed)
        {
            return this.CreateErrorResponse(userIdResult);
        }

        var userId = userIdResult.Data;

        var query = new GetUserByIdQuery(userId);
        var userResult = await this.Mediator.Send(query, cancellationToken);

        if (userResult.IsResultFailed)
        {
            return this.CreateErrorResponse(userResult);
        }

        return UserMapper.ToUserDto(userResult.Data);
    }

    /// <summary>
    /// Returns information about user by id.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">User has been found.</response>
    /// <response code="404">Unable to find user.</response>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpGet("id/{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var userResult = await this.Mediator.Send(query, cancellationToken);

        if (userResult.IsResultFailed)
        {
            return this.CreateErrorResponse(userResult);
        }

        return UserMapper.ToUserDto(userResult.Data);
    }

    /// <summary>
    /// Creates user entity.
    /// </summary>
    /// <param name="requestBody"><see cref="CreateUserDto"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="201">User has been created.</response>
    /// <response code="404">Role to assign to user is not found.</response>
    /// <response code="500">Unable to create user due to internal issues.</response>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserDto>> CreateUser(
        [FromBody, BindRequired] CreateUserDto requestBody,
        CancellationToken cancellationToken)
    {
        var command = UserMapper.ToCreateUserCommand(requestBody, confirmEmailImmediately: true);
        var userCreationResult = await this.Mediator.Send(command, cancellationToken);

        if (userCreationResult.IsResultFailed)
        {
            return this.CreateErrorResponse(userCreationResult);
        }

        var userDto = UserMapper.ToUserDto(userCreationResult.Data);

        var getUserLink = this.controllersLinkService.GenerateGetUserLink(userDto.Id);

        return this.Created(getUserLink, userDto);
    }

    /// <summary>
    /// Updates user entity.
    /// </summary>
    /// <param name="requestBody"><see cref="UserDto"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">User details have been updated.</response>
    /// <response code="404">Unable to find user.</response>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [Authorize(Roles = UserRoleConstants.Admin)]
    [HttpPut]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> UpdateUser(
        [FromBody, BindRequired] UserDto requestBody,
        CancellationToken cancellationToken)
    {
        var command = UserMapper.ToUpdateUserCommand(requestBody);
        var userUpdateResult = await this.Mediator.Send(command, cancellationToken);

        if (userUpdateResult.IsResultFailed)
        {
            return this.CreateErrorResponse(userUpdateResult);
        }

        return UserMapper.ToUserDto(userUpdateResult.Data);
    }

    /// <summary>
    /// Updates user entity with "IsDeleted=true".
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="204">User status "IsDeleted" has been set to true.</response>
    /// <response code="404">Unable to find user.</response>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [Authorize(Roles = UserRoleConstants.Admin)]
    [HttpDelete("id/{id:guid}/soft-remove")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> SoftRemoveUser(Guid id, CancellationToken cancellationToken)
    {
        var command = new SoftRemoveUserByIdCommand(id);
        var userRemoveResult = await this.Mediator.Send(command, cancellationToken);

        if (userRemoveResult.IsResultFailed)
        {
            return this.CreateErrorResponse(userRemoveResult);
        }

        return this.NoContent();
    }

    /// <summary>
    /// Removes user entity from DB.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="204">User has been removed from DB.</response>
    /// <response code="404">Unable to find user.</response>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [Authorize(Roles = UserRoleConstants.Admin)]
    [HttpDelete("id/{id:guid}/remove")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RemoveUser(Guid id, CancellationToken cancellationToken)
    {
        var command = new RemoveUserByIdCommand(id);
        var userRemoveResult = await this.Mediator.Send(command, cancellationToken);

        if (userRemoveResult.IsResultFailed)
        {
            return this.CreateErrorResponse(userRemoveResult);
        }

        return this.NoContent();
    }
}
