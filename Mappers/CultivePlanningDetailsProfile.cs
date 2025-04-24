using AutoMapper;
using EcoinverGMAO_api.Models.Entities;
using EcoinverGMAO_api.Models.Dto;

namespace EcoinverGMAO_api.Profiles
{
    public class CultivePlanningDetailsProfile : Profile
    {
        public CultivePlanningDetailsProfile()
        {
            // lectura
            CreateMap<CultivePlanningDetails, CultivePlanningDetailsDto>();

            // creación
            CreateMap<CreateCultivePlanningDetailsDto, CultivePlanningDetails>();

            // actualización
            CreateMap<UpdateCultivePlanningDetailsDto, CultivePlanningDetails>()
                .ForMember(
                    dest => dest.CultivePlanningId,
                    opt => opt.MapFrom(src => src.CultivePlanningId)
                );
        }
    }
}
