using AutoMapper;

using DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.AuthenticateUser;
using DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.ConfirmEmail;
using DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.CreateUser;
using DY.Auth.Identity.Api.Core.Interfaces.Presentation;
using DY.Auth.Identity.Api.Presentation.Mapping;
using DY.Auth.Identity.Api.Presentation.Models.DTO.User;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.Presentation.Controllers;

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
        this.httpContextService = httpContextService ?? throw new ArgumentNullException(nameof(httpContextService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// User account registration.
    /// </summary>
    /// <param name="requestBody"><see cref="UserRegistrationDto"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="201">Created user.</response>
    /// <response code="404">Unable to create user due to missing role.</response>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpPost("sign-up")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> SignUpUser(
        [FromBody, BindRequired] UserRegistrationDto requestBody,
        CancellationToken cancellationToken)
    {
        var command = this.mapper.Map<CreateUserCommand>(
            requestBody,
            opts => opts.Items[UserProfile.ConfirmUserEmailContextKey] = false);
        var creationResult = await this.Mediator.Send(command, cancellationToken);

        if (creationResult.IsResultFailed)
        {
            return this.CreateBadResponseByServiceResult(creationResult);
        }

        var userDto = this.mapper.Map<UserDto>(creationResult.Data);

        var getUserLink = this.httpContextService.GenerateGetUserLink(userDto.Id);

        return this.Created(getUserLink, userDto);
    }

    /// <summary>
    /// User account authentication.
    /// </summary>
    /// <param name="requestBody"><see cref="UserSignInDto"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">User has authenticated.</response>
    /// <response code="400">Unable to authenticate with provided credentials.</response>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpPost("sign-in")]
    [ProducesResponseType(typeof(AuthResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResultDto>> SignIn(
        [FromBody, BindRequired] UserSignInDto requestBody,
        CancellationToken cancellationToken)
    {
        var command = this.mapper.Map<AuthenticateUserCommand>(requestBody);
        var authUserResult = await this.Mediator.Send(command, cancellationToken);

        if (authUserResult.IsResultFailed)
        {
            return this.CreateBadResponseByServiceResult(authUserResult);
        }

        return this.mapper.Map<AuthResultDto>(authUserResult.Data);
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
            return this.CreateBadResponseByServiceResult(confirmationResult);
        }

        return this.NoContent();
    }
}
