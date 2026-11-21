using DY.Auth.Identity.Api.ApplicationLogic.User.Commands.ConfirmEmail;
using DY.Auth.Identity.Api.Core.Constants;
using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Interfaces.Presentation;
using DY.Auth.Identity.Api.Presentation.Mapping;
using DY.Auth.Identity.Api.Presentation.Models.DTO.User;
using DY.Auth.Identity.Api.Presentation.Services;
using DY.Auth.Identity.Api.Startup.ApplicationSettings;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.Presentation.Controllers;

/// <summary>
/// Authentication controller.
/// </summary>
/// <param name="mediator">The instance of <see cref="IMediator"/>.</param>
/// <param name="cookiesService">The instance of <see cref="ICookiesService"/>.</param>
/// <param name="controllersLinkService">The instance of <see cref="IControllersLinkService"/>.</param>
/// <param name="appSettings">The instance of <see cref="AppSettings"/>.</param>
public class AuthController(
    IMediator mediator,
    ICookiesService cookiesService,
    IControllersLinkService controllersLinkService,
    AppSettings appSettings) : ControllerBase(mediator)
{
    private readonly ICookiesService cookiesService = cookiesService ?? throw new ArgumentNullException(nameof(cookiesService));
    private readonly IControllersLinkService controllersLinkService = controllersLinkService ?? throw new ArgumentNullException(nameof(controllersLinkService));
    private readonly AppSettings appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));

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
        var command = UserMapper.ToCreateUserCommand(requestBody);
        var creationResult = await this.Mediator.Send(command, cancellationToken);

        if (creationResult.IsResultFailed)
        {
            return this.CreateErrorResponse(creationResult);
        }

        var userDto = UserMapper.ToUserDto(creationResult.Data);

        var getUserLink = this.controllersLinkService.GenerateGetUserLink(userDto.Id);

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
    public async Task<ActionResult> SignIn(
        [FromBody, BindRequired] UserSignInDto requestBody,
        CancellationToken cancellationToken)
    {
        var command = UserMapper.ToAuthenticateUserCommand(requestBody);
        var authUserResult = await this.Mediator.Send(command, cancellationToken);

        if (authUserResult.IsResultFailed)
        {
            return this.CreateErrorResponse(authUserResult);
        }

        return await this.GetSuccessfulAuthenticationResponse(authUserResult.Data.AuthenticatedUser);
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
            return this.CreateErrorResponse(confirmationResult);
        }

        return this.NoContent();
    }

    private async Task<ActionResult> GetSuccessfulAuthenticationResponse(ClaimsPrincipal userClaimsPrincipal)
    {
        if (this.appSettings.IdentitySettings.AuthType == AuthType.Jwt)
        {
            var authenticationResultDto = this.GenerateJwtAuthenticationResult(userClaimsPrincipal.Claims);

            return this.Ok(authenticationResultDto);
        }

        await this.cookiesService.SignInUsingCookiesAsync(userClaimsPrincipal);

        return this.Ok();
    }

    private AuthResultDto GenerateJwtAuthenticationResult(IEnumerable<Claim> claims)
    {
        var jwtSettings = this.appSettings.IdentitySettings.Jwt;

        var tokenExpirationTime = TimeSpan.FromMinutes(jwtSettings.ExpirationMinutes);
        var tokenExpirationDateTime = DateTime.UtcNow.Add(tokenExpirationTime);

        var accessToken = JwtService.GenerateJwtToken(
            jwtSettings.IssuerSigningKey,
            jwtSettings.ValidIssuer,
            jwtSettings.ValidAudience,
            tokenExpirationDateTime,
            claims);

        return new AuthResultDto
        {
            AccessToken = accessToken,
            Type = AuthConstants.JwtBearerAuthType,
            Expires = ((DateTimeOffset)tokenExpirationDateTime).ToUnixTimeSeconds(),
        };
    }
}
