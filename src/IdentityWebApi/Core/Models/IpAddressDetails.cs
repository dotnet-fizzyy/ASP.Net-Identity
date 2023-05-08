namespace IdentityWebApi.Core.Models;

/// <summary>
/// Ip address details model.
/// </summary>
public class IpAddressDetails
{
    /// <summary>
    /// Gets or sets ip address country code.
    /// </summary>
    public string CountryCode { get; set; }

    /// <summary>
    /// Gets or sets ip address country name.
    /// </summary>
    public string CountryName { get; set; }

    /// <summary>
    /// Gets or sets ip address region code.
    /// </summary>
    public string RegionCode { get; set; }

    /// <summary>
    /// Gets or sets ip address region name.
    /// </summary>
    public string RegionName { get; set; }

    /// <summary>
    /// Gets or sets ip address city.
    /// </summary>
    public string City { get; set; }
}
