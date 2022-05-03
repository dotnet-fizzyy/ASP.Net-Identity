using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.Infrastructure;

public interface IUnitOfWork
{
    /// <summary>
    /// Gets user repository abstraction.
    /// </summary>
    IUserRepository UserRepository { get; }

    IRoleRepository RoleRepository { get; }

    IEmailTemplateRepository EmailTemplateRepository { get; }

    Task CommitAsync();
}
