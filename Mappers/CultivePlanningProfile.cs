using AutoMapper;
using EcoinverGMAO_api.Models.Entities;
using EcoinverGMAO_api.Models.Dto;

public class CultivePlanningProfile : Profile
{
    public CultivePlanningProfile()
    {
        CreateMap<CreateCultivePlanningDto, CultivePlanning>()
            // mappea IdGenero del DTO al entity
            .ForMember(dest => dest.IdGenero, opt => opt.MapFrom(src => src.IdGenero))
            .ReverseMap();

        CreateMap<UpdateCultivePlanningDto, CultivePlanning>()
            .ForMember(dest => dest.IdGenero, opt => opt.MapFrom(src => src.IdGenero))
            .ReverseMap();

        CreateMap<CultivePlanning, CultivePlanningDto>()
            .ReverseMap();
    }
}
