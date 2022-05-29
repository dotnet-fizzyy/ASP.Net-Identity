using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Constants;
using IdentityWebApi.Core.Interfaces.ApplicationLogic;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.Presentation.Controllers;

/// <summary>
/// Role controller (available only for Admin).
/// </summary>
[Authorize(Roles = UserRoleConstants.Admin)]
public class RoleController : ControllerBase
{
    private readonly IRoleService roleService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleController"/> class.
    /// </summary>
    /// <param name="roleService"><see cref="IRoleService"/>.</param>
    public RoleController(IRoleService roleService)
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
    /// A <see cref="Task"/> representing the asynchronous operation with <see cref="RoleDto"/> Role.
    /// </returns>
    [HttpGet("id/{id:guid}")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoleDto>> GetRoleById(Guid id)
    {
        var roleResult = await this.roleService.GetRoleByIdAsync(id);

        if (roleResult.IsResultFailed)
        {
            return this.CreateFailedResponseByServiceResult(roleResult);
        }

        return roleResult.Data;
    }

    /// <summary>
    /// Grants role to user.
    /// </summary>
    /// <param name="userRoleDto"><see cref="UserRoleDto"/>.</param>
    /// <response code="204">Role has been found.</response>
    /// <response code="404">Unable to find role.</response>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpPost("grant")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GrantRoleToUser([FromBody, BindRequired] UserRoleDto userRoleDto)
    {
        var roleGrantResult = await this.roleService.GrantRoleToUserAsync(userRoleDto);

        if (roleGrantResult.IsResultFailed)
        {
            return this.CreateFailedResponseByServiceResult(roleGrantResult);
        }

        return this.NoContent();
    }

    /// <summary>
    /// Revokes role from user.
    /// </summary>
    /// <param name="userRoleDto"><see cref="UserRoleDto"/>.</param>
    /// <response code="204">Role has been found.</response>
    /// <response code="404">Unable to find role.</response>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpPost("revoke")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RevokeRoleFromUser([FromBody, BindRequired] UserRoleDto userRoleDto)
    {
        var roleRevocationResult = await this.roleService.RevokeRoleFromUser(userRoleDto);

        if (roleRevocationResult.IsResultFailed)
        {
            return this.CreateFailedResponseByServiceResult(roleRevocationResult);
        }

        return this.NoContent();
    }

    /// <summary>
    /// Creates role entity.
    /// </summary>
    /// <param name="roleDto"><see cref="RoleCreationDto"/>.</param>
    /// <response code="201">Role has been created.</response>
    /// <response code="400">Role already exists.</response>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RoleDto>> CreateRole([FromBody, BindRequired] RoleCreationDto roleDto)
    {
        var roleCreationResult = await this.roleService.CreateRoleAsync(roleDto);

        if (roleCreationResult.IsResultFailed)
        {
            return this.CreateFailedResponseByServiceResult(roleCreationResult);
        }

        return this.CreatedAtAction(nameof(this.CreateRole), roleCreationResult.Data);
    }

    /// <summary>
    /// Updates role entity.
    /// </summary>
    /// <param name="roleDto"><see cref="RoleDto"/>.</param>
    /// <response code="200">Role has been updated.</response>
    /// <response code="404">Unable to find role.</response>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpPut]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoleDto>> UpdateRole([FromBody, BindRequired] RoleDto roleDto)
    {
        var roleUpdateResult = await this.roleService.UpdateRoleAsync(roleDto);

        if (roleUpdateResult.IsResultFailed)
        {
            return this.CreateFailedResponseByServiceResult(roleUpdateResult);
        }

        return roleUpdateResult.Data;
    }

    /// <summary>
    /// Removes role entity.
    /// </summary>
    /// <param name="id">Role identifier.</param>
    /// <response code="204">Role has been removed.</response>
    /// <response code="404">Unable to find role.</response>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpDelete("id/{id:guid}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveRole(Guid id)
    {
        var roleRemoveResult = await this.roleService.RemoveRoleAsync(id);

        if (roleRemoveResult.IsResultFailed)
        {
            return this.CreateFailedResponseByServiceResult(roleRemoveResult);
        }

        return this.NoContent();
    }
}
