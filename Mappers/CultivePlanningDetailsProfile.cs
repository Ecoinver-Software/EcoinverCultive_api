using AutoMapper;
using EcoinverGMAO_api.Models.Entities;
using EcoinverGMAO_api.Models.Dto;

namespace EcoinverGMAO_api.Profiles
{
    public class CultivePlanningDetailsProfile : Profile
    {
        public CultivePlanningDetailsProfile()
        {
            CreateMap<CultivePlanningDetails, CultivePlanningDetailsDto>().ReverseMap();
            CreateMap<CultivePlanningDetails, CreateCultiveProductionDto>().ReverseMap();
            CreateMap<CultivePlanningDetails, UpdateCultivePlanningDetailsDto>().ReverseMap();
        }
    }
}