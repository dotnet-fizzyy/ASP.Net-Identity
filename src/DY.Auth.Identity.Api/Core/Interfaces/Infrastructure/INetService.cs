using DY.Auth.Identity.Api.Core.Models;

using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.Core.Interfaces.Infrastructure;

/// <summary>
/// Service for performing network operations.
/// </summary>
public interface INetService
{
    /// <summary>
    /// Get information about IP address region.
    /// </summary>
    /// <param name="ipv4">IP address with V4 format.</param>
    /// <returns>A <see cref="Task{IpAddressDetails}"/> representing the result of the asynchronous operation.</returns>
    Task<IpAddressDetails> GetIpAddressDetails(string ipv4);
}
