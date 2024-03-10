using AutoMapper;

using IdentityWebApi.Core.Models;
using IdentityWebApi.Infrastructure.Network.Models;

namespace IdentityWebApi.ApplicationLogic.Mappers;

/// <summary>
/// Configuration of Net models and entities mapping.
/// </summary>
public class NetServiceProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NetServiceProfile"/> class.
    /// </summary>
    public NetServiceProfile()
    {
        this.CreateMap<IpStackResponseModel, IpAddressDetails>();
    }
}
