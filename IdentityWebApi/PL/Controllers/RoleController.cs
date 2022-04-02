using IdentityWebApi.BL.Enums;
using IdentityWebApi.BL.Interfaces;
using IdentityWebApi.PL.Constants;
using IdentityWebApi.PL.Models.Action;
using IdentityWebApi.PL.Models.DTO;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.PL.Controllers;

[Authorize(Roles = UserRoleConstants.Admin)]
[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("id/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoleDto>> GetRoleById(Guid id)
    {
        var roleResult = await _roleService.GetRoleByIdAsync(id);

        if (roleResult.Result is ServiceResultType.NotFound)
        {
            return NotFound();
        }

        return roleResult.Data;
    }

    [HttpPost("grant")]
    public async Task<IActionResult> GrantRoleToUser([FromBody, BindRequired] UserRoleActionModel userRoleActionModel)
    {
        var roleGrantResult = await _roleService.GrantRoleToUserAsync(userRoleActionModel);
        if (roleGrantResult.Result is not ServiceResultType.Success)
        {
            return StatusCode((int)roleGrantResult.Result, roleGrantResult.Message);
        }

        return NoContent();
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeRoleFromUser(
        [FromBody, BindRequired] UserRoleActionModel userRoleActionModel)
    {
        var roleGrantResult = await _roleService.RevokeRoleFromUser(userRoleActionModel);

        if (roleGrantResult.Result is not ServiceResultType.Success)
        {
            return StatusCode((int)roleGrantResult.Result, roleGrantResult.Message);
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<RoleDto>> CreateRole([FromBody, BindRequired] RoleCreationActionModel roleDto)
    {
        var roleCreationResult = await _roleService.CreateRoleAsync(roleDto);

        if (roleCreationResult.Result is not ServiceResultType.Success)
        {
            return StatusCode((int)roleCreationResult.Result, roleCreationResult.Message);
        }

        return CreatedAtAction(nameof(CreateRole), roleCreationResult.Data);
    }

    [HttpPut]
    public async Task<ActionResult<RoleDto>> UpdateRole([FromBody, BindRequired] RoleDto roleDto)
    {
        var roleUpdateResult = await _roleService.UpdateRoleAsync(roleDto);

        if (roleUpdateResult.Result is not ServiceResultType.Success)
        {
            return StatusCode((int)roleUpdateResult.Result, roleUpdateResult.Message);
        }

        return roleUpdateResult.Data;
    }

    [HttpDelete("id/{id:guid}")]
    public async Task<IActionResult> RemoveRole(Guid id)
    {
        var roleRemoveResult = await _roleService.RemoveRoleAsync(id);

        if (roleRemoveResult.Result is not ServiceResultType.Success)
        {
            return StatusCode((int)roleRemoveResult.Result, roleRemoveResult.Message);
        }

        return NoContent();
    }
}
