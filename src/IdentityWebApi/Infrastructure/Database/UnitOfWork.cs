using IdentityWebApi.Core.Interfaces.Infrastructure;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.Infrastructure.Database;

/// <summary>
/// <inheritdoc cref="IUnitOfWork"/>
/// </summary>
[Obsolete("Replace with CQRS context")]
public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly DatabaseContext databaseContext;

    /// <inheritdoc/>
    public IUserRepository UserRepository { get; }

    /// <inheritdoc/>
    public IRoleRepository RoleRepository { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="databaseContext"><see cref="DatabaseContext"/>.</param>
    /// <param name="userRepository"><see cref="IUserRepository"/>.</param>
    /// <param name="roleRepository"><see cref="IRoleRepository"/>.</param>
    public UnitOfWork(
        DatabaseContext databaseContext,
        IUserRepository userRepository,
        IRoleRepository roleRepository
    )
    {
        this.databaseContext = databaseContext;

        this.UserRepository = userRepository;
        this.RoleRepository = roleRepository;
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
