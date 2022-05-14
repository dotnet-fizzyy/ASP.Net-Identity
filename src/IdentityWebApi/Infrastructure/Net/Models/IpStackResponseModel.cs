using Newtonsoft.Json;

namespace IdentityWebApi.Infrastructure.Net.Models;

/// <summary>
/// Response model from IpStack service.
/// </summary>
public class IpStackResponseModel
{
    /// <summary>
    /// Gets or sets country code.
    /// </summary>
    [JsonProperty("country_code")]
    public string CountryCode { get; set; }

    /// <summary>
    /// Gets or sets country name.
    /// </summary>
    [JsonProperty("country_name")]
    public string CountryName { get; set; }

    /// <summary>
    /// Gets or sets region code.
    /// </summary>
    [JsonProperty("region_code")]
    public string RegionCode { get; set; }

    /// <summary>
    /// Gets or sets region name.
    /// </summary>
    [JsonProperty("region_name")]
    public string RegionName { get; set; }

    /// <summary>
    /// Gets or sets city.
    /// </summary>
    [JsonProperty("city")]
    public string City { get; set; }
}