using AutoMapper;
using EcoinverGMAO_api.Models.Entities;
using EcoinverGMAO_api.Models.Dto;

namespace EcoinverGMAO_api.Profiles
{
    public class CultivePlanningProfile : Profile
    {
        public CultivePlanningProfile()
        {
            CreateMap<CultivePlanning, CultivePlanningDto>().ReverseMap();
            CreateMap<CultivePlanning, CreateCultivePlanningDto>().ReverseMap();
            CreateMap<CultivePlanning, UpdateCultivePlanningDto>().ReverseMap();
        }
    }
}