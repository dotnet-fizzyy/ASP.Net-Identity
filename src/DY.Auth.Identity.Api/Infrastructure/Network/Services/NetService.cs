using AutoMapper;

using DY.Auth.Identity.Api.Core.Interfaces.Infrastructure;
using DY.Auth.Identity.Api.Core.Models;
using DY.Auth.Identity.Api.Infrastructure.Network.Models;
using DY.Auth.Identity.Api.Startup.ApplicationSettings;

using Newtonsoft.Json;

using System.Net.Http;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.Infrastructure.Network.Services;

/// <inheritdoc />
public class NetService : INetService
{
    private const string IpStackUrl = "http://api.ipstack.com";

    private static readonly HttpClient HttpClient = new ();

    private readonly IMapper mapper;
    private readonly AppSettings appSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="NetService"/> class.
    /// </summary>
    /// <param name="mapper"><see cref="IMapper"/>.</param>
    /// <param name="appSettings"><see cref="AppSettings"/>.</param>
    public NetService(IMapper mapper, AppSettings appSettings)
    {
        this.mapper = mapper;
        this.appSettings = appSettings;
    }

    /// <inheritdoc />
    public async Task<IpAddressDetails> GetIpAddressDetails(string ipv4)
    {
        var httpResponse =
              await HttpClient.GetAsync($"{IpStackUrl}/{ipv4}?access_key={this.appSettings.IpStackSettings.AccessKey}");
        var responseMessage = httpResponse.EnsureSuccessStatusCode();

        var jsonResponseMessage = await responseMessage.Content.ReadAsStringAsync();
        var ipStackResponse = JsonConvert.DeserializeObject<IpStackResponseModel>(jsonResponseMessage);

        var ipAddressDetails = this.mapper.Map<IpAddressDetails>(ipStackResponse);

        return ipAddressDetails;
    }
}
