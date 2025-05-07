using AutoMapper;
using EcoinverGMAO_api.Data;
using EcoinverGMAO_api.Models.Entities;
using EcoinverGMAO_api.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoinverGMAO_api.Services
{
    public interface ICultivePlanningDetailsService
    {
        Task<IEnumerable<CultivePlanningDetailsDto>> GetAllAsync();
        Task<CultivePlanningDetailsDto> GetByIdAsync(int id);
        Task<CultivePlanningDetailsDto> CreateAsync(CreateCultivePlanningDetailsDto dto);
        Task<CultivePlanningDetailsDto> UpdateAsync(int id, UpdateCultivePlanningDetailsDto dto);
        Task<bool> DeleteAsync(int id);
    }

    public class CultivePlanningDetailsService : ICultivePlanningDetailsService
    {
        private readonly IRepository<CultivePlanningDetails> _repository;
        private readonly IMapper _mapper;

        public CultivePlanningDetailsService(IRepository<CultivePlanningDetails> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CultivePlanningDetailsDto>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CultivePlanningDetailsDto>>(items);
        }

        public async Task<CultivePlanningDetailsDto> GetByIdAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
                return null;
            return _mapper.Map<CultivePlanningDetailsDto>(item);
        }

        public async Task<CultivePlanningDetailsDto> CreateAsync(CreateCultivePlanningDetailsDto dto)
        {
            var entity = _mapper.Map<CultivePlanningDetails>(dto);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<CultivePlanningDetailsDto>(created);
        }

        public async Task<CultivePlanningDetailsDto> UpdateAsync(int id, UpdateCultivePlanningDetailsDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return null;
            // Mapeamos el dto sobre la entidad existente.
            _mapper.Map(dto, entity);
            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<CultivePlanningDetailsDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return false;

            // Hard delete en lugar de soft delete
            await _repository.HardDeleteAsync(entity);
            return true;
        }
    }
}