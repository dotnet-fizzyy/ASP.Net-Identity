using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.Infrastructure;

/// <summary>
/// Repository with base methods for entity operations.
/// </summary>
/// <typeparam name="T">Entity.</typeparam>
public interface IBaseRepository<T>
    where T : class
{
    /// <summary>
    /// Searching for single entity with criteria.
    /// </summary>
    /// <param name="expression">Expression with searching criteria.</param>
    /// <returns>Entity found by search criteria.</returns>
    Task<T> SearchForSingleItemAsync(Expression<Func<T, bool>> expression);
}