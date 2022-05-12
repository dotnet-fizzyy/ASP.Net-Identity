using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Interfaces.ApplicationLogic;
using IdentityWebApi.Core.Interfaces.Infrastructure;
using IdentityWebApi.Core.Results;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services;

/// <inheritdoc cref="IUserService" />
public class UserService : IUserService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="unitOfWork"><see cref="IUnitOfWork"/>.</param>
    /// <param name="mapper"><see cref="IMapper"/>.</param>
    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<UserResultDto>> GetUserAsync(Guid id)
    {
        var searchUserResult = await this.unitOfWork.UserRepository.GetUserWithRoles(id);

        var userDtoModel = searchUserResult.Data is not null
            ? this.mapper.Map<UserResultDto>(searchUserResult.Data)
            : default;

        return new ServiceResult<UserResultDto>(
            searchUserResult.Result,
            searchUserResult.Message,
            userDtoModel
        );
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<UserResultDto>> CreateUserAsync(UserDto user)
    {
        var userEntity = this.mapper.Map<AppUser>(user);

        var createdUserResult = await this.unitOfWork.UserRepository.CreateUserAsync(userEntity, user.Password, user.UserRole, true);

        var userDtoModel = createdUserResult.Data.appUser is not null
            ? this.mapper.Map<UserResultDto>(createdUserResult.Data.appUser)
            : default;

        return new ServiceResult<UserResultDto>(
            createdUserResult.Result,
            createdUserResult.Message,
            userDtoModel
        );
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<UserResultDto>> UpdateUserAsync(UserDto user)
    {
        var userEntity = this.mapper.Map<AppUser>(user);

        var updatedUserResult = await this.unitOfWork.UserRepository.UpdateUserAsync(userEntity);

        var userDtoModel = updatedUserResult.Data is not null
            ? this.mapper.Map<UserResultDto>(updatedUserResult.Data)
            : default;

        return new ServiceResult<UserResultDto>(
            updatedUserResult.Result,
            updatedUserResult.Message,
            userDtoModel
        );
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> RemoveUserAsync(Guid id) =>
        await this.unitOfWork.UserRepository.RemoveUserAsync(id);
}