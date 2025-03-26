using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EcoinverGMAO_api.Models.Entities;
using EcoinverGMAO_api.Models.Dto;
using EcoinverGMAO_api.Data;

namespace EcoinverGMAO_api.Services
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetAllClientsAsync();
        Task<ClientDto> GetClientByIdAsync(int id);
    }

    public class ClientService : IClientService
    {
        private readonly IRepository<Client> _repository;
        private readonly IMapper _mapper;

        public ClientService(IRepository<Client> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientDto>> GetAllClientsAsync()
        {
            var clients = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClientDto>>(clients);
        }

        public async Task<ClientDto> GetClientByIdAsync(int id)
        {
            var client = await _repository.GetByIdAsync(id);
            if (client == null)
                return null;
            return _mapper.Map<ClientDto>(client);
        }
    }
}
