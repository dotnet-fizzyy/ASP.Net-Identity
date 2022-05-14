using IdentityWebApi.Core.Interfaces.Infrastructure;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.Infrastructure.Database;

/// <summary>
/// <inheritdoc cref="IUnitOfWork"/>
/// </summary>
public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly DatabaseContext databaseContext;

    /// <inheritdoc/>
    public IUserRepository UserRepository { get; }

    /// <inheritdoc/>
    public IRoleRepository RoleRepository { get; }

    /// <inheritdoc/>
    public IEmailTemplateRepository EmailTemplateRepository { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="databaseContext"><see cref="DatabaseContext"/>.</param>
    /// <param name="userRepository"><see cref="IUserRepository"/>.</param>
    /// <param name="roleRepository"><see cref="IRoleRepository"/>.</param>
    /// <param name="emailTemplateRepository"><see cref="IEmailTemplateRepository"/>.</param>
    public UnitOfWork(
        DatabaseContext databaseContext,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IEmailTemplateRepository emailTemplateRepository
    )
    {
        this.databaseContext = databaseContext;

        this.UserRepository = userRepository;
        this.RoleRepository = roleRepository;
        this.EmailTemplateRepository = emailTemplateRepository;
    }

    /// <inheritdoc/>
    public async Task CommitAsync()
    {
        await this.databaseContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.databaseContext.Dispose();
    }
}
