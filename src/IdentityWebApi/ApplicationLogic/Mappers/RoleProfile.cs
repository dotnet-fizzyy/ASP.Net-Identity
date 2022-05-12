using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Entities;

namespace IdentityWebApi.ApplicationLogic.Mappers;

/// <summary>
/// Configuration of Role models and entities mapping.
/// </summary>
public class RoleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoleProfile"/> class.
    /// </summary>
    public RoleProfile()
    {
        this.CreateMap<RoleDto, AppRole>().ReverseMap();
        this.CreateMap<RoleCreationDto, AppRole>();
    }
}
