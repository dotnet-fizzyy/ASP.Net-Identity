using System;
using System.Net.Http;
using System.Web;

namespace DY.Auth.Identity.Api.Core.Utilities;

/// <summary>
/// HTTP utilities.
/// </summary>
public static class HttpUtilities
{
    /// <summary>
    /// Adds query parameter with given key and value for <see cref="HttpRequestMessage"/>.
    /// </summary>
    /// <param name="httpRequestMessage">Given instance of <see cref="HttpRequestMessage"/> to add query parameter.</param>
    /// <param name="key">Given query parameter key to set.</param>
    /// <param name="value">Given query parameter value to set.</param>
    public static void AddQueryParameter(this HttpRequestMessage httpRequestMessage, string key, string value)
    {
        ArgumentNullException.ThrowIfNull(httpRequestMessage);

        var separator = httpRequestMessage.RequestUri!.OriginalString.Contains('?') ? '&' : '?';

        var query = HttpUtility.ParseQueryString(string.Empty);
        query.Add(key, value);

        httpRequestMessage.RequestUri = new Uri($"{httpRequestMessage.RequestUri}{separator}{query}", UriKind.Relative);
    }
}
