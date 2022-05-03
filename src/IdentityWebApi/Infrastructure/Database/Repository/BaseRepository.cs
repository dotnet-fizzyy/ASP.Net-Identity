using IdentityWebApi.Core.Interfaces.Infrastructure;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IdentityWebApi.Infrastructure.Database.Repository;

public abstract class BaseRepository<T> : IBaseRepository<T>
    where T : class
{
    protected readonly DatabaseContext DatabaseContext;
    private readonly DbSet<T> entity;

    protected BaseRepository(DatabaseContext databaseContext)
    {
        this.DatabaseContext = databaseContext;
        this.entity = databaseContext.Set<T>();
    }

    public async Task<T> SearchForSingleItemAsync(Expression<Func<T, bool>> expression) =>
        await this.entity.AsNoTracking().SingleOrDefaultAsync(expression);
}