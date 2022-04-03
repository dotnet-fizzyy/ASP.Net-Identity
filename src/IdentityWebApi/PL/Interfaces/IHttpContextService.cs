using System;

namespace IdentityWebApi.PL.Interfaces;

public interface IHttpContextService
{
    string GenerateConfirmEmailLink(string email, string token);

    string GenerateGetUserLink(Guid id);
}