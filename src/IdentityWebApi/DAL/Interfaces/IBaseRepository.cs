using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IdentityWebApi.DAL.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T> SearchForSingleItemAsync(Expression<Func<T, bool>> expression);
}