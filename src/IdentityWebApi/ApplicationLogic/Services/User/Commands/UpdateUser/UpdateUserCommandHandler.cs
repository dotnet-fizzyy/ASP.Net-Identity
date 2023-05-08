using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Output;
using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Results;
using IdentityWebApi.Infrastructure.Database;

using MediatR;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services.User.Commands.UpdateUser;

/// <summary>
/// Update user CQRS handler.
/// </summary>
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ServiceResult<UserResult>>
{
    private readonly DatabaseContext databaseContext;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"><see cref="DatabaseContext"/>.</param>
    /// <param name="mapper"><see cref="IMapper"/>.</param>
    public UpdateUserCommandHandler(DatabaseContext databaseContext, IMapper mapper)
    {
        this.databaseContext = databaseContext;
        this.mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<UserResult>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var existingAppUser = await this.GetUser(command.UserDto.Id);
        if (existingAppUser == null)
        {
            return new ServiceResult<UserResult>(ServiceResultType.NotFound);
        }

        UpdateUserDetails(existingAppUser, command.UserDto);

        await this.databaseContext.SaveChangesAsync();

        var updatedUserDto = this.mapper.Map<AppUser, UserResult>(existingAppUser);

        return new ServiceResult<UserResult>(ServiceResultType.Success, updatedUserDto);
    }

    private async Task<AppUser> GetUser(Guid id) =>
        await this.databaseContext.SearchById<AppUser>(id);

    private static void UpdateUserDetails(AppUser appUser, Models.Action.UserDto userDto)
    {
        appUser.UserName = userDto.UserName;
        appUser.PhoneNumber = userDto.PhoneNumber;
    }
}
