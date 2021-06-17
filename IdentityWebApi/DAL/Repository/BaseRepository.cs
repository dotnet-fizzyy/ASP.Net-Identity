using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IdentityWebApi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityWebApi.DAL.Repository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T: class
    {
        protected readonly DatabaseContext databaseContext;
        private readonly DbSet<T> _entity;
        
        protected BaseRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
            _entity = databaseContext.Set<T>();
        }
        
        public async Task<T> SearchForSingleItemAsync(Expression<Func<T, bool>> expression)
        {
            var item = await _entity.SingleOrDefaultAsync(expression);

            return item;
        }

        public virtual async Task<T> CreateItemAsync(T entity)
        {
            var createdEntity = await databaseContext.AddAsync(entity);

            await databaseContext.SaveChangesAsync();

            createdEntity.State = EntityState.Detached;
            
            return createdEntity.Entity;
        }
    }
}