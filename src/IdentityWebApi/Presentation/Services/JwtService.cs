using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace IdentityWebApi.Presentation.Services;

/// <summary>
/// JWT service.
/// </summary>
public static class JwtService
{
    /// <summary>
    /// Creates encoded security signing key.
    /// </summary>
    /// <param name="key">Original token signing key.</param>
    /// <returns><see cref="SymmetricSecurityKey"/> encoded key.</returns>
    public static SymmetricSecurityKey CreateSecuritySigningKey(string key) =>
        new (Encoding.UTF8.GetBytes(key));

    /// <summary>
    /// Generates JWT token.
    /// </summary>
    /// <param name="signingKey">Original token signing key.</param>
    /// <param name="issuer">Token issuer.</param>
    /// <param name="audience">Token target audience.</param>
    /// <param name="expiration">Token expiration time.</param>
    /// <param name="claims">User claims.</param>
    /// <returns>JWT token.</returns>
    public static string GenerateJwtToken(
        string signingKey,
        string issuer,
        string audience,
        TimeSpan expiration,
        IEnumerable<Claim> claims)
    {
        var key = CreateSecuritySigningKey(signingKey);
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            expires: DateTime.UtcNow.Add(expiration),
            claims: claims,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
}