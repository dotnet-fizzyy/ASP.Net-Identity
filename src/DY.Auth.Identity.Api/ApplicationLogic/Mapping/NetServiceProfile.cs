using AutoMapper;

using DY.Auth.Identity.Api.Core.Models;
using DY.Auth.Identity.Api.Infrastructure.Network.Models;

namespace DY.Auth.Identity.Api.ApplicationLogic.Mapping;

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
