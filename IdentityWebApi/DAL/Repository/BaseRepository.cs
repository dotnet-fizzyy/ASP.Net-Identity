using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IdentityWebApi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityWebApi.DAL.Repository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T: class
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

        public async Task<T> SearchForSingleItemAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                var query = _entity.Where(expression).AsNoTracking();

                if (includes.Length != 0)
                {
                    query = includes
                        .Aggregate(query,
                            (
                                current, includeProperty) => current.Include(includeProperty)
                        );
                }

                var item = await query.SingleOrDefaultAsync();

                return item;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex);
                throw new InvalidOperationException($"More then one item has been found. Error: {ex.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception($"Unable to find item in database. Error: {e.Message}");
            }
        }

        public virtual async Task<T> CreateItemAsync(T entity)
        {
            var createdEntity = await DatabaseContext.AddAsync(entity);

            await DatabaseContext.SaveChangesAsync();

            createdEntity.State = EntityState.Detached;
            
            return createdEntity.Entity;
        }
    }
}