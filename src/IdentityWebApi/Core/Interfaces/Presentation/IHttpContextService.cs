using System;

namespace IdentityWebApi.Core.Interfaces.Presentation;

public interface IHttpContextService
{
    string GenerateConfirmEmailLink(string email, string token);

    string GenerateGetUserLink(Guid id);
}