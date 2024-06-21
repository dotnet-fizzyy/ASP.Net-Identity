using System.Collections.Generic;

namespace IdentityWebApi.Core.Utilities;

/// <summary>
/// Application extensions.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Checks whether <see cref="IEnumerable{T}" /> collection is null or empty.
    /// </summary>
    /// <param name="collection">Instance of <see cref="IEnumerable{T}"/>.</param>
    /// <typeparam name="T">Collection type.</typeparam>
    /// <returns>Boolean value indicating whether collection is null or empty.</returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
    {
        if (collection is null)
        {
            return true;
        }

        using var enumerator = collection.GetEnumerator();

        return !enumerator.MoveNext();
    }

    /// <summary>
    /// Checks whether <see cref="ICollection{T}" /> collection is null or empty.
    /// </summary>
    /// <param name="collection">Instance of <see cref="ICollection{T}"/>.</param>
    /// <typeparam name="T">Collection type.</typeparam>
    /// <returns>Boolean value indicating whether <see cref="ICollection{T}" /> is null or empty.</returns>
    public static bool IsNullOrEmpty<T>(this ICollection<T> collection) =>
        collection == null || collection.Count == 0;

    /// <summary>
    /// Checks whether <see cref="ISet{T}" /> collection is null or empty.
    /// </summary>
    /// <param name="collection">Instance of <see cref="ISet{T}"/>.</param>
    /// <typeparam name="T">Collection type.</typeparam>
    /// <returns>Boolean value indicating whether <see cref="ISet{T}" /> is null or empty.</returns>
    public static bool IsNullOrEmpty<T>(this ISet<T> collection) =>
        collection == null || collection.Count == 0;
}
