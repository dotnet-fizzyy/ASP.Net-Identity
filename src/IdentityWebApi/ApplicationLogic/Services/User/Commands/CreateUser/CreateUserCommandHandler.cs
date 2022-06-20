using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.ApplicationSettings;
using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Interfaces.ApplicationLogic;
using IdentityWebApi.Core.Results;
using IdentityWebApi.Core.Utilities;
using IdentityWebApi.Infrastructure.Database;
using IdentityWebApi.Infrastructure.Database.Constants;

using MediatR;

using Microsoft.AspNetCore.Identity;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.CreateUser;

/// <summary>
/// Create user CQRS handler.
/// </summary>
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ServiceResult<UserDto>>
{
    private readonly UserManager<AppUser> userManager;
    private readonly RoleManager<AppRole> roleManager;
    private readonly DatabaseContext databaseContext;
    private readonly AppSettings appSettings;
    private readonly IMapper mapper;
    private readonly IEmailService emailService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="userManager"><see cref="UserManager{T}"/>.</param>
    /// <param name="roleManager"><see cref="RoleManager{T}"/>.</param>
    /// <param name="databaseContext"><see cref="DatabaseContext"/>.</param>
    /// <param name="appSettings"><see cref="AppSettings"/>.</param>
    /// <param name="mapper"><see cref="IMapper"/>.</param>
    /// <param name="emailService"><see cref="IEmailService"/>.</param>
    public CreateUserCommandHandler(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        DatabaseContext databaseContext,
        AppSettings appSettings,
        IMapper mapper,
        IEmailService emailService)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.databaseContext = databaseContext;
        this.appSettings = appSettings;
        this.mapper = mapper;
        this.emailService = emailService;
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<UserDto>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var userEntity = this.mapper.Map<AppUser>(command.User);

        var userCreationResult = await this.CreateUser(userEntity, command.User.Password);
        if (userCreationResult.IsResultFailed)
        {
            return GenerateErrorResult(userCreationResult);
        }

        var createdUser = userCreationResult.Data;

        var roleAssignmentResult = await this.AssignRoleToUser(createdUser, command.User.UserRole);
        if (roleAssignmentResult.IsResultFailed)
        {
            return GenerateErrorResult(roleAssignmentResult);
        }

        // todo: Move immediate confirmation to appSettings
        const bool shouldConfirmImmediately = true;

        var confirmationEmailResult = await this.ConfirmUserEmail(createdUser, shouldConfirmImmediately);
        if (confirmationEmailResult.IsResultFailed)
        {
            return GenerateErrorResult(confirmationEmailResult);
        }

        this.SendUserEmailConfirmation(createdUser.Email, shouldConfirmImmediately);

        var userDto = this.mapper.Map<UserDto>(createdUser);

        return new ServiceResult<UserDto>(ServiceResultType.Success, userDto);
    }

    private static ServiceResult<UserDto> GenerateErrorResult(ServiceResult result) =>
        new (result.Result, result.Message);

    private async Task<ServiceResult<AppUser>> CreateUser(AppUser user, string password)
    {
        var userCreationResult = await this.userManager.CreateAsync(user, password);
        if (!userCreationResult.Succeeded)
        {
            return new ServiceResult<AppUser>(
                ServiceResultType.InternalError,
                IdentityUtilities.ConcatenateIdentityErrorMessages(userCreationResult.Errors)
            );
        }

        return new ServiceResult<AppUser>(ServiceResultType.Success, user);
    }

    private async Task<ServiceResult> AssignRoleToUser(AppUser user, string role)
    {
        if (string.IsNullOrEmpty(role))
        {
            return new ServiceResult(ServiceResultType.Success);
        }

        var existingRole = await this.roleManager.FindByNameAsync(role);
        if (existingRole == null)
        {
            return new ServiceResult(
                ServiceResultType.NotFound,
                "No role exists to assign to user"
            );
        }

        var roleAssignmentResult = await this.userManager.AddToRoleAsync(user, role);
        if (!roleAssignmentResult.Succeeded)
        {
            return new ServiceResult<AppUser>(
                ServiceResultType.InternalError,
                IdentityUtilities.ConcatenateIdentityErrorMessages(roleAssignmentResult.Errors)
            );
        }

        return new ServiceResult(ServiceResultType.Success);
    }

    private async Task<ServiceResult<string>> ConfirmUserEmail(AppUser user, bool shouldConfirmImmediately)
    {
        var token = string.Empty;

        if (!this.appSettings.IdentitySettings.Email.RequireConfirmation)
        {
            return new ServiceResult<string>(ServiceResultType.Success, token);
        }

        token = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

        if (shouldConfirmImmediately)
        {
            var emailConfirmationResult = await this.userManager.ConfirmEmailAsync(user, token);
            if (!emailConfirmationResult.Succeeded)
            {
                return new ServiceResult<string>(
                    ServiceResultType.InternalError,
                    IdentityUtilities.ConcatenateIdentityErrorMessages(emailConfirmationResult.Errors)
                );
            }
        }

        return new ServiceResult<string>(ServiceResultType.Success, token);
    }

    private void SendUserEmailConfirmation(string email, bool shouldConfirmImmediately)
    {
        var shouldSendConfirmationEmail = this.appSettings.IdentitySettings.Email.RequireConfirmation &&
                                              !shouldConfirmImmediately;

        if (!shouldSendConfirmationEmail)
        {
            return;
        }

        Task.Run(() => this.SendConfirmationEmail(email)).ConfigureAwait(continueOnCapturedContext: false);
    }

    private void SendConfirmationEmail(string email)
    {
        var emailTemplate = this.databaseContext.EmailTemplates.Single(x =>
            x.Id == EntityConfigurationConstants.EmailConfirmationTemplateId
        );

        // todo: Extract email subject from email template entity
        this.emailService.SendEmailAsync(email, "Confirm your email", emailTemplate.Layout);
    }
}