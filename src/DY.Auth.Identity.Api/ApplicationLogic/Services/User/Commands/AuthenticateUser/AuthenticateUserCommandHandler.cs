using DY.Auth.Identity.Api.Core.Constants;
using DY.Auth.Identity.Api.Core.Entities;
using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Results;
using DY.Auth.Identity.Api.Infrastructure.Database;
using DY.Auth.Identity.Api.Presentation.Services;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.AuthenticateUser;

/// <summary>
/// Authenticate user CQRS handler.
/// </summary>
public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, ServiceResult<AuthenticateUserResult>>
{
    private const string FailedAuthenticationErrorMessage = "Cannot authenticate user with provided email and password";

    private readonly DatabaseContext databaseContext;
    private readonly SignInManager<AppUser> signInManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext">Instance of <see cref="DatabaseContext"/>.</param>
    /// <param name="signInManager">Instance of <see cref="SignInManager{T}"/>.</param>
    public AuthenticateUserCommandHandler(
        DatabaseContext databaseContext,
        SignInManager<AppUser> signInManager)
    {
        this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
        this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<AuthenticateUserResult>> Handle(
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

        var userRoleNames = GetUserRoleNames(user);

        var userClaimsIdentity = new AuthenticateUserResult
        {
            AuthenticatedUser = ClaimsService.AssignClaims(
                user.Id,
                user.Email,
                userRoleNames,
                authScheme: AuthConstants.AppAuthPolicyName),
        };

        return new ServiceResult<AuthenticateUserResult>(ServiceResultType.Success, userClaimsIdentity);
    }

    private static ServiceResult<AuthenticateUserResult> GenerateFailedAuthResult() =>
        new(ServiceResultType.InvalidData, FailedAuthenticationErrorMessage);

    private static IReadOnlyCollection<string> GetUserRoleNames(AppUser user) =>
        user.UserRoles.Select(userRole => userRole.Role.Name).ToList();

    private async Task<AppUser> GetUserByEmail(string email, CancellationToken cancellationToken) =>
        await this.databaseContext.Users
            .AsNoTracking()
            .Include(user => user.UserRoles)
            .ThenInclude(userRole => userRole.Role)
            .SingleOrDefaultAsync(user => user.Email.ToLower() == email.ToLower(), cancellationToken);
}
