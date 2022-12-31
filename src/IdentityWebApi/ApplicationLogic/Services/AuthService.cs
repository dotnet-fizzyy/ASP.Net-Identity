using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.ApplicationLogic.Models.Output;
using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Interfaces.ApplicationLogic;
using IdentityWebApi.Core.Interfaces.Infrastructure;
using IdentityWebApi.Core.Results;

using System.Threading.Tasks;

namespace IdentityWebApi.ApplicationLogic.Services;

/// <inheritdoc cref="IAuthService" />
public class AuthService : IAuthService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthService"/> class.
    /// </summary>
    /// <param name="unitOfWork"><see cref="IUnitOfWork"/>.</param>
    /// <param name="mapper"><see cref="IMapper"/>.</param>
    public AuthService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<(UserResult userDto, string token)>> SignUpUserAsync(
        UserRegistrationDto userModel)
    {
        var userEntity = this.mapper.Map<AppUser>(userModel);

        var createdResult =
            await this.unitOfWork.UserRepository.CreateUserAsync(userEntity, userModel.Password, userModel.Role, false);

        var userDtoModel = createdResult.Data.appUser is not null
            ? this.mapper.Map<UserResult>(createdResult.Data.appUser)
            : default;

        return new ServiceResult<(UserResult userDto, string token)>(
            createdResult.Result,
            createdResult.Message,
            (userDtoModel, createdResult.Data.token));
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<UserResult>> SignInUserAsync(UserSignInDto userModel)
    {
        var signInResult = await this.unitOfWork.UserRepository.SignInUserAsync(userModel.Email, userModel.Password);

        var userDtoModel = signInResult.Data is not null
            ? this.mapper.Map<UserResult>(signInResult.Data)
            : default;

        return new ServiceResult<UserResult>(
            signInResult.Result,
            signInResult.Message,
            userDtoModel);
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> ConfirmUserEmailAsync(string email, string token)
    {
        var emailConfirmationResult = await this.unitOfWork.UserRepository.ConfirmUserEmailAsync(email, token);

        return emailConfirmationResult;
    }
}
