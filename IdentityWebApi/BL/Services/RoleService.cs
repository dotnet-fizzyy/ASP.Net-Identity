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
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult> GrantRoleToUserAsync(UserRoleActionModel roleActionModel) =>
            await _roleRepository.GrantRoleToUserAsync(roleActionModel.UserId, roleActionModel.RoleId);

        public async Task<ServiceResult> RevokeRoleFromUser(UserRoleActionModel roleActionModel) =>
            await _roleRepository.RevokeRoleFromUserAsync(roleActionModel.UserId, roleActionModel.RoleId);

        public async Task<ServiceResult<RoleDto>> CreateRoleAsync(RoleDto roleDto) => 
            await HandleAppRoleEntity(_roleRepository.CreateRoleAsync, roleDto);

        public async Task<ServiceResult<RoleDto>> UpdateRoleAsync(RoleDto roleDto) => 
            await HandleAppRoleEntity(_roleRepository.UpdateRoleAsync, roleDto);

        public async Task<ServiceResult> RemoveRoleAsync(Guid id) =>
            await _roleRepository.RemoveRoleAsync(id);


        private async Task<ServiceResult<RoleDto>> HandleAppRoleEntity(Func<AppRole, Task<ServiceResult<AppRole>>> repositoryCall, RoleDto roleDto)
        {
            var roleEntity = _mapper.Map<AppRole>(roleDto);
            
            var roleCreationResult = await repositoryCall(roleEntity);

            var roleModel = roleCreationResult.Data is not null 
                ? _mapper.Map<RoleDto>(roleCreationResult.Data) 
                : default;
            
            return new ServiceResult<RoleDto>(roleCreationResult.Result, roleModel);
        }
    }
}