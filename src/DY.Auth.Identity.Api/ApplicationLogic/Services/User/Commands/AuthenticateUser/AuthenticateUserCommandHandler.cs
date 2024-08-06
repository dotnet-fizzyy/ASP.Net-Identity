using AutoMapper;

using DY.Auth.Identity.Api.ApplicationLogic.Models.Output;
using DY.Auth.Identity.Api.Core.Constants;
using DY.Auth.Identity.Api.Core.Entities;
using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Interfaces.Presentation;
using DY.Auth.Identity.Api.Core.Results;
using DY.Auth.Identity.Api.Infrastructure.Database;
using DY.Auth.Identity.Api.Presentation.Services;
using DY.Auth.Identity.Api.Startup.ApplicationSettings;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.AuthenticateUser;

/// <summary>
/// Authenticate user CQRS handler.
/// </summary>
public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, ServiceResult<AuthUserResult>>
{
    private const string FailedAuthenticationErrorMessage = "Cannot authenticate user with provided email and password";

    private readonly DatabaseContext databaseContext;
    private readonly SignInManager<AppUser> signInManager;
    private readonly AppSettings appSettings;
    private readonly IHttpContextService httpContextService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext">Instance of <see cref="DatabaseContext"/>.</param>
    /// <param name="appSettings">Instance of <see cref="AppSettings"/>.</param>
    /// <param name="signInManager">Instance of <see cref="SignInManager{T}"/>.</param>
    /// <param name="httpContextService">Instance of <see cref="IHttpContextService"/>.</param>
    /// <param name="mapper">Instance of <see cref="IMapper"/>.</param>
    public AuthenticateUserCommandHandler(
        DatabaseContext databaseContext,
        SignInManager<AppUser> signInManager,
        AppSettings appSettings,
        IHttpContextService httpContextService,
        IMapper mapper)
    {
        this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
        this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        this.appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        this.httpContextService = httpContextService ?? throw new ArgumentNullException(nameof(httpContextService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<AuthUserResult>> Handle(
        AuthenticateUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = await this.GetUserByEmail(command.Email, cancellationToken);

        if (user == null)
        {
            return GenerateFailedAuthResult();
        }

        var signInResult =
              await this.signInManager.CheckPasswordSignInAsync(
                user,
                command.Password,
                lockoutOnFailure: false);

        if (!signInResult.Succeeded)
        {
            return GenerateFailedAuthResult();
        }

        var authScheme = AuthConstants.AppAuthPolicyName;
        var userRoleNames = GetUserRoleNames(user);
        string token = null;

        var authUser =
              ClaimsService.AssignClaims(
                user.Id,
                user.Email,
                userRoleNames,
                authScheme);

        if (this.appSettings.IdentitySettings.AuthType == AuthType.Jwt)
        {
            token = this.GenerateJwt(authUser);
        }
        else
        {
            await this.httpContextService.SignInUsingCookiesAsync(authUser);
        }

        var authUserResponse = new AuthUserResult
        {
            Token = token,
            User = this.mapper.Map<UserResult>(user),
        };

        return new ServiceResult<AuthUserResult>(ServiceResultType.Success, authUserResponse);
    }

    private static ServiceResult<AuthUserResult> GenerateFailedAuthResult() =>
        new ServiceResult<AuthUserResult>(ServiceResultType.InvalidData, FailedAuthenticationErrorMessage);

    private static IReadOnlyCollection<string> GetUserRoleNames(AppUser user) =>
        user.UserRoles.Select(userRole => userRole.Role.Name).ToList();

    private async Task<AppUser> GetUserByEmail(string email, CancellationToken cancellationToken) =>
        await this.databaseContext.Users
            .AsNoTracking()
            .Include(user => user.UserRoles)
            .ThenInclude(userRole => userRole.Role)
            .SingleOrDefaultAsync(user => user.Email.ToLower() == email.ToLower(), cancellationToken);

    private string GenerateJwt(ClaimsPrincipal authUser)
    {
        var jwtSettings = this.appSettings.IdentitySettings.Jwt;
        var tokenExpirationTime = TimeSpan.FromMinutes(jwtSettings.ExpirationMinutes);

        return JwtService.GenerateJwtToken(
            jwtSettings.IssuerSigningKey,
            jwtSettings.ValidIssuer,
            jwtSettings.ValidAudience,
            tokenExpirationTime,
            authUser.Claims);
    }
}
