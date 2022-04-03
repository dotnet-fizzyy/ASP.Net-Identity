using IdentityWebApi.DAL.Interfaces;

using System;
using System.Threading.Tasks;


namespace IdentityWebApi.DAL;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly DatabaseContext _databaseContext;

    public IUserRepository UserRepository { get; }

    public IRoleRepository RoleRepository { get; }

    public IEmailTemplateRepository EmailTemplateRepository { get; }

    public UnitOfWork(
        DatabaseContext databaseContext,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IEmailTemplateRepository emailTemplateRepository
    )
    {
        _databaseContext = databaseContext;

        UserRepository = userRepository;
        RoleRepository = roleRepository;
        EmailTemplateRepository = emailTemplateRepository;
    }

    public async Task CommitAsync()
    {
        await _databaseContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _databaseContext.Dispose();
    }
}
