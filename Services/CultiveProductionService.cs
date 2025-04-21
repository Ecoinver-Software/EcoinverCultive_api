using AutoMapper;
using EcoinverGMAO_api.Data;
using EcoinverGMAO_api.Models.Entities;
using EcoinverGMAO_api.Models.Dto;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoinverGMAO_api.Services
{
    public interface ICultiveProductionService
    {
        Task<IEnumerable<CultiveProductionDto>> GetAllAsync();
        Task<CultiveProductionDto> GetByIdAsync(int id);
        Task<CultiveProductionDto> CreateAsync(CreateCultiveProductionDto dto);
        Task<CultiveProductionDto> UpdateAsync(int id, UpdateCultiveProductionDto dto);
        Task<bool> DeleteAsync(int id);
    }

    public class CultiveProductionService : ICultiveProductionService
    {
        private readonly IRepository<CultivePlanningDetails> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CultiveProductionService> _logger;

        public CultiveProductionService(
            IRepository<CultivePlanningDetails> repository,
            IMapper mapper,
            ILogger<CultiveProductionService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CultiveProductionDto>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CultiveProductionDto>>(items);
        }

        public async Task<CultiveProductionDto> GetByIdAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                _logger.LogWarning("Cultive production with id {Id} not found.", id);
                return null;
            }
            return _mapper.Map<CultiveProductionDto>(item);
        }

        public async Task<CultiveProductionDto> CreateAsync(CreateCultiveProductionDto dto)
        {
            var entity = _mapper.Map<CultivePlanningDetails>(dto);
            var created = await _repository.AddAsync(entity);
            _logger.LogInformation("Created new cultive production with id {Id}.", created.Id);
            return _mapper.Map<CultiveProductionDto>(created);
        }

        public async Task<CultiveProductionDto> UpdateAsync(int id, UpdateCultiveProductionDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("Cannot update: cultive production with id {Id} not found.", id);
                return null;
            }
            _mapper.Map(dto, entity);
            var updated = await _repository.UpdateAsync(entity);
            _logger.LogInformation("Updated cultive production with id {Id}.", updated.Id);
            return _mapper.Map<CultiveProductionDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("Cannot delete: cultive production with id {Id} not found.", id);
                return false;
            }
            await _repository.DeleteAsync(entity);
            _logger.LogInformation("Deleted cultive production with id {Id}.", id);
            return true;
        }
    }
}