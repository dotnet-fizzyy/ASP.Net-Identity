using System.Threading.Tasks;

namespace IdentityWebApi.DAL.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }

    IRoleRepository RoleRepository { get; }

    IEmailTemplateRepository EmailTemplateRepository { get; }

    Task CommitAsync();
}
