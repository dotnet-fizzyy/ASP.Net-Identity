using AutoMapper;

using DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.CreateRole;
using DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.GrantRoleToUser;
using DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.HardRemoveRoleById;
using DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.RevokeRoleFromUser;
using DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.SoftRemoveRoleById;
using DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.UpdateRole;
using DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Queries.GetRoleById;
using DY.Auth.Identity.Api.Core.Constants;
using DY.Auth.Identity.Api.Core.Interfaces.Presentation;
using DY.Auth.Identity.Api.Presentation.Models.DTO.Role;

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
/// Role controller.
/// </summary>
[Authorize(Roles = UserRoleConstants.Admin)]
public class RoleController : ControllerBase
{
    private readonly IHttpContextService httpContextService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleController"/> class.
    /// </summary>
    /// <param name="mediator">The instance of <see cref="IMediator"/>.</param>
    /// <param name="httpContextService">The instance of <see cref="IHttpContextService"/>.</param>
    /// <param name="mapper">The instance of <see cref="IMapper"/>.</param>
    public RoleController(
        IMediator mediator,
        IHttpContextService httpContextService,
        IMapper mapper)
            : base(mediator)
    {
        this.httpContextService = httpContextService ?? throw new ArgumentNullException(nameof(httpContextService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Returns role.
    /// </summary>
    /// <param name="id">Role identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">Role has been found.</response>
    /// <response code="404">Unable to find role.</response>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation with <see cref="RoleDto"/> Role.</returns>
    [HttpGet("id/{id:guid}")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoleDto>> GetRoleById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetRoleByIdQuery(id);
        var roleResult = await this.Mediator.Send(query, cancellationToken);

        if (roleResult.IsResultFailed)
        {
            return this.CreateBadResponseByServiceResult(roleResult);
        }

        return this.mapper.Map<RoleDto>(roleResult.Data);
    }

    /// <summary>
    /// Grants role to user.
    /// </summary>
    /// <param name="requestBody"><see cref="UserRoleDto"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="204">Role has been granted.</response>
    /// <response code="404">Unable to find role.</response>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpPost("grant")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GrantRoleToUser(
        [FromBody, BindRequired] UserRoleDto requestBody,
        CancellationToken cancellationToken)
    {
        var command = this.mapper.Map<GrantRoleToUserCommand>(requestBody);
        var roleGrantResult = await this.Mediator.Send(command, cancellationToken);

        if (roleGrantResult.IsResultFailed)
        {
            return this.CreateBadResponseByServiceResult(roleGrantResult);
        }

        return this.NoContent();
    }

    /// <summary>
    /// Revokes role from user.
    /// </summary>
    /// <param name="requestBody"><see cref="UserRoleDto"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="204">Role has been revoked.</response>
    /// <response code="404">Unable to find role.</response>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpPost("revoke")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RevokeRoleFromUser(
        [FromBody, BindRequired] UserRoleDto requestBody,
        CancellationToken cancellationToken)
    {
        var command = this.mapper.Map<RevokeRoleFromUserCommand>(requestBody);
        var roleRevokeResult = await this.Mediator.Send(command, cancellationToken);

        if (roleRevokeResult.IsResultFailed)
        {
            return this.CreateBadResponseByServiceResult(roleRevokeResult);
        }

        return this.NoContent();
    }

    /// <summary>
    /// Creates role entity.
    /// </summary>
    /// <param name="roleDto"><see cref="CreateRoleDto"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="201">Role has been created.</response>
    /// <response code="400">Role already exists.</response>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RoleDto>> CreateRole(
        [FromBody, BindRequired] CreateRoleDto roleDto,
        CancellationToken cancellationToken)
    {
        var command = this.mapper.Map<CreateRoleCommand>(roleDto);
        var roleCreationResult = await this.Mediator.Send(command, cancellationToken);

        if (roleCreationResult.IsResultFailed)
        {
            return this.CreateBadResponseByServiceResult(roleCreationResult);
        }

        var createdRoleDto = this.mapper.Map<RoleDto>(roleCreationResult.Data);

        var getRoleLink = this.httpContextService.GenerateGetRoleLink(createdRoleDto.Id);

        return this.Created(getRoleLink, createdRoleDto);
    }

    /// <summary>
    /// Updates role entity.
    /// </summary>
    /// <param name="roleDto"><see cref="RoleDto"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">Role details have been updated.</response>
    /// <response code="404">Unable to find role.</response>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpPut]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoleDto>> UpdateRole(
        [FromBody, BindRequired] UpdateRoleDto roleDto,
        CancellationToken cancellationToken)
    {
        var command = this.mapper.Map<UpdateRoleCommand>(roleDto);
        var roleUpdateResult = await this.Mediator.Send(command, cancellationToken);

        if (roleUpdateResult.IsResultFailed)
        {
            return this.CreateBadResponseByServiceResult(roleUpdateResult);
        }

        return this.mapper.Map<RoleDto>(roleUpdateResult.Data);
    }

    /// <summary>
    /// Updates role entity with "IsDeleted=true".
    /// </summary>
    /// <param name="id">Role identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="204">Role status "IsDeleted" has been set to true.</response>
    /// <response code="404">Unable to find role.</response>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpDelete("id/{id:guid}/soft-remove")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> SoftRemoveUser(Guid id, CancellationToken cancellationToken)
    {
        var command = new SoftRemoveRoleByIdCommand(id);
        var roleRemoveResult = await this.Mediator.Send(command, cancellationToken);

        if (roleRemoveResult.IsResultFailed)
        {
            return this.CreateBadResponseByServiceResult(roleRemoveResult);
        }

        return this.NoContent();
    }

    /// <summary>
    /// Removes role entity.
    /// </summary>
    /// <param name="id">Role identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="204">Role has been removed from DB.</response>
    /// <response code="404">Unable to find role.</response>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpDelete("id/{id:guid}/hard-remove")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RemoveRole(Guid id, CancellationToken cancellationToken)
    {
        var command = new HardRemoveRoleByIdCommand(id);
        var roleRemoveResult = await this.Mediator.Send(command, cancellationToken);

        if (roleRemoveResult.IsResultFailed)
        {
            return this.CreateBadResponseByServiceResult(roleRemoveResult);
        }

        return this.NoContent();
    }
}
