using System;

namespace IdentityWebApi.Core.Interfaces.Presentation;

/// <summary>
/// HTTP Context service.
/// </summary>
public interface IHttpContextService
{
    /// <summary>
    /// Generates link to confirm email on registration process.
    /// </summary>
    /// <param name="email">Email to confirm.</param>
    /// <param name="token">Confirmation token.</param>
    /// <returns>Link to confirm email on registration process.</returns>
    string GenerateConfirmEmailLink(string email, string token);

    /// <summary>
    /// Generates link to get user profile action.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>Link to to get user profile action.</returns>
    string GenerateGetUserLink(Guid id);
}