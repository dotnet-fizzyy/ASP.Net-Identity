using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.Infrastructure;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }

    IRoleRepository RoleRepository { get; }

    IEmailTemplateRepository EmailTemplateRepository { get; }

    Task CommitAsync();
}
