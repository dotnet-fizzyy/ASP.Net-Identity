using System;

namespace IdentityWebApi.ApplicationLogic.Services.Common;

/// <summary>
/// Base command/query entity Id part.
/// </summary>
public interface IBaseId
{
    /// <summary>
    /// Gets command/query entity Id.
    /// </summary>
    public Guid Id { get; }
}
