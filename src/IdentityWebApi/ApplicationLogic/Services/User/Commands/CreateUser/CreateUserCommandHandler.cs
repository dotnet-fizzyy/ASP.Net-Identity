using AutoMapper;

using HandlebarsDotNet;

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

using System;
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
    /// <param name="userManager">Instance of <see cref="UserManager{T}"/>.</param>
    /// <param name="roleManager">Instance of <see cref="RoleManager{T}"/>.</param>
    /// <param name="databaseContext">Instance of <see cref="DatabaseContext"/>.</param>
    /// <param name="appSettings">Instance of <see cref="AppSettings"/>.</param>
    /// <param name="emailService">Instance of <see cref="IEmailService"/>.</param>
    /// <param name="httpContextService">Instance of <see cref="IHttpContextService"/>.</param>
    /// <param name="mapper">Instance of <see cref="IMapper"/>.</param>
    public CreateUserCommandHandler(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        DatabaseContext databaseContext,
        AppSettings appSettings,
        IEmailService emailService,
        IHttpContextService httpContextService,
        IMapper mapper)
    {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
        this.appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        this.httpContextService = httpContextService ?? throw new ArgumentNullException(nameof(httpContextService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<Models.Action.UserDto>> Handle(
        CreateUserCommand command, CancellationToken cancellationToken)
    {
        var userCreationResult = await this.CreateUserAsync(command);

        if (userCreationResult.IsResultFailed)
        {
            return GenerateHandlerErrorResult(userCreationResult);
        }

        var createdUser = userCreationResult.Data;

        if (!string.IsNullOrEmpty(command.UserRole))
        {
            var roleAssignmentResult =
                  await this.AssignRoleAsync(createdUser, command.UserRole);

            if (roleAssignmentResult.IsResultFailed)
            {
                return GenerateHandlerErrorResult(roleAssignmentResult);
            }
        }

        if (this.appSettings.IdentitySettings.Email.RequireConfirmation)
        {
            var confirmationEmailResult =
                  await this.ProcessUserEmailConfirmationAsync(createdUser, command.ConfirmEmailImmediately);

            if (confirmationEmailResult.IsResultFailed)
            {
                return GenerateHandlerErrorResult(confirmationEmailResult);
            }
        }

        var userDto = this.mapper.Map<Models.Action.UserDto>(createdUser);

        return new ServiceResult<Models.Action.UserDto>(ServiceResultType.Success, userDto);
    }

    private async Task<ServiceResult<AppUser>> CreateUserAsync(CreateUserCommand command)
    {
        var userEntity = this.mapper.Map<AppUser>(command);

        var userCreationResult = await this.userManager.CreateAsync(userEntity, command.Password);

        if (!userCreationResult.Succeeded)
        {
            return new ServiceResult<AppUser>(
                ServiceResultType.InternalError,
                IdentityUtilities.ConcatenateIdentityErrorMessages(userCreationResult.Errors));
        }

        return new ServiceResult<AppUser>(ServiceResultType.Success, userEntity);
    }

    private async Task<ServiceResult> AssignRoleAsync(AppUser user, string role)
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

    private async Task<ServiceResult> ProcessUserEmailConfirmationAsync(AppUser user, bool shouldConfirmImmediately)
    {
        var confirmationToken = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

        if (shouldConfirmImmediately)
        {
            var confirmationEmailResult = await this.ConfirmUserEmail(user, confirmationToken);

            if (confirmationEmailResult.IsResultFailed)
            {
                return confirmationEmailResult;
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

    private void SendConfirmationEmail(string email, string confirmationToken)
    {
        Task.Run(() => this.HandleUserEmailConfirmationSending(email, confirmationToken));
    }

    private void HandleUserEmailConfirmationSending(string email, string confirmationToken)
    {
        var confirmationLink = this.httpContextService.GenerateConfirmEmailLink(email, confirmationToken);

        var emailTemplate =
              this.databaseContext.EmailTemplates.Single(
                template => template.Id == EntityConfigurationConstants.EmailConfirmationTemplateId);

        var emailLayout = GenerateEmailLayout(emailTemplate.Layout, confirmationLink);

        this.emailService.SendEmailAsync(email, emailTemplate.Subject, emailLayout);
    }

    private static ServiceResult<Models.Action.UserDto> GenerateHandlerErrorResult(ServiceResult serviceResult) =>
        serviceResult.GenerateErrorResult<Models.Action.UserDto>();

    private static string GenerateEmailLayout(string predefinedEmailLayout, string confirmationLink)
    {
        var template = Handlebars.Compile(predefinedEmailLayout);
        var templateData = new { link = confirmationLink };

        return template(templateData);
    }
}
