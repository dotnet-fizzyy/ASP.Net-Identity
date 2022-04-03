using IdentityWebApi.Core.Interfaces.Infrastructure;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IdentityWebApi.Infrastructure.Repository;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly DatabaseContext DatabaseContext;
    private readonly DbSet<T> _entity;

    protected BaseRepository(DatabaseContext databaseContext)
    {
        DatabaseContext = databaseContext;
        _entity = databaseContext.Set<T>();
    }

    public async Task<T> SearchForSingleItemAsync(Expression<Func<T, bool>> expression)
    {
        var item = await _entity.AsNoTracking().SingleOrDefaultAsync(expression);

        return item;
    }
}