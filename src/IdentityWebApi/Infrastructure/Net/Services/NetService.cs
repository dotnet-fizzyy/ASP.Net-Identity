using AutoMapper;

using IdentityWebApi.Core.ApplicationSettings;
using IdentityWebApi.Core.Interfaces.Infrastructure;
using IdentityWebApi.Core.Models;
using IdentityWebApi.Infrastructure.Net.Models;

using Newtonsoft.Json;

using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityWebApi.Infrastructure.Net.Services;

public class NetService : INetService
{
    private static readonly HttpClient HttpClient = new ();

    private const string IpStackUrl = "http://api.ipstack.com";

    private readonly IMapper mapper;
    private readonly AppSettings appSettings;

    public NetService(IMapper mapper, AppSettings appSettings)
    {
        this.mapper = mapper;
        this.appSettings = appSettings;
    }

    public async Task<IpAddressDetails> GetIpAddressDetails(string ipv4)
    {
        var httpResponse = await HttpClient.GetAsync($"{IpStackUrl}/{ipv4}?access_key={this.appSettings.IpStackSettings.AccessKey}");
        var responseMessage = httpResponse.EnsureSuccessStatusCode();

        var jsonResponseMessage = await responseMessage.Content.ReadAsStringAsync();
        var ipStackResponse = JsonConvert.DeserializeObject<IpStackResponseModel>(jsonResponseMessage);

        var ipAddressDetails = this.mapper.Map<IpAddressDetails>(ipStackResponse);

        return ipAddressDetails;
    }
}