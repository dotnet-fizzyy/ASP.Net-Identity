using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.ApplicationLogic.Models.Output;
using IdentityWebApi.ApplicationLogic.Services.Role.Commands.GrantRoleToUser;
using IdentityWebApi.ApplicationLogic.Services.Role.Commands.HardRemoveRoleById;
using IdentityWebApi.ApplicationLogic.Services.Role.Commands.RevokeRoleFromUser;
using IdentityWebApi.ApplicationLogic.Services.Role.Queries.GetRoleById;
using IdentityWebApi.Core.Constants;
using IdentityWebApi.Core.Interfaces.ApplicationLogic;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.Presentation.Controllers;

/// <summary>
/// Role controller.
/// </summary>
[Authorize(Roles = UserRoleConstants.Admin)]
public class RoleController : ControllerBase
{
    private readonly IRoleService roleService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleController"/> class.
    /// </summary>
    /// <param name="mediator">Instance of <see cref="IMediator"/>.</param>
    /// <param name="roleService">Instance of <see cref="IRoleService"/>.</param>
    public RoleController(IMediator mediator, IRoleService roleService)
        : base(mediator)
    {
        this.roleService = roleService;
    }

    /// <summary>
    /// Returns role.
    /// </summary>
    /// <param name="id">Role identifier.</param>
    /// <response code="200">Role has been found.</response>
    /// <response code="404">Unable to find role.</response>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation with <see cref="RoleResult"/> Role.
    /// </returns>
    [HttpGet("id/{id:guid}")]
    [ProducesResponseType(typeof(RoleResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoleResult>> GetRoleById(Guid id)
    {
        var query = new GetRoleByIdQuery(id);
        var roleResult = await this.Mediator.Send(query);

        if (roleResult.IsResultFailed)
        {
            return this.CreateResponseByServiceResult(roleResult);
        }

        return roleResult.Data;
    }

    /// <summary>
    /// Grants role to user.
    /// </summary>
    /// <param name="userRoleDto"><see cref="UserRoleDto"/>.</param>
    /// <response code="204">Role has been granted.</response>
    /// <response code="404">Unable to find role.</response>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    [HttpPost("grant")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GrantRoleToUser([FromBody, BindRequired] UserRoleDto userRoleDto)
    {
        var command = new GrantRoleToUserCommand(userRoleDto.UserId, userRoleDto.RoleId);
        var roleGrantResult = await this.Mediator.Send(command);

        if (roleGrantResult.IsResultFailed)
        {
            return this.CreateResponseByServiceResult(roleGrantResult);
        }

        return this.NoContent();
    }

    /// <summary>
    /// Revokes role from user.
    /// </summary>
    /// <param name="userRoleDto"><see cref="UserRoleDto"/>.</param>
    /// <response code="204">Role has been revoked.</response>
    /// <response code="404">Unable to find role.</response>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    [HttpPost("revoke")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RevokeRoleFromUser([FromBody, BindRequired] UserRoleDto userRoleDto)
    {
        var command = new RevokeRoleFromUserCommand(userRoleDto.UserId, userRoleDto.RoleId);
        var roleRevokeResult = await this.Mediator.Send(command);

        if (roleRevokeResult.IsResultFailed)
        {
            return this.CreateResponseByServiceResult(roleRevokeResult);
        }

        return this.NoContent();
    }

    /// <summary>
    /// Creates role entity.
    /// </summary>
    /// <param name="roleDto"><see cref="RoleCreationDto"/>.</param>
    /// <response code="201">Role has been created.</response>
    /// <response code="400">Role already exists.</response>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RoleDto>> CreateRole([FromBody, BindRequired] RoleCreationDto roleDto)
    {
        var roleCreationResult = await this.roleService.CreateRoleAsync(roleDto);

        if (roleCreationResult.IsResultFailed)
        {
            return this.CreateResponseByServiceResult(roleCreationResult);
        }

        return this.CreatedAtAction(nameof(this.CreateRole), roleCreationResult.Data);
    }

    /// <summary>
    /// Updates role entity.
    /// </summary>
    /// <param name="roleDto"><see cref="RoleDto"/>.</param>
    /// <response code="200">Role details have been updated.</response>
    /// <response code="404">Unable to find role.</response>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    [HttpPut]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoleDto>> UpdateRole([FromBody, BindRequired] RoleDto roleDto)
    {
        var roleUpdateResult = await this.roleService.UpdateRoleAsync(roleDto);

        if (roleUpdateResult.IsResultFailed)
        {
            return this.CreateResponseByServiceResult(roleUpdateResult);
        }

        return roleUpdateResult.Data;
    }

    /// <summary>
    /// Removes role entity.
    /// </summary>
    /// <param name="id">Role identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="204">Role has been removed from DB.</response>
    /// <response code="404">Unable to find role.</response>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Authorize(Roles = UserRoleConstants.Admin)]
    [HttpDelete("id/{id:guid}/hard-remove")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveRole(Guid id, CancellationToken cancellationToken)
    {
        var command = new HardRemoveRoleByIdCommand(id);
        var roleRemoveResult = await this.Mediator.Send(command, cancellationToken);

        if (roleRemoveResult.IsResultFailed)
        {
            return this.CreateResponseByServiceResult(roleRemoveResult);
        }

        return this.NoContent();
    }
}
