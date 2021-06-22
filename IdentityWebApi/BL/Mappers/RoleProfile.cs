using AutoMapper;
using IdentityWebApi.DAL.Entities;
using IdentityWebApi.PL.Models.DTO;

namespace IdentityWebApi.BL.Mappers
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleDto, AppRole>().ReverseMap();
        }
    }
}