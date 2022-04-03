using IdentityWebApi.Core.Interfaces.ApplicationLogic;
using IdentityWebApi.Presentation.Models.Action;
using IdentityWebApi.Presentation.Models.DTO;
using IdentityWebApi.Core.Constants;

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
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("id/{id:guid}")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoleById(Guid id)
    {
        var roleResult = await _roleService.GetRoleByIdAsync(id);

        if (roleResult.IsResultFailed)
        {
            return NotFound();
        }

        return Ok(roleResult.Data);
    }

    [HttpPost("grant")]
    public async Task<IActionResult> GrantRoleToUser([FromBody, BindRequired] UserRoleActionModel userRoleActionModel)
    {
        var roleGrantResult = await _roleService.GrantRoleToUserAsync(userRoleActionModel);
        
        if (roleGrantResult.IsResultFailed)
        {
            return GetFailedResponseByServiceResult(roleGrantResult);
        }

        return NoContent();
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeRoleFromUser([FromBody, BindRequired] UserRoleActionModel userRoleActionModel)
    {
        var roleGrantResult = await _roleService.RevokeRoleFromUser(userRoleActionModel);

        if (roleGrantResult.IsResultFailed)
        {
            return GetFailedResponseByServiceResult(roleGrantResult);
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<RoleDto>> CreateRole([FromBody, BindRequired] RoleCreationActionModel roleDto)
    {
        var roleCreationResult = await _roleService.CreateRoleAsync(roleDto);

        if (roleCreationResult.IsResultFailed)
        {
            return GetFailedResponseByServiceResult(roleCreationResult);
        }

        return CreatedAtAction(nameof(CreateRole), roleCreationResult.Data);
    }

    [HttpPut]
    public async Task<ActionResult<RoleDto>> UpdateRole([FromBody, BindRequired] RoleDto roleDto)
    {
        var roleUpdateResult = await _roleService.UpdateRoleAsync(roleDto);

        if (roleUpdateResult.IsResultFailed)
        {
            return GetFailedResponseByServiceResult(roleUpdateResult);
        }

        return roleUpdateResult.Data;
    }

    [HttpDelete("id/{id:guid}")]
    public async Task<IActionResult> RemoveRole(Guid id)
    {
        var roleRemoveResult = await _roleService.RemoveRoleAsync(id);

        if (roleRemoveResult.IsResultFailed)
        {
            return GetFailedResponseByServiceResult(roleRemoveResult);
        }

        return NoContent();
    }
}
