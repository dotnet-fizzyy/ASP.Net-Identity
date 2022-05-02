using AutoMapper;

using IdentityWebApi.Core.ApplicationSettings;
using IdentityWebApi.Core.Interfaces.Infrastructure;
using IdentityWebApi.Core.Models;
using IdentityWebApi.Infrastructure.IpStack.Models;

using Newtonsoft.Json;

using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityWebApi.Infrastructure.IpStack.Services;

public class NetService : INetService
{
    private static readonly HttpClient HttpClient = new ();

    private const string IpStackUrl = "http://api.ipstack.com";
    
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public NetService(IMapper mapper, AppSettings appSettings)
    {
        _mapper = mapper;
        _appSettings = appSettings;
    }
    
    public async Task<IpAddressDetails> GetIpAddressDetails(string ipv4)
    {
        var httpResponse = await HttpClient.GetAsync($"{IpStackUrl}/{ipv4}?access_key={_appSettings.IpStackSettings.AccessKey}");
        var responseMessage = httpResponse.EnsureSuccessStatusCode();

        var jsonResponseMessage = await responseMessage.Content.ReadAsStringAsync();
        var ipStackResponse = JsonConvert.DeserializeObject<IpStackResponseModel>(jsonResponseMessage);

        var ipAddressDetails = _mapper.Map<IpAddressDetails>(ipStackResponse);
        
        return ipAddressDetails;
    }
}