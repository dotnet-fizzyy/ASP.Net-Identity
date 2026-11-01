using System;

namespace DY.Auth.Identity.Api.Core.Interfaces.Presentation;

/// <summary>
/// Controller link generation service.
/// </summary>
public interface IControllersLinkService
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
    /// <returns>Link to get user profile action.</returns>
    string GenerateGetUserLink(Guid id);

    /// <summary>
    /// Generates link to get role action.
    /// </summary>
    /// <param name="id">Role identifier.</param>
    /// <returns>Link to get role action.</returns>
    string GenerateGetRoleLink(Guid id);

    /// <summary>
    /// Generates link to get email template action.
    /// </summary>
    /// <param name="id">Email template identifier.</param>
    /// <returns>Link to get email template action.</returns>
    string GenerateEmailTemplateLink(Guid id);
}
