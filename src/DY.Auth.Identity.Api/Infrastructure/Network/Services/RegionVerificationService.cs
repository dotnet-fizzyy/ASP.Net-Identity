using AutoMapper;

using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Interfaces.Infrastructure;
using DY.Auth.Identity.Api.Core.Models;
using DY.Auth.Identity.Api.Core.Utilities;
using DY.Auth.Identity.Api.Infrastructure.Network.Models;
using DY.Auth.Identity.Api.Startup.ApplicationSettings;

using Newtonsoft.Json;

using System.Net.Http;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.Infrastructure.Network.Services;

/// <inheritdoc />
public class RegionVerificationService : IRegionVerificationService
{
    private const string AccessKeyQueryParameter = "access_key";

    private readonly IHttpClientFactory clientFactory;
    private readonly IMapper mapper;
    private readonly AppSettings appSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegionVerificationService"/> class.
    /// </summary>
    /// <param name="clientFactory">The instance of <see cref="IHttpClientFactory"/>.</param>
    /// <param name="mapper">The instance of <see cref="IMapper"/>.</param>
    /// <param name="appSettings">The instance of <see cref="AppSettings"/>.</param>
    public RegionVerificationService(
        IHttpClientFactory clientFactory,
        IMapper mapper,
        AppSettings appSettings)
    {
        this.clientFactory = clientFactory;
        this.mapper = mapper;
        this.appSettings = appSettings;
    }

    /// <inheritdoc />
    public async Task<IpAddressDetails> GetIpAddressDetails(string ipv4)
    {
        var httpClient = this.GetHttpClient();

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri: ipv4);
        this.SetAccessKeyQueryParameter(httpRequestMessage);

        var httpResponse = await httpClient.SendAsync(httpRequestMessage);
        var responseMessage = httpResponse.EnsureSuccessStatusCode();

        var jsonResponseMessage = await responseMessage.Content.ReadAsStringAsync();
        var ipStackResponse = JsonConvert.DeserializeObject<IpStackResponseModel>(jsonResponseMessage);

        return this.mapper.Map<IpAddressDetails>(ipStackResponse);
    }

    private HttpClient GetHttpClient() =>
        this.clientFactory.CreateClient(InternalApi.RegionVerification.ToString());

    private void SetAccessKeyQueryParameter(HttpRequestMessage httpRequestMessage) =>
        httpRequestMessage.AddQueryParameter(AccessKeyQueryParameter, this.appSettings.IpStackSettings.AccessKey);
}
