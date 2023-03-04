using System.Collections.Generic;

namespace IdentityWebApi.Core.Utilities;

/// <summary>
/// Application extensions.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Checks whether collection is null or empty.
    /// </summary>
    /// <param name="collection">Instance of <see cref="IEnumerable{T}"/>.</param>
    /// <typeparam name="T">Collection type.</typeparam>
    /// <returns>
    /// Boolean value indicating whether collection is null or empty.
    /// </returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) =>
        collection == null || !collection.GetEnumerator().MoveNext();
}