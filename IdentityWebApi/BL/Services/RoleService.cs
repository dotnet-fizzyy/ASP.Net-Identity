using System;
using System.Threading.Tasks;
using AutoMapper;
using IdentityWebApi.BL.Interfaces;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.DAL.Entities;
using IdentityWebApi.DAL.Interfaces;
using IdentityWebApi.PL.Models.Action;
using IdentityWebApi.PL.Models.DTO;

namespace IdentityWebApi.BL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult<RoleDto>> GetRoleByIdAsync(Guid id)
        {
            var roleEntityResult = await _unitOfWork.RoleRepository.GetRoleByIdAsync(id);
            
            var roleModel = roleEntityResult.Data is not null 
                ? _mapper.Map<RoleDto>(roleEntityResult.Data) 
                : default;
            
            return new ServiceResult<RoleDto>(roleEntityResult.Result, roleModel);
        }

        public async Task<ServiceResult> GrantRoleToUserAsync(UserRoleActionModel roleActionModel) =>
            await _unitOfWork.RoleRepository.GrantRoleToUserAsync(roleActionModel.UserId, roleActionModel.RoleId);

        public async Task<ServiceResult> RevokeRoleFromUser(UserRoleActionModel roleActionModel) =>
            await _unitOfWork.RoleRepository.RevokeRoleFromUserAsync(roleActionModel.UserId, roleActionModel.RoleId);

        public async Task<ServiceResult<RoleDto>> CreateRoleAsync(RoleCreationActionModel roleDto)
        {
            var roleEntity = _mapper.Map<AppRole>(roleDto);
            
            return await HandleAppRoleEntity(_unitOfWork.RoleRepository.CreateRoleAsync, roleEntity);
        }

        public async Task<ServiceResult<RoleDto>> UpdateRoleAsync(RoleDto roleDto)
        {
            var roleEntity = _mapper.Map<AppRole>(roleDto);
            
            return await HandleAppRoleEntity(_unitOfWork.RoleRepository.UpdateRoleAsync, roleEntity);
        }

        public async Task<ServiceResult> RemoveRoleAsync(Guid id) =>
            await _unitOfWork.RoleRepository.RemoveRoleAsync(id);


        private async Task<ServiceResult<RoleDto>> HandleAppRoleEntity(Func<AppRole, Task<ServiceResult<AppRole>>> repositoryCall, AppRole roleEntity)
        {
            var roleCreationResult = await repositoryCall(roleEntity);

            var roleModel = roleCreationResult.Data is not null 
                ? _mapper.Map<RoleDto>(roleCreationResult.Data) 
                : default;
            
            return new ServiceResult<RoleDto>(roleCreationResult.Result, roleModel);
        }
    }
}