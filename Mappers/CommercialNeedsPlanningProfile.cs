using AutoMapper;
using EcoinverGMAO_api.Models;
using EcoinverGMAO_api.Models.Dto;
using EcoinverGMAO_api.Models.Entities;

namespace EcoinverGMAO_api.Profiles
{
    public class CommercialNeedsPlanningProfile : Profile
    {
        public CommercialNeedsPlanningProfile()
        {
            CreateMap<CommercialNeedsPlanning, CommercialNeedsPlanningDto>().ReverseMap();
            CreateMap<CommercialNeedsPlanning, CreateCommercialNeedsPlanningDto>().ReverseMap();
            CreateMap<CommercialNeedsPlanning, UpdateCommercialNeedsPlanningDto>().ReverseMap();
        }
    }
}
