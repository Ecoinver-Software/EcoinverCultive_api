using AutoMapper;
using EcoinverGMAO_api.Models.Entities;
using EcoinverGMAO_api.Models.Dto;

namespace EcoinverGMAO_api.Profiles
{
    public class CultivePlanningDetailsProfile : Profile
    {
        public CultivePlanningDetailsProfile()
        {
            // Map entity → read‑DTO
            CreateMap<CultivePlanningDetails, CultivePlanningDetailsDto>();

            // Map create‑DTO → entity
            CreateMap<CreateCultivePlanningDetailsDto, CultivePlanningDetails>();

            // Map update‑DTO → entity
            CreateMap<UpdateCultivePlanningDetailsDto, CultivePlanningDetails>();

            
        }
    }
}
