using AutoMapper;
using EcoinverGMAO_api.Models;
using EcoinverGMAO_api.Models.Dto;

namespace EcoinverGMAO_api.Profiles
{
    public class CommercialNeedsProfile : Profile
    {
        public CommercialNeedsProfile()
        {
            CreateMap<CommercialNeeds, CommercialNeedsDto>().ReverseMap();
            CreateMap<CommercialNeeds, CreateCommercialNeedsDto>().ReverseMap();
            CreateMap<CommercialNeeds, UpdateCommercialNeedsDto>().ReverseMap();
        }
    }
}
