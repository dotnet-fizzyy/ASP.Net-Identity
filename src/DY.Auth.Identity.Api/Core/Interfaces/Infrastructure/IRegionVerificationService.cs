using DY.Auth.Identity.Api.Core.Models;

using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.Core.Interfaces.Infrastructure;

/// <summary>
/// Region verification service.
/// </summary>
public interface IRegionVerificationService
{
    /// <summary>
    /// Gets region information about IP address.
    /// </summary>
    /// <param name="ipv4">IP address with V4 format.</param>
    /// <returns>A <see cref="Task{IpAddressDetails}"/> representing the result of the asynchronous operation.</returns>
    Task<IpAddressDetails> GetIpAddressDetailsAsync(string ipv4);
}
