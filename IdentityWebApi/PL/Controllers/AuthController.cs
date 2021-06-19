using System.Threading.Tasks;
using IdentityWebApi.BL.Constants;
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
        private readonly IEmailService _emailService;
        
        public AuthController(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUpUser([FromBody] UserRegistrationActionModel userModel)
        {
            var creationResult = await _authService.SignUpUserAsync(userModel);
            if (creationResult.ServiceResultType == ServiceResultType.NotFound)
            {
                return NotFound(creationResult.Message);
            }
            
            var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { email = creationResult.Data.userDto.Email, creationResult.Data.token }, Request.Scheme);
            await _emailService.SendEmailAsync(creationResult.Data.userDto.Email, EmailSubjects.AccountConfirmation, $"<a href='{confirmationLink}'>confirm</a>");

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
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Missing email or token provided");
            }

            var confirmationResult = await _authService.ConfirmUserEmailAsync(email, token);
            if (confirmationResult.ServiceResultType != ServiceResultType.Success)
            {
                return BadRequest(confirmationResult.Message);
            }
            
            return NoContent();
        }
    }
}