using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Constants;
using IdentityWebApi.Core.Interfaces.ApplicationLogic;
using IdentityWebApi.Core.Interfaces.Presentation;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System.Threading.Tasks;

namespace IdentityWebApi.Presentation.Controllers;

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    private readonly IClaimsService _claimsService;
    private readonly IHttpContextService _httpContextService;
    
    public AuthController(
        IAuthService authService, 
        IEmailService emailService, 
        IClaimsService claimsService, 
        IHttpContextService httpContextService
    )
    {
        _authService = authService;
        _emailService = emailService;
        _claimsService = claimsService;
        _httpContextService = httpContextService;
    }

    /// <summary>
    /// User account creation
    /// </summary>
    /// <response code="201">Created user</response>
    /// <response code="404">Unable to create user</response>
    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUpUser([FromBody, BindRequired] UserRegistrationDto userModel)
    {
        var creationResult = await _authService.SignUpUserAsync(userModel);
        
        if (creationResult.IsResultNotFound)
        {
            return NotFound(creationResult.Message);
        }

        var confirmationLink = _httpContextService.GenerateConfirmEmailLink(creationResult.Data.userDto.Email, creationResult.Data.token);

        await _emailService.SendEmailAsync(
            creationResult.Data.userDto.Email, 
            EmailSubjects.AccountConfirmation,
            $"<a href='{confirmationLink}'>confirm</a>"
        );

        var getUserLink = _httpContextService.GenerateGetUserLink(creationResult.Data.userDto.Id);

        return Created(getUserLink, creationResult.Data.userDto);
    }

    /// <summary>
    /// User account authentication
    /// </summary>
    /// <response code="200">User has authenticated</response>
    /// <response code="400">Unable to authenticate with provided credentials</response>
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody, BindRequired] UserSignInDto userModel)
    {
        var signInResult = await _authService.SignInUserAsync(userModel);
        
        if (signInResult.IsResultFailed)
        {
            return BadRequest(signInResult.Message);
        }

        var claims = _claimsService.AssignClaims(signInResult.Data);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claims);

        return Ok(signInResult.Data);
    }

    /// <summary>
    /// User account email confirmation
    /// </summary>
    /// <response code="200">Email has been confirmed</response>
    /// <response code="400">Unable to create user</response>
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery, BindRequired] string email, [FromQuery, BindRequired] string token)
    {
        var confirmationResult = await _authService.ConfirmUserEmailAsync(email, token);

        if (confirmationResult.IsResultFailed)
        {
            return BadRequest(confirmationResult.Message);
        }

        return NoContent();
    }
}