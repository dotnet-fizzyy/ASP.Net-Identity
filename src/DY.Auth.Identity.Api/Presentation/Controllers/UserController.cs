using AutoMapper;

using DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.CreateUser;
using DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.HardRemoveUserById;
using DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.SoftRemoveUserById;
using DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.UpdateUser;
using DY.Auth.Identity.Api.ApplicationLogic.Services.User.Queries.GetUserById;
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
[Authorize]
public class UserController : ControllerBase
{
    private readonly IHttpContextService httpContextService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </summary>
    /// <param name="mediator">The instance of <see cref="IMediator"/>.</param>
    /// <param name="httpContextService">The instance of <see cref="IHttpContextService"/>.</param>
    /// <param name="mapper">The instance of <see cref="IMapper"/>.</param>
    public UserController(
        IMediator mediator,
        IHttpContextService httpContextService,
        IMapper mapper)
            : base(mediator)
    {
        this.httpContextService = httpContextService ?? throw new ArgumentNullException(nameof(httpContextService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

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
            return this.CreateBadResponseByServiceResult(userIdResult);
        }

        var userId = userIdResult.Data;

        var query = new GetUserByIdQuery(userId);
        var userResult = await this.Mediator.Send(query, cancellationToken);

        if (userResult.IsResultFailed)
        {
            return this.CreateBadResponseByServiceResult(userResult);
        }

        return this.mapper.Map<UserDto>(userResult.Data);
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
            return this.CreateBadResponseByServiceResult(userResult);
        }

        return this.mapper.Map<UserDto>(userResult.Data);
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
        var command = this.mapper.Map<CreateUserCommand>(
            requestBody,
            opts => opts.Items[UserProfile.ConfirmUserEmailContextKey] = true);
        var userCreationResult = await this.Mediator.Send(command, cancellationToken);

        if (userCreationResult.IsResultFailed)
        {
            return this.CreateBadResponseByServiceResult(userCreationResult);
        }

        var userDto = this.mapper.Map<UserDto>(userCreationResult.Data);

        var getUserLink = this.httpContextService.GenerateGetUserLink(userDto.Id);

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
        var command = this.mapper.Map<UpdateUserCommand>(requestBody);
        var userUpdateResult = await this.Mediator.Send(command, cancellationToken);

        if (userUpdateResult.IsResultFailed)
        {
            return this.CreateBadResponseByServiceResult(userUpdateResult);
        }

        return this.mapper.Map<UserDto>(userUpdateResult.Data);
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
            return this.CreateBadResponseByServiceResult(userRemoveResult);
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
    [HttpDelete("id/{id:guid}/hard-remove")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> HardRemoveUser(Guid id, CancellationToken cancellationToken)
    {
        var command = new HardRemoveUserByIdCommand(id);
        var userRemoveResult = await this.Mediator.Send(command, cancellationToken);

        if (userRemoveResult.IsResultFailed)
        {
            return this.CreateBadResponseByServiceResult(userRemoveResult);
        }

        return this.NoContent();
    }
}
