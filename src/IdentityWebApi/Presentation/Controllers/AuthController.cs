using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.ApplicationLogic.Models.Output;
using IdentityWebApi.ApplicationLogic.Services.User.Commands.AuthenticateUser;
using IdentityWebApi.ApplicationLogic.Services.User.Commands.ConfirmEmail;
using IdentityWebApi.ApplicationLogic.Services.User.Commands.CreateUser;
using IdentityWebApi.Core.Interfaces.Presentation;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.Presentation.Controllers;

/// <summary>
/// Authentication controller.
/// </summary>
public class AuthController : ControllerBase
{
    private readonly IHttpContextService httpContextService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="mediator">The instance of <see cref="IMediator"/>.</param>
    /// <param name="httpContextService">The instance of <see cref="IHttpContextService"/>.</param>
    /// <param name="mapper">The instance of <see cref="IMapper"/>.</param>
    public AuthController(
        IMediator mediator,
        IHttpContextService httpContextService,
        IMapper mapper)
            : base(mediator)
    {
        this.httpContextService = httpContextService;
        this.mapper = mapper;
    }

    /// <summary>
    /// User account registration.
    /// </summary>
    /// <param name="userRegistrationDto"><see cref="UserRegistrationDto"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="201">Created user.</response>
    /// <response code="404">Unable to create user due to missing role.</response>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpPost("sign-up")]
    [ProducesResponseType(typeof(UserResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SignUpUser(
        [FromBody, BindRequired] UserRegistrationDto userRegistrationDto,
        CancellationToken cancellationToken)
    {
        var command = this.mapper.Map<CreateUserCommand>(userRegistrationDto);
        var creationResult = await this.Mediator.Send(command, cancellationToken);

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
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">User has authenticated.</response>
    /// <response code="400">Unable to authenticate with provided credentials.</response>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpPost("sign-in")]
    [ProducesResponseType(typeof(AuthUserResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthUserResult>> SignIn(
        [FromBody, BindRequired] UserSignInDto userModel,
        CancellationToken cancellationToken)
    {
        var command = new AuthenticateUserCommand(userModel.Email, userModel.Password);
        var authUserResult = await this.Mediator.Send(command, cancellationToken);

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
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="204">Email has been confirmed.</response>
    /// <response code="404">User with provided email is not found.</response>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpGet("confirm-email")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmEmail(
        [FromQuery, BindRequired] string email,
        [FromQuery, BindRequired] string token,
        CancellationToken cancellationToken)
    {
        var command = new ConfirmEmailCommand(email, token);
        var confirmationResult = await this.Mediator.Send(command, cancellationToken);

        if (confirmationResult.IsResultFailed)
        {
            return this.CreateResponseByServiceResult(confirmationResult);
        }

        return this.NoContent();
    }
}
