using System;
using System.Threading.Tasks;
using IdentityWebApi.BL.Enums;
using IdentityWebApi.BL.Interfaces;
using IdentityWebApi.PL.Constants;
using IdentityWebApi.PL.Models.DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IdentityWebApi.PL.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("id/{id:guid}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            var userResult = await _userService.GetUserAsync(id);
            if (userResult.Result is not ServiceResultType.NotFound)
            {
                return NotFound();
            }

            return userResult.Data;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = UserRoleConstants.Admin)]
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody, BindRequired] UserDto user)
        {
            var userCreationResult = await _userService.CreateUserAsync(user);
            if (userCreationResult.Result is ServiceResultType.NotFound)
            {
                return NotFound(userCreationResult.Message);
            }
            
            return userCreationResult.Data;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = UserRoleConstants.Admin)]
        [HttpPut]
        public async Task<ActionResult<UserDto>> UpdateUser([FromBody, BindRequired] UserDto user)
        {
            var userUpdateResult = await _userService.UpdateUserAsync(user);
            if (userUpdateResult.Result is ServiceResultType.NotFound)
            {
                return NotFound(userUpdateResult.Message);
            }
            
            return userUpdateResult.Data;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = UserRoleConstants.Admin)]
        [HttpDelete("id/{id:guid}")]
        public async Task<IActionResult> RemoveUser(Guid id)
        {
            var result = await _userService.RemoveUserAsync(id);
            if (result.Result is ServiceResultType.NotFound)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}