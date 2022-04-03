using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Entities;

namespace IdentityWebApi.ApplicationLogic.Mappers;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<RoleDto, AppRole>().ReverseMap();
        CreateMap<RoleCreationDto, AppRole>();
    }
}
