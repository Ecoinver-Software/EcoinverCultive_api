using AutoMapper;
using EcoinverGMAO_api.Models.Entities;
using EcoinverGMAO_api.Models.Dto;

namespace EcoinverGMAO_api.Profiles
{
    public class CultiveProductionProfile : Profile
    {
        public CultiveProductionProfile()
        {
            
            CreateMap<CultiveProduction, CultiveProductionDto>().ReverseMap();

            CreateMap<CultiveProduction, CreateCultiveProductionDto>().ReverseMap();

            CreateMap<CultiveProduction, UpdateCultiveProductionDto>().ReverseMap();
        }
    }
}
