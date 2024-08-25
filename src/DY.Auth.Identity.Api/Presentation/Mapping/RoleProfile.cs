using AutoMapper;

using DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.CreateRole;
using DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.UpdateRole;
using DY.Auth.Identity.Api.Presentation.Models.DTO.Role;

namespace DY.Auth.Identity.Api.Presentation.Mapping;

/// <summary>
/// Role profile mapping.
/// </summary>
public class RoleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoleProfile"/> class.
    /// </summary>
    public RoleProfile()
    {
        this.CreateMap<CreateRoleDto, CreateRoleCommand>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        this.CreateMap<CreateRoleResult, RoleDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp))
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate));

        this.CreateMap<UpdateRoleDto, UpdateRoleCommand>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp));

        this.CreateMap<UpdateRoleResult, RoleDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp))
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate));
    }
}
