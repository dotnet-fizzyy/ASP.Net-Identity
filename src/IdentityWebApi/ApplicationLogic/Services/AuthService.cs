using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Interfaces.ApplicationLogic;
using IdentityWebApi.Core.Interfaces.Infrastructure;
using IdentityWebApi.Core.Results;

using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AuthService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceResult<(UserResultDto userDto, string token)>> SignUpUserAsync(
        UserRegistrationDto userModel)
    {
        var userEntity = _mapper.Map<AppUser>(userModel);

        var createdResult =
            await _unitOfWork.UserRepository.CreateUserAsync(userEntity, userModel.Password, userModel.Role, false);

        var userDtoModel = createdResult.Data.appUser is not null
            ? _mapper.Map<UserResultDto>(createdResult.Data.appUser)
            : default;

        return new ServiceResult<(UserResultDto userDto, string token)>(
            createdResult.Result,
            createdResult.Message,
            (userDtoModel, createdResult.Data.token)
        );
    }

    public async Task<ServiceResult<UserResultDto>> SignInUserAsync(UserSignInDto userModel)
    {
        var signInResult = await _unitOfWork.UserRepository.SignInUserAsync(userModel.Email, userModel.Password);

        var userDtoModel = signInResult.Data is not null
            ? _mapper.Map<UserResultDto>(signInResult.Data)
            : default;

        return new ServiceResult<UserResultDto>(
            signInResult.Result,
            signInResult.Message,
            userDtoModel
        );
    }

    public async Task<ServiceResult> ConfirmUserEmailAsync(string email, string token)
    {
        var emailConfirmationResult = await _unitOfWork.UserRepository.ConfirmUserEmailAsync(email, token);

        return emailConfirmationResult;
    }
}
