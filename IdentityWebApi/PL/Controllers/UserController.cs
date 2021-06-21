using System;
using System.Net;
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
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = UserRoleConstants.Admin)]
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("id/{id:guid}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            var userResult = await _userService.GetUserAsync(id);
            if (userResult.ServiceResultType is ServiceResultType.NotFound)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            return userResult.Data;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody, BindRequired] UserDto user)
        {
            var userCreationResult = await _userService.CreateUserAsync(user);
            if (userCreationResult.ServiceResultType is ServiceResultType.NotFound)
            {
                return StatusCode((int)HttpStatusCode.NotFound, userCreationResult.Message);
            }
            
            return userCreationResult.Data;
        }

        [HttpPut]
        public async Task<ActionResult<UserDto>> UpdateUser([FromBody, BindRequired] UserDto user)
        {
            var userUpdateResult = await _userService.UpdateUserAsync(user);
            if (userUpdateResult.ServiceResultType is ServiceResultType.NotFound)
            {
                return StatusCode((int)HttpStatusCode.NotFound, userUpdateResult.Message);
            }
            
            return userUpdateResult.Data;
        }

        [HttpDelete("id/{id:guid}")]
        public async Task<IActionResult> RemoveUser(Guid id)
        {
            var result = await _userService.RemoveUserAsync(id);
            if (result.ServiceResultType is ServiceResultType.NotFound)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }

            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}