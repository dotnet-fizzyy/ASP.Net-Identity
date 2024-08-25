using AutoMapper;

using DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.CreateRole;
using DY.Auth.Identity.Api.Core.Entities;

namespace DY.Auth.Identity.Api.ApplicationLogic.Mapping;

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
        this.CreateMap<CreateRoleCommand, AppRole>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }
}
