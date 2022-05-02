using IdentityWebApi.Core.Models;

using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.Infrastructure;

public interface INetService
{
    Task<IpAddressDetails> GetIpAddressDetails(string ipv4);
}