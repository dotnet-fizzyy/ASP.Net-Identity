using IdentityWebApi.BL.Constants;
using IdentityWebApi.BL.Enums;
using IdentityWebApi.BL.Interfaces;
using IdentityWebApi.PL.Models.Action;
using IdentityWebApi.PL.Interfaces;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System.Threading.Tasks;

namespace IdentityWebApi.PL.Controllers;

[ApiController]
[Route("api/[controller]")]
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

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUpUser([FromBody, BindRequired] UserRegistrationActionModel userModel)
    {
        var creationResult = await _authService.SignUpUserAsync(userModel);
        
        if (creationResult.Result is ServiceResultType.NotFound)
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

        return Created(getUserLink!, creationResult.Data.userDto);
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody, BindRequired] UserSignInActionModel userModel)
    {
        var signInResult = await _authService.SignInUserAsync(userModel);
        
        if (signInResult.Result is ServiceResultType.InvalidData)
        {
            return BadRequest(signInResult.Message);
        }

        var claims = _claimsService.AssignClaims(signInResult.Data);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claims);

        return Ok(signInResult.Data);
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery, BindRequired] string email, [FromQuery, BindRequired] string token)
    {
        var confirmationResult = await _authService.ConfirmUserEmailAsync(email, token);
     
        if (confirmationResult.Result is not ServiceResultType.Success)
        {
            return BadRequest(confirmationResult.Message);
        }

        return NoContent();
    }
}