using IdentityWebApi.Core.Interfaces.Infrastructure;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IdentityWebApi.Infrastructure.Database.Repository;

/// <summary>
/// <inheritdoc cref="IBaseRepository{T}"/>
/// </summary>
/// <typeparam name="T">Entity.</typeparam>
public abstract class BaseRepository<T> : IBaseRepository<T>
    where T : class
{
    /// <summary>
    /// Database EF context.
    /// </summary>
    protected readonly DatabaseContext DatabaseContext;
    private readonly DbSet<T> entity;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class.
    /// </summary>
    /// <param name="databaseContext"><see cref="DatabaseContext"/>.</param>
    protected BaseRepository(DatabaseContext databaseContext)
    {
        this.DatabaseContext = databaseContext;
        this.entity = databaseContext.Set<T>();
    }

    /// <inheritdoc/>
    public async Task<T> SearchForSingleItemAsync(Expression<Func<T, bool>> expression) =>
        await this.entity.AsNoTracking().SingleOrDefaultAsync(expression);
}