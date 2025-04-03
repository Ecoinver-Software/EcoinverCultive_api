using AutoMapper;
using EcoinverGMAO_api.Models;
using EcoinverGMAO_api.Models.Dto;
using EcoinverGMAO_api.Models.Entities;

namespace EcoinverGMAO_api.Profiles
{
    public class CommercialNeedsPlanningDetailsProfile : Profile
    {
        public CommercialNeedsPlanningDetailsProfile()
        {
            CreateMap<CommercialNeedsPlanningDetails, CommercialNeedsPlanningDetailsDto>().ReverseMap();
            CreateMap<CommercialNeedsPlanningDetails, CreateCommercialNeedsPlanningDetailsDto>().ReverseMap();
            CreateMap<CommercialNeedsPlanningDetails, UpdateCommercialNeedsPlanningDetailsDto>().ReverseMap();
        }
    }
}
