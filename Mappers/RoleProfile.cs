using AutoMapper;
using EcoinverGMAO_api.Models.Identity;
using EcoinverGMAO_api.Models.Dto;

namespace EcoinverGMAO_api.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Role, CreateRoleDto>().ReverseMap();
            CreateMap<Role, UpdateRoleDto>().ReverseMap();
        }
    }
}
