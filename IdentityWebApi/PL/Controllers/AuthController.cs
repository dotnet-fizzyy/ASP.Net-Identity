using System.Threading.Tasks;
using IdentityWebApi.BL.Enums;
using IdentityWebApi.BL.Interfaces;
using IdentityWebApi.PL.Models.Action;
using Microsoft.AspNetCore.Mvc;

namespace IdentityWebApi.PL.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUpUser([FromBody] UserRegistrationActionModel userModel)
        {
            var creationResult = await _authService.SignUpUser(userModel);
            if (creationResult.ServiceResultType == ServiceResultType.NotFound)
            {
                return NotFound(creationResult.Message);
            }
            
            return CreatedAtAction(nameof(SignUpUser), creationResult.Data);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] UserRegistrationActionModel userModel)
        {
            return Ok();
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            return Ok();
        }
    }
}