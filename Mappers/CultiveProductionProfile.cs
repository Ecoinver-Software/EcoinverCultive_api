using AutoMapper;
using EcoinverGMAO_api.Models.Entities;
using EcoinverGMAO_api.Models.Dto;

namespace EcoinverGMAO_api.Profiles
{
    public class CultiveProductionProfile : Profile
    {
        public CultiveProductionProfile()
        {
            // Existing mappings
            CreateMap<CultiveProduction, CultiveProductionDto>().ReverseMap();
            CreateMap<CultiveProduction, CreateCultiveProductionDto>().ReverseMap();
            CreateMap<CultiveProduction, UpdateCultiveProductionDto>().ReverseMap();

            // Add the new mappings with explicit property mapping for the foreign key
            CreateMap<CultivePlanningDetails, CultiveProductionDto>()
                .ForMember(dest => dest.CultivePlanningDetailsId, opt => opt.MapFrom(src => src.CultivePlanningId));

            CreateMap<CreateCultiveProductionDto, CultivePlanningDetails>()
                .ForMember(dest => dest.CultivePlanningId, opt => opt.MapFrom(src => src.CultivePlanningDetailsId));

            CreateMap<UpdateCultiveProductionDto, CultivePlanningDetails>()
                .ForMember(dest => dest.CultivePlanningId, opt => opt.MapFrom(src => src.CultivePlanningDetailsId));
        }
    }
}