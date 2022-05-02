using AutoMapper;

using IdentityWebApi.Core.Models;
using IdentityWebApi.Infrastructure.IpStack.Models;

namespace IdentityWebApi.ApplicationLogic.Mappers;

public class NetServiceProfile : Profile
{
    public NetServiceProfile()
    {
        CreateMap<IpStackResponseModel, IpAddressDetails>();
    }
}