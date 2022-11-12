using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.ApplicationLogic.Services.User.Models;
using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Interfaces.Presentation;
using IdentityWebApi.Core.Results;
using IdentityWebApi.Infrastructure.Database;
using IdentityWebApi.Presentation.Services;
using IdentityWebApi.Startup.ApplicationSettings;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.AuthenticateUser;

/// <summary>
/// Authenticate user CQRS handler.
/// </summary>
public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, ServiceResult<AuthUserResponse>>
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
        this.databaseContext = databaseContext;
        this.signInManager = signInManager;
        this.appSettings = appSettings;
        this.httpContextService = httpContextService;
        this.mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<AuthUserResponse>> Handle(
        AuthenticateUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = await this.GetUserByEmail(command.Email);

        if (user == null)
        {
            return GenerateFailedAuthResult();
        }

        var signInResult = await this.signInManager.CheckPasswordSignInAsync(
            user,
            command.Password,
            lockoutOnFailure: false);

        if (!signInResult.Succeeded)
        {
            return GenerateFailedAuthResult();
        }

        var userRoleNames = GetUserRoleNames(user);
        var token = string.Empty;

        var authUser = ClaimsService.AssignClaims(
            user.Id,
            user.Email,
            userRoleNames);

        if (this.appSettings.IdentitySettings.AuthType == AuthType.Jwt)
        {
            token = this.GenerateJwt(authUser);
        }
        else
        {
            await this.httpContextService.SignInUsingCookiesAsync(authUser);
        }

        var authUserResponse = new AuthUserResponse
        {
            Jwt = token,
            User = this.mapper.Map<UserResultDto>(user),
        };

        return new ServiceResult<AuthUserResponse>(ServiceResultType.Success, authUserResponse);
    }

    private static ServiceResult<AuthUserResponse> GenerateFailedAuthResult() =>
        new (ServiceResultType.InvalidData, FailedAuthenticationErrorMessage);

    private static IEnumerable<string> GetUserRoleNames(AppUser user) =>
        user.UserRoles.Select(userRole => userRole.Role.Name).ToList();

    private async Task<AppUser> GetUserByEmail(string email) =>
        await this.databaseContext.Users
            .AsNoTracking()
            .Include(user => user.UserRoles)
            .ThenInclude(userRole => userRole.Role)
            .SingleOrDefaultAsync(user => user.Email.ToLower() == email.ToLower());

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