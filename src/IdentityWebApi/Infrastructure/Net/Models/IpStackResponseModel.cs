using Newtonsoft.Json;

namespace IdentityWebApi.Infrastructure.Net.Models;

public class IpStackResponseModel
{
    [JsonProperty("country_code")]
    public string CountryCode { get; set; }

    [JsonProperty("country_name")]
    public string CountryName { get; set; }

    [JsonProperty("region_code")]
    public string RegionCode { get; set; }

    [JsonProperty("region_name")]
    public string RegionName { get; set; }

    [JsonProperty("city")]
    public string City { get; set; }
}