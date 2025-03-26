using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EcoinverGMAO_api.Models.Entities;
using EcoinverGMAO_api.Models.Dto;
using EcoinverGMAO_api.Data;

namespace EcoinverGMAO_api.Services
{
    public interface ICultiveService
    {
        Task<IEnumerable<CultiveDto>> GetAllCultivesAsync();
        Task<CultiveDto> GetCultiveByIdAsync(int id);
    }

    public class CultiveService : ICultiveService
    {
        private readonly IRepository<Cultive> _repository;
        private readonly IMapper _mapper;

        public CultiveService(IRepository<Cultive> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CultiveDto>> GetAllCultivesAsync()
        {
            var cultives = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CultiveDto>>(cultives);
        }

        public async Task<CultiveDto> GetCultiveByIdAsync(int id)
        {
            var cultive = await _repository.GetByIdAsync(id);
            if (cultive == null)
                return null;
            return _mapper.Map<CultiveDto>(cultive);
        }
    }
}
