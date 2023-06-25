using System;
using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.Infrastructure;

/// <summary>
/// Unit of work pattern.
/// </summary>
[Obsolete("Remove after CQRS pattern full implementation")]
public interface IUnitOfWork
{
    /// <summary>
    /// Gets role repository abstraction.
    /// </summary>
    IRoleRepository RoleRepository { get; }

    /// <summary>
    /// Commits DB changes.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    Task CommitAsync();
}
