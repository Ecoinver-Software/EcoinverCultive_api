using AutoMapper;
using EcoinverGMAO_api.Models.Entities;
using EcoinverGMAO_api.Models.Dto;

namespace EcoinverGMAO_api.Profiles
{
    public class CultiveProfile : Profile
    {
        public CultiveProfile()
        {
            CreateMap<Cultive, CultiveDto>();

            // Mapeo de actualización: sólo IdCultivePlanning
            CreateMap<UpdateCultiveDto, Cultive>()
                .ForMember(dest => dest.IdCultivePlanning,
                           opt => opt.MapFrom(src => src.IdCultivePlanning));
        }
    }
}
