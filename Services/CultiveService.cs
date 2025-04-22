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
        Task<CultiveDto> UpdateCultiveAsync(int id, UpdateCultiveDto dto);
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

        public async Task<CultiveDto> UpdateCultiveAsync(int id, UpdateCultiveDto dto)
        {
            // 1) Recuperar la entidad
            var cultive = await _repository.GetByIdAsync(id);
            if (cultive == null)
                return null;            // aquí devuelves un CultiveDto nulo, no un Task

            // 2) Mapear los cambios del DTO sobre la entidad
            _mapper.Map(dto, cultive);

            // 3) Persistir la actualización
            await _repository.UpdateAsync(cultive);

            // 4) Devolver el DTO resultante
            return _mapper.Map<CultiveDto>(cultive);
        }
    }
}
