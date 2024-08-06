using DY.Auth.Identity.Api.ApplicationLogic.Services.Common;

using FluentValidation;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.User.Queries.GetUserById;

/// <summary>
/// <see cref="GetUserByIdQuery"/> validator.
/// </summary>
public class GetUserByIdQueryHandlerValidator : AbstractValidator<GetUserByIdQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserByIdQueryHandlerValidator"/> class.
    /// </summary>
    public GetUserByIdQueryHandlerValidator()
    {
        this.Include(new BaseIdValidator());
    }
}
