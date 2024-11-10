using DY.Auth.Identity.Api.Core.Enums;

using Moq;
using Moq.Protected;

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.UnitTests.Shared.Mocks;

/// <summary>
/// <see cref="IHttpClientFactory"/> tests builder.
/// </summary>
public class HttpClientFactoryBuilder
{
    private readonly Mock<IHttpClientFactory> clientFactoryMock = new ();

    private readonly Mock<HttpMessageHandler> httpMessageHandlerMock = new (MockBehavior.Strict);

    /// <summary>
    /// Sets <see cref="InternalApi"/> mocked http client.
    /// </summary>
    /// <param name="internalApi">Given client to set.</param>
    /// <returns>Given instance of <see cref="HttpClientFactoryBuilder"/>.</returns>
    public HttpClientFactoryBuilder WithClient(InternalApi internalApi)
    {
        this.BuildHttpClient(internalApi.ToString());

        return this;
    }

    /// <summary>
    /// Sets JSON specific response with HTTP status code to mocked HTTP client.
    /// </summary>
    /// <param name="statusCode">Given response status code to set.</param>
    /// <param name="jsonContent">Given JSON content to set.</param>
    /// <returns>Given instance of <see cref="HttpClientFactoryBuilder"/>.</returns>
    public HttpClientFactoryBuilder WithResponse(HttpStatusCode statusCode, [StringSyntax(StringSyntaxAttribute.Json)] string jsonContent = "")
    {
        var stringContent = new StringContent(jsonContent);

        this.WithResponse(stringContent, statusCode);

        return this;
    }

    /// <summary>
    /// Sets specific response from file path with HTTP status code to mocked HTTP client.
    /// </summary>
    /// <param name="statusCode">Given response status code to set.</param>
    /// <param name="filePath">Given file path to search for mocked JSON response.</param>
    /// <returns>Given instance of <see cref="HttpClientFactoryBuilder"/>.</returns>
    public HttpClientFactoryBuilder WithResponseFromFile(HttpStatusCode statusCode, string filePath)
    {
        var fileContent = GetFileContent(filePath);
        var stringContent = new StringContent(fileContent);

        this.WithResponse(stringContent, statusCode);

        return this;
    }

    /// <summary>
    /// Gets configured mock of <see cref="IHttpClientFactory"/>.
    /// </summary>
    /// <returns>The mock of <see cref="IHttpClientFactory"/>.</returns>
    public Mock<IHttpClientFactory> GetResult() =>
        this.clientFactoryMock;

    private void BuildHttpClient(string clientName)
    {
        var httpClient = new HttpClient(this.httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://test.com"),
        };

        this.clientFactoryMock
            .Setup(factory => factory.CreateClient(clientName))
            .Returns(httpClient)
            .Verifiable();
    }

    private void WithResponse(HttpContent content, HttpStatusCode statusCode)
    {
        const string methodName = "SendAsync";

        this.httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                methodName,
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = content,
            })
            .Verifiable();
    }

    private static string GetFileContent(string filePath)
    {
        var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var pathToFile = Path.Combine(currentDirectory!, "ResponseContent", filePath);

        using var stream = new FileStream(pathToFile, FileMode.Open, FileAccess.Read, FileShare.Read);
        var buffer = new byte[stream.Length];

        stream.Read(buffer, offset: 0, count: buffer.Length);

        return Encoding.Default.GetString(buffer);
    }
}
