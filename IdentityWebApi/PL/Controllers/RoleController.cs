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
    [Route("api/role")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody, BindRequired] RoleDto roleDto)
        {
            var roleCreationResult = await _roleService.CreateRoleAsync(roleDto);
            if (roleCreationResult.Result is not ServiceResultType.Success)
            {
                return StatusCode((int)(roleCreationResult.Result is ServiceResultType.InvalidData 
                    ? HttpStatusCode.BadRequest 
                    : HttpStatusCode.InternalServerError
                ), roleCreationResult.Message);
            }
            
            return StatusCode((int)HttpStatusCode.Created, roleCreationResult.Data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRole([FromBody, BindRequired] RoleDto roleDto)
        {
            var roleUpdateResult = await _roleService.UpdateRoleAsync(roleDto);
            if (roleUpdateResult.Result is not ServiceResultType.Success)
            {
                return StatusCode((int)(roleUpdateResult.Result is ServiceResultType.InvalidData 
                        ? HttpStatusCode.BadRequest 
                        : HttpStatusCode.InternalServerError
                    ), roleUpdateResult.Message);
            }
            
            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpDelete("id/{id:guid}")]
        public async Task<IActionResult> RemoveRole(Guid id)
        {
            var roleRemoveResult = await _roleService.RemoveRoleAsync(id);
            if (roleRemoveResult.Result is not ServiceResultType.Success)
            {
                return StatusCode((int)(roleRemoveResult.Result is ServiceResultType.NotFound 
                        ? HttpStatusCode.NotFound 
                        : HttpStatusCode.InternalServerError
                    ), roleRemoveResult.Message);
            }
            
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}