using System;
using System.Threading.Tasks;
using IdentityWebApi.BL.Enums;
using IdentityWebApi.BL.Interfaces;
using IdentityWebApi.PL.Constants;
using IdentityWebApi.PL.Models.Action;
using IdentityWebApi.PL.Models.DTO;
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
            if (userResult.Result is ServiceResultType.NotFound)
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
            if (userResult.Result is ServiceResultType.NotFound)
            {
                return NotFound();
            }

            return userResult.Data;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<UserResultDto>> CreateUser([FromBody, BindRequired] UserActionModel user)
            => (await _userService.CreateUserAsync(user)).Data;

        [Authorize(Roles = UserRoleConstants.Admin)]
        [HttpPut]
        public async Task<ActionResult<UserResultDto>> UpdateUser([FromBody, BindRequired] UserActionModel user)
        {
            var userUpdateResult = await _userService.UpdateUserAsync(user);
            if (userUpdateResult.Result is ServiceResultType.NotFound)
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
            if (result.Result is ServiceResultType.NotFound)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}