using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EcoinverGMAO_api.Models.Entities;
using EcoinverGMAO_api.Models.Dto;
using EcoinverGMAO_api.Data;

namespace EcoinverGMAO_api.Services
{
    public interface IGenderService
    {
        Task<IEnumerable<GenderDto>> GetAllGendersAsync();
        Task<GenderDto> GetGenderByIdAsync(int id);
    }

    public class GenderService : IGenderService
    {
        private readonly IRepository<Gender> _repository;
        private readonly IMapper _mapper;

        public GenderService(IRepository<Gender> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GenderDto>> GetAllGendersAsync()
        {
            var genders = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<GenderDto>>(genders);
        }

        public async Task<GenderDto> GetGenderByIdAsync(int id)
        {
            var gender = await _repository.GetByIdAsync(id);
            if (gender == null)
                return null;
            return _mapper.Map<GenderDto>(gender);
        }
    }
}
