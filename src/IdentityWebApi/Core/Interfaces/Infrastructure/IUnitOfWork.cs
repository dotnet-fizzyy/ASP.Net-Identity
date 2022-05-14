using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.Infrastructure;

/// <summary>
/// Unit of work pattern.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Gets user repository abstraction.
    /// </summary>
    IUserRepository UserRepository { get; }

    /// <summary>
    /// Gets role repository abstraction.
    /// </summary>
    IRoleRepository RoleRepository { get; }

    /// <summary>
    /// Gets email template repository abstraction.
    /// </summary>
    IEmailTemplateRepository EmailTemplateRepository { get; }

    /// <summary>
    /// Commits DB changes.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    Task CommitAsync();
}
