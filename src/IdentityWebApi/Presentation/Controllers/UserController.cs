using System;
using System.Threading.Tasks;
using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Constants;
using IdentityWebApi.Core.Interfaces.ApplicationLogic;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IdentityWebApi.Presentation.Controllers;

public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IClaimsService _claimsService;

    public UserController(IUserService userService, IClaimsService claimsService)
    {
        _userService = userService;
        _claimsService = claimsService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserResultDto>> GetUserByToken()
    {
        var userId = _claimsService.GetUserIdFromIdentityUser(User);

        var userResult = await _userService.GetUserAsync(userId.Data);

        if (userResult.IsResultNotFound)
        {
            return NotFound();
        }

        return userResult.Data;
    }

    [Authorize]
    [HttpGet("id/{id:guid}")]
    public async Task<ActionResult<UserResultDto>> GetUser(Guid id)
    {
        var userResult = await _userService.GetUserAsync(id);

        if (userResult.IsResultNotFound)
        {
            return NotFound();
        }

        return userResult.Data;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<UserResultDto>> CreateUser([FromBody, BindRequired] UserDto user) =>
        (await _userService.CreateUserAsync(user)).Data;

    [Authorize(Roles = UserRoleConstants.Admin)]
    [HttpPut]
    public async Task<ActionResult<UserResultDto>> UpdateUser([FromBody, BindRequired] UserDto user)
    {
        var userUpdateResult = await _userService.UpdateUserAsync(user);

        if (userUpdateResult.IsResultNotFound)
        {
            return NotFound(userUpdateResult.Message);
        }

        return userUpdateResult.Data;
    }

    [Authorize(Roles = UserRoleConstants.Admin)]
    [HttpDelete("id/{id:guid}")]
    public async Task<IActionResult> RemoveUser(Guid id)
    {
        var result = await _userService.RemoveUserAsync(id);

        if (result.IsResultNotFound)
        {
            return NotFound();
        }

        return NoContent();
    }
}