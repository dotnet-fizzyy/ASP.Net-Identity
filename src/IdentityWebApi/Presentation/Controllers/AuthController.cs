using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Constants;
using IdentityWebApi.Core.Interfaces.ApplicationLogic;
using IdentityWebApi.Core.Interfaces.Presentation;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
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
    /// <param name="userModel"></param>
    /// <response code="201">Created user</response>
    /// <response code="404">Unable to create user due to missing role</response>
    [HttpPost("sign-up")]
    [ProducesResponseType(typeof(UserResultDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SignUpUser([FromBody, BindRequired] UserRegistrationDto userModel)
    {
        var creationResult = await _authService.SignUpUserAsync(userModel);
        
        if (creationResult.IsResultNotFound)
        {
            // todo: replace 404 with 400
            return GetFailedResponseByServiceResult(creationResult);
        }

        var confirmationLink = _httpContextService.GenerateConfirmEmailLink(
            creationResult.Data.userDto.Email, 
            creationResult.Data.token
        );

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
    /// <param name="userModel"></param>
    /// <response code="200">User has authenticated</response>
    /// <response code="400">Unable to authenticate with provided credentials</response>
    [HttpPost("sign-in")]
    [ProducesResponseType(typeof(UserResultDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResultDto>> SignIn([FromBody, BindRequired] UserSignInDto userModel)
    {
        var signInResult = await _authService.SignInUserAsync(userModel);
        
        if (signInResult.IsResultFailed)
        {
            return GetFailedResponseByServiceResult(signInResult);
        }

        var claims = _claimsService.AssignClaims(signInResult.Data);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claims);

        return signInResult.Data;
    }

    /// <summary>
    /// User account email confirmation
    /// </summary>
    /// <param name="email"></param>
    /// <param name="token"></param>
    /// <response code="204">Email has been confirmed</response>
    /// <response code="404">User with provided email is not found</response>
    [HttpGet("confirm-email")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmEmail([FromQuery, BindRequired] string email, [FromQuery, BindRequired] string token)
    {
        var confirmationResult = await _authService.ConfirmUserEmailAsync(email, token);

        if (confirmationResult.IsResultFailed)
        {
            return GetFailedResponseByServiceResult(confirmationResult);
        }

        return NoContent();
    }
}