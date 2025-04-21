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
        private readonly IRepository<CultiveProduction> _productionRepo;
        private readonly IRepository<CultivePlanningDetails> _detailsRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CultiveProductionService> _logger;

        public CultiveProductionService(
            IRepository<CultiveProduction> productionRepo,
            IRepository<CultivePlanningDetails> detailsRepo,
            IMapper mapper,
            ILogger<CultiveProductionService> logger)
        {
            _productionRepo = productionRepo;
            _detailsRepo = detailsRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CultiveProductionDto>> GetAllAsync()
        {
            var entities = await _productionRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<CultiveProductionDto>>(entities);
        }

        public async Task<CultiveProductionDto> GetByIdAsync(int id)
        {
            var entity = await _productionRepo.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("CultiveProduction con ID {Id} no encontrado.", id);
                return null;
            }
            return _mapper.Map<CultiveProductionDto>(entity);
        }

        public async Task<CultiveProductionDto> CreateAsync(CreateCultiveProductionDto dto)
        {
            // 1) validar existencia del detail
            var detail = await _detailsRepo.GetByIdAsync(dto.CultivePlanningDetailsId);
            if (detail == null)
            {
                _logger.LogWarning("No existe CultivePlanningDetails con ID {Id}.", dto.CultivePlanningDetailsId);
                throw new KeyNotFoundException($"CultivePlanningDetails con ID {dto.CultivePlanningDetailsId} no encontrado");
            }

            // 2) mapear a producción y asignar sólo la FK
            var entity = _mapper.Map<CultiveProduction>(dto);
            entity.CultivePlanningDetailsId = dto.CultivePlanningDetailsId;

            // 3) guardar
            var created = await _productionRepo.AddAsync(entity);
            _logger.LogInformation("Creada CultiveProduction con ID {Id}.", created.Id);
            return _mapper.Map<CultiveProductionDto>(created);
        }

        public async Task<CultiveProductionDto> UpdateAsync(int id, UpdateCultiveProductionDto dto)
        {
            // 1) recuperar existente
            var existing = await _productionRepo.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("CultiveProduction con ID {Id} no encontrado.", id);
                return null;
            }

            // 2) si cambió el detail, validar
            if (dto.CultivePlanningDetailsId > 0
                && dto.CultivePlanningDetailsId != existing.CultivePlanningDetailsId)
            {
                var detail = await _detailsRepo.GetByIdAsync(dto.CultivePlanningDetailsId);
                if (detail == null)
                {
                    _logger.LogWarning("No existe CultivePlanningDetails con ID {Id}.", dto.CultivePlanningDetailsId);
                    throw new KeyNotFoundException($"CultivePlanningDetails con ID {dto.CultivePlanningDetailsId} no encontrado");
                }
                existing.CultivePlanningDetailsId = dto.CultivePlanningDetailsId;
            }

            // 3) mapear el resto y actualizar
            _mapper.Map(dto, existing);
            var updated = await _productionRepo.UpdateAsync(existing);
            _logger.LogInformation("Actualizada CultiveProduction con ID {Id}.", updated.Id);
            return _mapper.Map<CultiveProductionDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _productionRepo.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("CultiveProduction con ID {Id} no encontrado.", id);
                return false;
            }
            await _productionRepo.DeleteAsync(entity);
            _logger.LogInformation("Eliminada CultiveProduction con ID {Id}.", id);
            return true;
        }
    }
}
