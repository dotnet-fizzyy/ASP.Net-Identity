using System.Net;
using System.Threading.Tasks;
using IdentityWebApi.BL.Constants;
using IdentityWebApi.BL.Enums;
using IdentityWebApi.BL.Interfaces;
using IdentityWebApi.PL.Models.Action;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IdentityWebApi.PL.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly IClaimsService _claimsService;
        
        public AuthController(IAuthService authService, IEmailService emailService, IClaimsService claimsService)
        {
            _authService = authService;
            _emailService = emailService;
            _claimsService = claimsService;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUpUser([FromBody, BindRequired] UserRegistrationActionModel userModel)
        {
            var creationResult = await _authService.SignUpUserAsync(userModel);
            if (creationResult.Result is ServiceResultType.NotFound)
            {
                return StatusCode((int)HttpStatusCode.NotFound, creationResult.Message);
            }
            
            var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { email = creationResult.Data.userDto.Email, creationResult.Data.token }, Request.Scheme);
            await _emailService.SendEmailAsync(creationResult.Data.userDto.Email, EmailSubjects.AccountConfirmation, $"<a href='{confirmationLink}'>confirm</a>");

            return StatusCode((int)HttpStatusCode.Created, creationResult.Data.userDto);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody, BindRequired] UserRegistrationActionModel userModel)
        {
            var signInResult = await _authService.SignInUserAsync(userModel);
            if (signInResult.Result is ServiceResultType.InvalidData)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, signInResult.Message);
            }

            var claims = _claimsService.AssignClaims(signInResult.Data);
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claims);
            
            return StatusCode((int)HttpStatusCode.OK, signInResult.Data);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return StatusCode((int)HttpStatusCode.BadRequest,"Missing email or token provided");
            }

            var confirmationResult = await _authService.ConfirmUserEmailAsync(email, token);
            if (confirmationResult.Result is not ServiceResultType.Success)
            {
                return StatusCode((int)HttpStatusCode.BadRequest,confirmationResult.Message);
            }
            
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}