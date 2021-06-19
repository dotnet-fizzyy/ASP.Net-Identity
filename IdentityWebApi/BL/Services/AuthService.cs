using System;
using System.Threading.Tasks;
using AutoMapper;
using IdentityWebApi.BL.Constants;
using IdentityWebApi.BL.Enums;
using IdentityWebApi.BL.Interfaces;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.DAL.Entities;
using IdentityWebApi.DAL.Interfaces;
using IdentityWebApi.PL.Models.Action;
using IdentityWebApi.PL.Models.DTO;
using IdentityWebApi.Startup.Settings;

namespace IdentityWebApi.BL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AuthService(IUserRepository userRepository, IMapper mapper, IEmailService emailService, AppSettings appSettings)
        {
            _emailService = emailService;
            _userRepository = userRepository;
            _mapper = mapper;
            _appSettings = appSettings;
        }

        public async Task<ServiceResult<UserDto>> SignUpUser(UserRegistrationActionModel userModel)
        {
            var userEntity = _mapper.Map<AppUser>(userModel);
            
            var createdResult = await _userRepository.CreateUserAsync(userEntity, userModel.Password, userModel.Role, false);
            if (createdResult.ServiceResultType is not ServiceResultType.Success)
            {
                return new ServiceResult<UserDto>(createdResult.ServiceResultType, createdResult.Message);
            }

            var confirmationLink = new Uri($"{_appSettings.Url}/api/auth/confirm-email?email={createdResult.Data.appUser.Email}&token={createdResult.Data.token}");
            await _emailService.SendEmailAsync(createdResult.Data.appUser.Email, EmailSubjects.AccountConfirmation, $"<a href={confirmationLink}>confirm</a>");
            
            return new ServiceResult<UserDto>(ServiceResultType.Success, _mapper.Map<UserDto>(createdResult.Data.appUser));
        }
    }
}