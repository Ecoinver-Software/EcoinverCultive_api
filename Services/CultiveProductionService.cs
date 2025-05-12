using AutoMapper;
using EcoinverGMAO_api.Data;
using EcoinverGMAO_api.Models.Entities;
using EcoinverGMAO_api.Models.Dto;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        // Método one-off para recalcular estimados en masa
        Task BulkRecalculateProduccionEstimadoAsync();
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
            var detail = await _detailsRepo.GetByIdAsync(dto.CultivePlanningDetailsId)
                         ?? throw new KeyNotFoundException($"CultivePlanningDetails {dto.CultivePlanningDetailsId} no encontrado.");
            var cultive = await _cultiveRepo.GetByIdAsync(dto.CultiveId)
                         ?? throw new KeyNotFoundException($"Cultive {dto.CultiveId} no encontrado.");

            var entity = _mapper.Map<CultiveProduction>(dto);
            entity.Kilos = detail.Kilos.ToString();

            if (!decimal.TryParse(entity.Kilos, out var kilosVal))
                throw new InvalidOperationException($"Kilos inválidos: {entity.Kilos}");

            var superficie = (decimal)cultive.Superficie;
            entity.KilosAjustados = (kilosVal * superficie).ToString();

            var created = await _productionRepo.AddAsync(entity);
            _logger.LogInformation("Creada CultiveProduction {Id} para Cultive {CultiveId}.", created.Id, created.CultiveId);

            await RecalculateAndSaveCultiveEstimadaAsync(created.CultiveId);
            return _mapper.Map<CultiveProductionDto>(created);
        }

        public async Task<CultiveProductionDto> UpdateAsync(int id, UpdateCultiveProductionDto dto)
        {
            // 1️⃣ Cargar la entidad existente
            var existing = await _productionRepo.GetByIdAsync(id)
                           ?? throw new KeyNotFoundException($"CultiveProduction {id} no encontrada.");

            // 2️⃣ Si cambió el detalle, actualizar la referencia
            if (dto.CultivePlanningDetailsId != existing.CultivePlanningDetailsId)
            {
                existing.CultivePlanningDetailsId = dto.CultivePlanningDetailsId;
            }

            // 3️⃣ Siempre leer el detalle actual (nuevo o viejo) y asignar kilos desde ahí
            var detail = await _detailsRepo.GetByIdAsync(existing.CultivePlanningDetailsId)
                         ?? throw new KeyNotFoundException($"CultivePlanningDetails {existing.CultivePlanningDetailsId} no encontrado.");
            existing.Kilos = detail.Kilos.ToString();

            // 4️⃣ Si cambió el cultivo, actualizar la referencia
            if (dto.CultiveId != existing.CultiveId)
            {
                // (opcional) podrías validar que exista el nuevo Cultive
                existing.CultiveId = dto.CultiveId;
            }

            // 5️⃣ Actualizar rangos de fecha
            existing.FechaInicio = dto.FechaInicio;
            existing.FechaFin = dto.FechaFin;

            // 6️⃣ Parsear kilos y recalcular kilos ajustados
            if (!decimal.TryParse(existing.Kilos, out var kilosVal))
                throw new InvalidOperationException($"Kilos inválidos: {existing.Kilos}");

            var cultive = await _cultiveRepo.GetByIdAsync(existing.CultiveId)
                          ?? throw new KeyNotFoundException($"Cultive {existing.CultiveId} no encontrado.");
            existing.KilosAjustados = (kilosVal * (decimal)cultive.Superficie).ToString();

            // 7️⃣ Guardar y loggear
            var updated = await _productionRepo.UpdateAsync(existing);
            _logger.LogInformation("Actualizada CultiveProduction {Id}.", updated.Id);

            // 8️⃣ Recalcular el estimado en el cultivo padre
            await RecalculateAndSaveCultiveEstimadaAsync(updated.CultiveId);

            // 9️⃣ Devolver el DTO
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

            await _productionRepo.HardDeleteAsync(entity);
            _logger.LogInformation("Hard-deleted CultiveProduction {Id}.", id);

            await RecalculateAndSaveCultiveEstimadaAsync(entity.CultiveId);
            return true;
        }

        public async Task BulkRecalculateProduccionEstimadoAsync()
        {
            var allProductions = await _productionRepo.GetAllAsync();
            var allCultives = await _cultiveRepo.GetAllAsync();

            foreach (var cultive in allCultives)
            {
                var suma = allProductions
                    .Where(p => p.CultiveId == cultive.Id)
                    .Select(p => decimal.TryParse(p.KilosAjustados, out var v) ? v : 0M)
                    .Sum();

                cultive.ProduccionEstimada = (double)suma;
                await _cultiveRepo.UpdateAsync(cultive);
            }
        }

        private async Task RecalculateAndSaveCultiveEstimadaAsync(int cultiveId)
        {
            // Suma todos los kilos ajustados de las producciones de este cultivo
            var producciones = await _productionRepo.GetAllAsync();
            var total = producciones
                .Where(p => p.CultiveId == cultiveId)
                .Select(p => decimal.TryParse(p.KilosAjustados, out var v) ? v : 0M)
                .Sum();

            var cultive = await _cultiveRepo.GetByIdAsync(cultiveId);
            cultive.ProduccionEstimada = (double)total;
            await _cultiveRepo.UpdateAsync(cultive);

            _logger.LogInformation(
                "Actualizada ProduccionEstimada del Cultive {CultiveId} a {Total}.",
                cultiveId, total);
        }
    }
}
