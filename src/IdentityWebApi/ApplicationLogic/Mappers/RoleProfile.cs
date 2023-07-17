using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.ApplicationLogic.Models.Output;
using IdentityWebApi.ApplicationLogic.Services.Role.Commands.CreateRole;
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
        this.CreateMap<RoleCreationDto, CreateRoleCommand>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        this.CreateMap<CreateRoleCommand, AppRole>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        this.CreateMap<AppRole, RoleResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp));
    }
}
