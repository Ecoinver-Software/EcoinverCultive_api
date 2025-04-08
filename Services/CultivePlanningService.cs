using AutoMapper;
using EcoinverGMAO_api.Data;
using EcoinverGMAO_api.Models.Entities;
using EcoinverGMAO_api.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoinverGMAO_api.Services
{
    public interface ICultivePlanningService
    {
        Task<IEnumerable<CultivePlanningDto>> GetAllAsync();
        Task<CultivePlanningDto> GetByIdAsync(int id);
        Task<CultivePlanningDto> CreateAsync(CreateCultivePlanningDto dto);
        Task<CultivePlanningDto> UpdateAsync(int id, UpdateCultivePlanningDto dto);
        Task<bool> DeleteAsync(int id);
    }

    public class CultivePlanningService : ICultivePlanningService
    {
        private readonly IRepository<CultivePlanning> _repository;
        private readonly IMapper _mapper;

        public CultivePlanningService(IRepository<CultivePlanning> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CultivePlanningDto>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CultivePlanningDto>>(items);
        }

        public async Task<CultivePlanningDto> GetByIdAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
                return null;
            return _mapper.Map<CultivePlanningDto>(item);
        }

        public async Task<CultivePlanningDto> CreateAsync(CreateCultivePlanningDto dto)
        {
            var entity = _mapper.Map<CultivePlanning>(dto);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<CultivePlanningDto>(created);
        }

        public async Task<CultivePlanningDto> UpdateAsync(int id, UpdateCultivePlanningDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return null;

            _mapper.Map(dto, entity);

            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<CultivePlanningDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return false;
            await _repository.DeleteAsync(entity);
            return true;
        }
    }
}