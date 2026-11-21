using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Interfaces.Infrastructure;
using DY.Auth.Identity.Api.Core.Models;
using DY.Auth.Identity.Api.Core.Utilities;
using DY.Auth.Identity.Api.Infrastructure.Network.Models;
using DY.Auth.Identity.Api.Startup.ApplicationSettings;

using Newtonsoft.Json;

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.Infrastructure.Network.Services;

/// <summary>
/// Region verification service.
/// </summary>
/// <param name="clientFactory">The instance of <see cref="IHttpClientFactory"/>.</param>
/// <param name="appSettings">The instance of <see cref="AppSettings"/>.</param>
public class RegionVerificationService(
    IHttpClientFactory clientFactory,
    AppSettings appSettings) : IRegionVerificationService
{
    private const string AccessKeyQueryParameter = "access_key";

    private readonly IHttpClientFactory clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
    private readonly AppSettings appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));

    /// <inheritdoc />
    public async Task<IpAddressDetails> GetIpAddressDetailsAsync(string ipv4)
    {
        var httpClient = this.GetHttpClient();

        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri: ipv4);
        this.SetAccessKeyQueryParameter(httpRequestMessage);

        var httpResponse = await httpClient.SendAsync(httpRequestMessage);
        var responseMessage = httpResponse.EnsureSuccessStatusCode();

        var jsonResponseMessage = await responseMessage.Content.ReadAsStringAsync();
        var ipStackResponse = JsonConvert.DeserializeObject<IpStackResponseModel>(jsonResponseMessage);

        return Map(ipStackResponse);
    }

    private HttpClient GetHttpClient() =>
        this.clientFactory.CreateClient(nameof(InternalApi.RegionVerification));

    private void SetAccessKeyQueryParameter(HttpRequestMessage httpRequestMessage) =>
        httpRequestMessage.AddQueryParameter(AccessKeyQueryParameter, this.appSettings.IpStackSettings.AccessKey);

    private static IpAddressDetails Map(IpStackResponseModel source)
    {
        if (source is null)
        {
            return null;
        }

        return new IpAddressDetails
        {
            CountryCode = source.CountryCode,
            CountryName = source.CountryName,
            RegionCode = source.RegionCode,
            RegionName = source.RegionName,
            City = source.City,
        };
    }
}
