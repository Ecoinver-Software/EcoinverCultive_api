using AutoMapper;
using EcoinverGMAO_api.Models;
using EcoinverGMAO_api.Models.Dto;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EcoinverGMAO_api.Profiles
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyDto>().ReverseMap();
            CreateMap<Company, CreateCompanyDto>().ReverseMap();
            CreateMap<Company, UpdateCompanyDto>().ReverseMap();
        }
    }
}
