using System;
using System.Threading.Tasks;
using IdentityWebApi.BL.Enums;
using IdentityWebApi.BL.Interfaces;
using IdentityWebApi.PL.DTO;
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

        [HttpGet("id/{id:guid}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            var result = await _userService.GetUserAsync(id);
            if (result.ServiceResultType == ServiceResultType.NotFound)
            {
                return NotFound();
            }

            return result.Data;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody, BindRequired] UserDto user)
        {
            var userCreationResult = await _userService.CreateUserAsync(user);
            if (userCreationResult.ServiceResultType == ServiceResultType.NotFound)
            {
                return NotFound(userCreationResult.Message);
            }
            
            return userCreationResult.Data;
        }

        [HttpPut]
        public async Task<ActionResult<UserDto>> UpdateUser([FromBody, BindRequired] UserDto user)
        {
            var userUpdateResult = await _userService.UpdateUserAsync(user);
            if (userUpdateResult.ServiceResultType == ServiceResultType.NotFound)
            {
                return NotFound(userUpdateResult.Message);
            }
            
            return userUpdateResult.Data;
        }

        [HttpDelete("id/{id:guid}")]
        public async Task<IActionResult> RemoveUser(Guid id)
        {
            var result = await _userService.RemoveUserAsync(id);
            if (result.ServiceResultType == ServiceResultType.NotFound)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}