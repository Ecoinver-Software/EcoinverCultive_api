using AutoMapper;
using EcoinverGMAO_api.Models.Dto;
using EcoinverGMAO_api.Models.Entities;

namespace EcoinverGMAO_api.Profiles
{
    public class CultiveProductionProfile : Profile
    {
        public CultiveProductionProfile()
        {
            // Entidad → DTO de salida
            CreateMap<CultiveProduction, CultiveProductionDto>();

            // DTO de creación → Entidad
            CreateMap<CreateCultiveProductionDto, CultiveProduction>()
                .ForMember(d => d.CultiveId, opt => opt.MapFrom(src => src.CultiveId))
                .ForMember(d => d.CultivePlanningDetailsId, opt => opt.MapFrom(src => src.CultivePlanningDetailsId))
                .ForMember(d => d.Kilos, opt => opt.MapFrom(src => src.Kilos))
                .ForMember(d => d.FechaInicio, opt => opt.MapFrom(src => src.FechaInicio))
                .ForMember(d => d.FechaFin, opt => opt.MapFrom(src => src.FechaFin));

            // DTO de actualización → Entidad
            CreateMap<UpdateCultiveProductionDto, CultiveProduction>()
                .ForMember(d => d.CultiveId, opt => opt.MapFrom(src => src.CultiveId))
                .ForMember(d => d.CultivePlanningDetailsId, opt => opt.MapFrom(src => src.CultivePlanningDetailsId))
                
                .ForMember(d => d.FechaInicio, opt => opt.MapFrom(src => src.FechaInicio))
                .ForMember(d => d.FechaFin, opt => opt.MapFrom(src => src.FechaFin));
        }
    }
}
