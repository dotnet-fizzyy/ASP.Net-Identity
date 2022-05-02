using AutoMapper;

using IdentityWebApi.Core.Models;
using IdentityWebApi.Infrastructure.Net.Models;

namespace IdentityWebApi.ApplicationLogic.Mappers;

public class NetServiceProfile : Profile
{
    public NetServiceProfile()
    {
        CreateMap<IpStackResponseModel, IpAddressDetails>();
    }
}