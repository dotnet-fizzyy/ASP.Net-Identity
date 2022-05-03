using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Interfaces.ApplicationLogic;
using IdentityWebApi.Core.Interfaces.Infrastructure;
using IdentityWebApi.Core.Results;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ServiceResult<RoleDto>> GetRoleByIdAsync(Guid id)
    {
        var roleEntityResult = await this.unitOfWork.RoleRepository.GetRoleByIdAsync(id);

        var roleModel = roleEntityResult.Data is not null
            ? this.mapper.Map<RoleDto>(roleEntityResult.Data)
            : default;

        return new ServiceResult<RoleDto>(roleEntityResult.Result, roleModel);
    }

    public async Task<ServiceResult> GrantRoleToUserAsync(UserRoleDto roleDto) =>
        await HandleAppRole(
            this.unitOfWork.RoleRepository.GrantRoleToUserAsync,
            roleDto.UserId,
            roleDto.RoleId
        );

    public async Task<ServiceResult> RevokeRoleFromUser(UserRoleDto roleDto) =>
        await HandleAppRole(
            this.unitOfWork.RoleRepository.RevokeRoleFromUserAsync,
            roleDto.UserId,
            roleDto.RoleId
        );

    public async Task<ServiceResult<RoleDto>> CreateRoleAsync(RoleCreationDto roleDto)
    {
        var roleEntity = this.mapper.Map<AppRole>(roleDto);

        return await HandleAppRole(this.unitOfWork.RoleRepository.CreateRoleAsync, roleEntity);
    }

    public async Task<ServiceResult<RoleDto>> UpdateRoleAsync(RoleDto roleDto)
    {
        var roleEntity = this.mapper.Map<AppRole>(roleDto);

        return await HandleAppRole(this.unitOfWork.RoleRepository.UpdateRoleAsync, roleEntity);
    }

    public async Task<ServiceResult> RemoveRoleAsync(Guid id)
    {
        var serviceResult = await this.unitOfWork.RoleRepository.RemoveRoleAsync(id);

        if (serviceResult.Result == ServiceResultType.Success)
        {
            await this.unitOfWork.CommitAsync();
        }

        return serviceResult;
    }


    private async Task<ServiceResult<RoleDto>> HandleAppRole(
        Func<AppRole, Task<ServiceResult<AppRole>>> repositoryCall,
        AppRole roleEntity
    )
    {
        var roleCreationResult = await repositoryCall(roleEntity);

        var roleModel = roleCreationResult.Data is not null
            ? this.mapper.Map<RoleDto>(roleCreationResult.Data)
            : default;

        await this.unitOfWork.CommitAsync();

        return new ServiceResult<RoleDto>(roleCreationResult.Result, roleModel);
    }

    private async Task<ServiceResult> HandleAppRole(
        Func<Guid, Guid, Task<ServiceResult>> repositoryCall,
        Guid userId,
        Guid roleId
    )
    {
        var serviceResult = await repositoryCall(userId, roleId);

        if (serviceResult.Result == ServiceResultType.Success)
        {
            await this.unitOfWork.CommitAsync();
        }

        return serviceResult;
    }
}
