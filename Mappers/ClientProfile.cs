using AutoMapper;
using EcoinverGMAO_api.Models.Entities;  // Asegúrate de que aquí esté tu entidad Client
using EcoinverGMAO_api.Models.Dto;

namespace EcoinverGMAO_api.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ClientDto>();
        }
    }
}
