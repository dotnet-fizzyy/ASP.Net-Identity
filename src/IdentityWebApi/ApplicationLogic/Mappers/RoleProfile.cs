using AutoMapper;

using IdentityWebApi.Core.Entities;
using IdentityWebApi.Presentation.Models.Action;
using IdentityWebApi.Presentation.Models.DTO;

namespace IdentityWebApi.ApplicationLogic.Mappers;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<RoleDto, AppRole>().ReverseMap();
        CreateMap<RoleCreationActionModel, AppRole>();
    }
}
