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
        private readonly IRepository<Cultive> _cultiveRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CultiveProductionService> _logger;

        public CultiveProductionService(
            IRepository<CultiveProduction> productionRepo,
            IRepository<CultivePlanningDetails> detailsRepo,
            IRepository<Cultive> cultiveRepo,
            IMapper mapper,
            ILogger<CultiveProductionService> logger)
        {
            _productionRepo = productionRepo;
            _detailsRepo = detailsRepo;
            _cultiveRepo = cultiveRepo;
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
            // 1) Validar existencia del detalle de planificación
            var detail = await _detailsRepo.GetByIdAsync(dto.CultivePlanningDetailsId);
            if (detail == null)
                throw new KeyNotFoundException($"CultivePlanningDetails {dto.CultivePlanningDetailsId} no encontrado.");

            // 2) Validar existencia del cultivo
            var cultive = await _cultiveRepo.GetByIdAsync(dto.CultiveId);
            if (cultive == null)
                throw new KeyNotFoundException($"Cultive {dto.CultiveId} no encontrado.");

            // 3) Mapear DTO → Entidad
            var entity = _mapper.Map<CultiveProduction>(dto);

            // 4) Sobrescribir kilos crudos con los del detalle
            entity.Kilos = detail.Kilos.ToString();

            // 5) Calcular kilos ajustados por superficie (decimal * decimal)
            if (!decimal.TryParse(entity.Kilos, out var kilosVal))
                throw new InvalidOperationException($"Kilos inválidos: {entity.Kilos}");

            var superficieDec = (decimal)cultive.Superficie;
            entity.KilosAjustados = (kilosVal * superficieDec).ToString();

            // 6) Persistir
            var created = await _productionRepo.AddAsync(entity);
            _logger.LogInformation("Creada CultiveProduction {Id} para Cultive {CultiveId}.",
                                   created.Id, created.CultiveId);

            return _mapper.Map<CultiveProductionDto>(created);
        }

        public async Task<CultiveProductionDto> UpdateAsync(int id, UpdateCultiveProductionDto dto)
        {
            // 1) Recuperar existente
            var existing = await _productionRepo.GetByIdAsync(id)
                         ?? throw new KeyNotFoundException($"CultiveProduction {id} no encontrada.");

            // 2) Si cambia el cultivo, validar
            if (dto.CultiveId != existing.CultiveId)
            {
                var cultiveNew = await _cultiveRepo.GetByIdAsync(dto.CultiveId)
                                ?? throw new KeyNotFoundException($"Cultive {dto.CultiveId} no encontrado.");
                existing.CultiveId = dto.CultiveId;
            }

            // 3) Si cambia el detalle, validar y actualizar kilos crudos
            if (dto.CultivePlanningDetailsId != existing.CultivePlanningDetailsId)
            {
                var detailNew = await _detailsRepo.GetByIdAsync(dto.CultivePlanningDetailsId)
                               ?? throw new KeyNotFoundException($"CultivePlanningDetails {dto.CultivePlanningDetailsId} no encontrado.");
                existing.CultivePlanningDetailsId = dto.CultivePlanningDetailsId;
                existing.Kilos = detailNew.Kilos.ToString();
            }

            // 4) Mapear el resto de campos: fechas
            existing.FechaInicio = dto.FechaInicio;
            existing.FechaFin = dto.FechaFin;

            // 5) Recalcular kilos ajustados
            if (!decimal.TryParse(existing.Kilos, out var kilosVal))
                throw new InvalidOperationException($"Kilos inválidos: {existing.Kilos}");
            var cultiveForCalc = await _cultiveRepo.GetByIdAsync(existing.CultiveId);
            var superficieDec2 = (decimal)cultiveForCalc.Superficie;
            existing.KilosAjustados = (kilosVal * superficieDec2).ToString();

            // 6) Persistir
            var updated = await _productionRepo.UpdateAsync(existing);
            _logger.LogInformation("Actualizada CultiveProduction {Id}.", updated.Id);

            return _mapper.Map<CultiveProductionDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _productionRepo.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("CultiveProduction {Id} no encontrada para borrar.", id);
                return false;
            }

            // Hard delete en lugar de soft delete
            await _productionRepo.HardDeleteAsync(entity);
            _logger.LogInformation("Hard-deleted CultiveProduction {Id}.", id);
            return true;
        }
    }
}