using System;
using System.Threading.Tasks;
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
            => await _userService.GetUser(id);

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody, BindRequired] UserDto user)
            => await _userService.CreateUser(user);

        [HttpPut]
        public async Task<ActionResult<UserDto>> UpdateUser([FromBody, BindRequired] UserDto user)
            => await _userService.UpdateUser(user);
    }
}