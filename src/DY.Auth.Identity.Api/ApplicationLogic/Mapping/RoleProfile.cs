using AutoMapper;

using DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.CreateRole;
using DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.UpdateRole;
using DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Queries.GetRoleById;
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

        this.CreateMap<AppRole, CreateRoleResult>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp))
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate));

        this.CreateMap<UpdateRoleCommand, AppRole>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp));

        this.CreateMap<AppRole, UpdateRoleResult>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp))
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate));

        this.CreateMap<AppRole, GetRoleByIdResult>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.MapFrom(src => src.ConcurrencyStamp))
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate));
    }
}
