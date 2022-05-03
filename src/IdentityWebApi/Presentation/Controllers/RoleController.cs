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

[Authorize(Roles = UserRoleConstants.Admin)]
public class RoleController : ControllerBase
{
    private readonly IRoleService roleService;

    public RoleController(IRoleService roleService)
    {
        this.roleService = roleService;
    }

    /// <summary>
    /// Get role by id.
    /// </summary>
    /// <param name="id">Identifier of role entity.</param>
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
            return this.GetFailedResponseByServiceResult(roleResult);
        }

        return roleResult.Data;
    }

    /// <summary>
    /// Grant role for user
    /// </summary>
    /// <param name="userRoleDto"></param>
    /// <response code="204">Role has been found.</response>
    /// <response code="404">Unable to find role.</response>
    [HttpPost("grant")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GrantRoleToUser([FromBody, BindRequired] UserRoleDto userRoleDto)
    {
        var roleGrantResult = await this.roleService.GrantRoleToUserAsync(userRoleDto);

        if (roleGrantResult.IsResultFailed)
        {
            return this.GetFailedResponseByServiceResult(roleGrantResult);
        }

        return this.NoContent();
    }

    /// <summary>
    /// Revoke role from user
    /// </summary>
    /// <param name="userRoleDto"></param>
    /// <response code="204">Role has been found</response>
    /// <response code="404">Unable to find role</response>
    [HttpPost("revoke")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RevokeRoleFromUser([FromBody, BindRequired] UserRoleDto userRoleDto)
    {
        var roleGrantResult = await this.roleService.RevokeRoleFromUser(userRoleDto);

        if (roleGrantResult.IsResultFailed)
        {
            return this.GetFailedResponseByServiceResult(roleGrantResult);
        }

        return this.NoContent();
    }

    /// <summary>
    /// Create role
    /// </summary>
    /// <param name="roleDto"></param>
    /// <response code="201">Role has been created</response>
    /// <response code="400">Role already exists</response>
    [HttpPost]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RoleDto>> CreateRole([FromBody, BindRequired] RoleCreationDto roleDto)
    {
        var roleCreationResult = await roleService.CreateRoleAsync(roleDto);

        if (roleCreationResult.IsResultFailed)
        {
            return GetFailedResponseByServiceResult(roleCreationResult);
        }

        return CreatedAtAction(nameof(CreateRole), roleCreationResult.Data);
    }

    /// <summary>
    /// Update role by id
    /// </summary>
    /// <param name="roleDto"></param>
    /// <response code="200">Role has been updated</response>
    /// <response code="404">Unable to find role</response>
    [HttpPut]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoleDto>> UpdateRole([FromBody, BindRequired] RoleDto roleDto)
    {
        var roleUpdateResult = await this.roleService.UpdateRoleAsync(roleDto);

        if (roleUpdateResult.IsResultFailed)
        {
            return this.GetFailedResponseByServiceResult(roleUpdateResult);
        }

        return roleUpdateResult.Data;
    }

    /// <summary>
    /// Remove role by id
    /// </summary>
    /// <param name="id"></param>
    /// <response code="204">Role has been removed</response>
    /// <response code="404">Unable to find role</response>
    [HttpDelete("id/{id:guid}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveRole(Guid id)
    {
        var roleRemoveResult = await this.roleService.RemoveRoleAsync(id);

        if (roleRemoveResult.IsResultFailed)
        {
            return this.GetFailedResponseByServiceResult(roleRemoveResult);
        }

        return this.NoContent();
    }
}
