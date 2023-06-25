using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.ApplicationLogic.Models.Output;
using IdentityWebApi.ApplicationLogic.Services.User.Commands.AuthenticateUser;
using IdentityWebApi.ApplicationLogic.Services.User.Commands.ConfirmEmail;
using IdentityWebApi.ApplicationLogic.Services.User.Commands.CreateUser;
using IdentityWebApi.Core.Constants;
using IdentityWebApi.Core.Interfaces.Presentation;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System.Threading.Tasks;

namespace IdentityWebApi.Presentation.Controllers;

/// <summary>
/// Authentication controller.
/// </summary>
public class AuthController : ControllerBase
{
    private readonly IHttpContextService httpContextService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="httpContextService"><see cref="IHttpContextService"/>.</param>
    /// <param name="mediator"><see cref="IMediator"/>.</param>
    public AuthController(IHttpContextService httpContextService, IMediator mediator)
        : base(mediator)
    {
        this.httpContextService = httpContextService;
    }

    /// <summary>
    /// User account registration.
    /// </summary>
    /// <param name="userRegistrationDto"><see cref="UserRegistrationDto"/>.</param>
    /// <response code="201">Created user.</response>
    /// <response code="404">Unable to create user due to missing role.</response>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpPost("sign-up")]
    [ProducesResponseType(typeof(UserResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SignUpUser([FromBody, BindRequired] UserRegistrationDto userRegistrationDto)
    {
        var createUserCommand = new CreateUserCommand
        {
            Email = userRegistrationDto.Email,
            UserName = userRegistrationDto.UserName,
            Password = userRegistrationDto.Password,
            UserRole = UserRoleConstants.User,
        };

        var creationResult = await this.Mediator.Send(createUserCommand);

        if (creationResult.IsResultFailed)
        {
            return this.CreateResponseByServiceResult(creationResult);
        }

        var getUserLink = this.httpContextService.GenerateGetUserLink(creationResult.Data.Id);

        return this.Created(getUserLink, creationResult.Data);
    }

    /// <summary>
    /// User account authentication.
    /// </summary>
    /// <param name="userModel"><see cref="UserSignInDto"/>.</param>
    /// <response code="200">User has authenticated.</response>
    /// <response code="400">Unable to authenticate with provided credentials.</response>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpPost("sign-in")]
    [ProducesResponseType(typeof(AuthUserResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthUserResult>> SignIn([FromBody, BindRequired] UserSignInDto userModel)
    {
        var authUserCommand = new AuthenticateUserCommand(userModel.Email, userModel.Password);
        var authUserResult = await this.Mediator.Send(authUserCommand);

        if (authUserResult.IsResultFailed)
        {
            return this.CreateResponseByServiceResult(authUserResult);
        }

        return authUserResult.Data;
    }

    /// <summary>
    /// User account email confirmation.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="token">Confirmation token.</param>
    /// <response code="204">Email has been confirmed.</response>
    /// <response code="404">User with provided email is not found.</response>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpGet("confirm-email")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmEmail(
        [FromQuery, BindRequired] string email,
        [FromQuery, BindRequired] string token)
    {
        var command = new ConfirmEmailCommand(email, token);
        var confirmationResult = await this.Mediator.Send(command);

        if (confirmationResult.IsResultFailed)
        {
            return this.CreateResponseByServiceResult(confirmationResult);
        }

        return this.NoContent();
    }
}
