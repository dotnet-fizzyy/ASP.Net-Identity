using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.ApplicationSettings;
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
    private readonly IEmailService emailService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="userManager"><see cref="UserManager{T}"/>.</param>
    /// <param name="roleManager"><see cref="RoleManager{T}"/>.</param>
    /// <param name="databaseContext"><see cref="DatabaseContext"/>.</param>
    /// <param name="appSettings"><see cref="AppSettings"/>.</param>
    /// <param name="emailService"><see cref="IEmailService"/>.</param>
    /// <param name="mapper"><see cref="IMapper"/>.</param>
    public CreateUserCommandHandler(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        DatabaseContext databaseContext,
        AppSettings appSettings,
        IEmailService emailService,
        IMapper mapper)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.databaseContext = databaseContext;
        this.appSettings = appSettings;
        this.emailService = emailService;
        this.mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<UserDto>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var userEntity = this.mapper.Map<AppUser>(command.User);

        var userCreationResult = await this.CreateUser(userEntity, command.User.Password);
        if (userCreationResult.IsResultFailed)
        {
            return userCreationResult.GenerateErrorResult<UserDto>();
        }

        var createdUser = userCreationResult.Data;

        if (!string.IsNullOrEmpty(command.User.UserRole))
        {
            var roleAssignmentResult = await this.AssignRoleToUser(createdUser, command.User.UserRole);
            if (roleAssignmentResult.IsResultFailed)
            {
                return roleAssignmentResult.GenerateErrorResult<UserDto>();
            }
        }

        if (this.appSettings.IdentitySettings.Email.RequireConfirmation)
        {
            var confirmationToken = await this.GenerateConfirmationToken(createdUser);

            if (command.ConfirmEmailImmediately)
            {
                var confirmationEmailResult = await this.ConfirmUserEmail(createdUser, confirmationToken);
                if (confirmationEmailResult.IsResultFailed)
                {
                    return confirmationEmailResult.GenerateErrorResult<UserDto>();
                }
            }
            else
            {
                this.SendUserEmailConfirmation(createdUser.Email, confirmationToken);
            }
        }

        var userDto = this.mapper.Map<UserDto>(createdUser);

        return new ServiceResult<UserDto>(ServiceResultType.Success, userDto);
    }

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

    private async Task<ServiceResult<string>> ConfirmUserEmail(AppUser user, string confirmationToken)
    {
        var emailConfirmationResult = await this.userManager.ConfirmEmailAsync(user, confirmationToken);
        if (!emailConfirmationResult.Succeeded)
        {
            return new ServiceResult<string>(
                ServiceResultType.InternalError,
                IdentityUtilities.ConcatenateIdentityErrorMessages(emailConfirmationResult.Errors)
            );
        }

        return new ServiceResult<string>(ServiceResultType.Success, confirmationToken);
    }

    private async Task<string> GenerateConfirmationToken(AppUser user) =>
        await this.userManager.GenerateEmailConfirmationTokenAsync(user);

    private void SendUserEmailConfirmation(string email, string confirmationToken)
    {
        // todo: process handlebars template cast
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