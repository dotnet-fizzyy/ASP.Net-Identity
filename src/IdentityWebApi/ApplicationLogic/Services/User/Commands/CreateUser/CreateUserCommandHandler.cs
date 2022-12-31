using AutoMapper;

using HandlebarsDotNet;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Interfaces.Infrastructure;
using IdentityWebApi.Core.Interfaces.Presentation;
using IdentityWebApi.Core.Results;
using IdentityWebApi.Core.Utilities;
using IdentityWebApi.Infrastructure.Database;
using IdentityWebApi.Infrastructure.Database.Constants;
using IdentityWebApi.Startup.ApplicationSettings;

using MediatR;

using Microsoft.AspNetCore.Identity;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.CreateUser;

/// <summary>
/// Create user CQRS handler.
/// </summary>
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ServiceResult<Models.Action.UserDto>>
{
    private readonly UserManager<AppUser> userManager;
    private readonly RoleManager<AppRole> roleManager;
    private readonly DatabaseContext databaseContext;
    private readonly AppSettings appSettings;
    private readonly IEmailService emailService;
    private readonly IHttpContextService httpContextService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="userManager"><see cref="UserManager{T}"/>.</param>
    /// <param name="roleManager"><see cref="RoleManager{T}"/>.</param>
    /// <param name="databaseContext"><see cref="DatabaseContext"/>.</param>
    /// <param name="appSettings"><see cref="AppSettings"/>.</param>
    /// <param name="emailService"><see cref="IEmailService"/>.</param>
    /// <param name="httpContextService"><see cref="IHttpContextService"/>.</param>
    /// <param name="mapper"><see cref="IMapper"/>.</param>
    public CreateUserCommandHandler(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        DatabaseContext databaseContext,
        AppSettings appSettings,
        IEmailService emailService,
        IHttpContextService httpContextService,
        IMapper mapper)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.databaseContext = databaseContext;
        this.appSettings = appSettings;
        this.emailService = emailService;
        this.httpContextService = httpContextService;
        this.mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<Models.Action.UserDto>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var userEntity = this.mapper.Map<AppUser>(command.UserDto);

        var userCreationResult = await this.ProcessUserCreation(userEntity, command.UserDto.Password);
        if (userCreationResult.IsResultFailed)
        {
            return GenerateHandlerErrorResult(userCreationResult);
        }

        var createdUser = userCreationResult.Data;

        if (!string.IsNullOrEmpty(command.UserDto.UserRole))
        {
            var roleAssignmentResult = await this.ProcessRoleAssignment(createdUser, command.UserDto.UserRole);
            if (roleAssignmentResult.IsResultFailed)
            {
                return GenerateHandlerErrorResult(roleAssignmentResult);
            }
        }

        if (this.appSettings.IdentitySettings.Email.RequireConfirmation)
        {
            var confirmationEmailResult = await this.ProcessUserEmailConfirmation(
                createdUser,
                command.ConfirmEmailImmediately);
            if (confirmationEmailResult.IsResultFailed)
            {
                return GenerateHandlerErrorResult(confirmationEmailResult);
            }
        }

        var userDto = this.mapper.Map<Models.Action.UserDto>(createdUser);

        return new ServiceResult<Models.Action.UserDto>(ServiceResultType.Success, userDto);
    }

    private static ServiceResult<Models.Action.UserDto> GenerateHandlerErrorResult(ServiceResult serviceResult) =>
        serviceResult.GenerateErrorResult<Models.Action.UserDto>();

    /// <summary>
    /// Creates user entity.
    /// </summary>
    /// <param name="user">
    /// <see cref="AppUser"/>.
    /// </param>
    /// <param name="password">
    /// User password.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation with <see cref="ServiceResult"/>.
    /// </returns>
    private async Task<ServiceResult<AppUser>> ProcessUserCreation(AppUser user, string password)
    {
        var userCreationResult = await this.userManager.CreateAsync(user, password);
        if (!userCreationResult.Succeeded)
        {
            return new ServiceResult<AppUser>(
                ServiceResultType.InternalError,
                IdentityUtilities.ConcatenateIdentityErrorMessages(userCreationResult.Errors));
        }

        return new ServiceResult<AppUser>(ServiceResultType.Success, user);
    }

    /// <summary>
    /// Assigns role to newly created user.
    /// </summary>
    /// <param name="user">
    /// <see cref="AppUser"/>.
    /// </param>
    /// <param name="role">
    /// Role to assign.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation with <see cref="ServiceResult"/>.
    /// </returns>
    private async Task<ServiceResult> ProcessRoleAssignment(AppUser user, string role)
    {
        var existingRole = await this.roleManager.FindByNameAsync(role);
        if (existingRole == null)
        {
            return new ServiceResult(
                ServiceResultType.NotFound,
                "No role exists to assign to user");
        }

        var roleAssignmentResult = await this.userManager.AddToRoleAsync(user, role);
        if (!roleAssignmentResult.Succeeded)
        {
            return new ServiceResult<AppUser>(
                ServiceResultType.InternalError,
                IdentityUtilities.ConcatenateIdentityErrorMessages(roleAssignmentResult.Errors));
        }

        return new ServiceResult(ServiceResultType.Success);
    }

    /// <summary>
    /// Processes user email confirmation.
    /// </summary>
    /// <param name="user">
    /// <see cref="AppUser"/>.
    /// </param>
    /// <param name="shouldConfirmImmediately">
    /// Indicates whether newly created user email should be confirmed without email sending.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation with <see cref="ServiceResult"/>.
    /// </returns>.
    private async Task<ServiceResult> ProcessUserEmailConfirmation(AppUser user, bool shouldConfirmImmediately)
    {
        var confirmationToken = await this.GenerateConfirmationToken(user);

        if (shouldConfirmImmediately)
        {
            var confirmationEmailResult = await this.ConfirmUserEmail(user, confirmationToken);
            if (confirmationEmailResult.IsResultFailed)
            {
                return confirmationEmailResult.GenerateErrorResult();
            }
        }
        else
        {
            this.SendConfirmationEmail(user.Email, confirmationToken);
        }

        return new ServiceResult(ServiceResultType.Success);
    }

    private async Task<ServiceResult<string>> ConfirmUserEmail(AppUser user, string confirmationToken)
    {
        var emailConfirmationResult = await this.userManager.ConfirmEmailAsync(user, confirmationToken);
        if (!emailConfirmationResult.Succeeded)
        {
            return new ServiceResult<string>(
                ServiceResultType.InternalError,
                IdentityUtilities.ConcatenateIdentityErrorMessages(emailConfirmationResult.Errors));
        }

        return new ServiceResult<string>(ServiceResultType.Success, confirmationToken);
    }

    private async Task<string> GenerateConfirmationToken(AppUser user) =>
        await this.userManager.GenerateEmailConfirmationTokenAsync(user);

    private void SendConfirmationEmail(string email, string confirmationToken)
    {
        Task
            .Run(() => this.HandleUserEmailConfirmationSending(email, confirmationToken))
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    private void HandleUserEmailConfirmationSending(string email, string confirmationToken)
    {
        var confirmationLink = this.httpContextService.GenerateConfirmEmailLink(email, confirmationToken);

        var emailTemplate =
            this.databaseContext.EmailTemplates.Single(template =>
                template.Id == EntityConfigurationConstants.EmailConfirmationTemplateId);

        var emailLayout = GenerateEmailLayout(emailTemplate.Layout, confirmationLink);

        this.emailService.SendEmailAsync(email, emailTemplate.Subject, emailLayout);
    }

    private static string GenerateEmailLayout(string predefinedEmailLayout, string confirmationLink)
    {
        var template = Handlebars.Compile(predefinedEmailLayout);
        var templateData = new
        {
            link = confirmationLink,
        };

        return template(templateData);
    }
}