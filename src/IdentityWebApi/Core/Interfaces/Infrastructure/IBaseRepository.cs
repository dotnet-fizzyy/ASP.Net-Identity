using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.Infrastructure;

public interface IBaseRepository<T> where T : class
{
    Task<T> SearchForSingleItemAsync(Expression<Func<T, bool>> expression);
}