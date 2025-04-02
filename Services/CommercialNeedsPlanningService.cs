using AutoMapper;
using EcoinverGMAO_api.Data;
using EcoinverGMAO_api.Models.Entities;
using EcoinverGMAO_api.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoinverGMAO_api.Services
{
    public interface ICommercialNeedsPlanningService
    {
        Task<IEnumerable<CommercialNeedsPlanningDto>> GetAllAsync();
        Task<CommercialNeedsPlanningDto> GetByIdAsync(int id);
        Task<CommercialNeedsPlanningDto> CreateAsync(CreateCommercialNeedsPlanningDto dto);
        Task<CommercialNeedsPlanningDto> UpdateAsync(int id, UpdateCommercialNeedsPlanningDto dto);
        Task<bool> DeleteAsync(int id);
    }

    public class CommercialNeedsPlanningService : ICommercialNeedsPlanningService
    {
        private readonly IRepository<CommercialNeedsPlanning> _repository;
        private readonly IMapper _mapper;

        public CommercialNeedsPlanningService(IRepository<CommercialNeedsPlanning> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommercialNeedsPlanningDto>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CommercialNeedsPlanningDto>>(items);
        }

        public async Task<CommercialNeedsPlanningDto> GetByIdAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
                return null;

            return _mapper.Map<CommercialNeedsPlanningDto>(item);
        }

        public async Task<CommercialNeedsPlanningDto> CreateAsync(CreateCommercialNeedsPlanningDto dto)
        {
            var entity = _mapper.Map<CommercialNeedsPlanning>(dto);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<CommercialNeedsPlanningDto>(created);
        }

        public async Task<CommercialNeedsPlanningDto> UpdateAsync(int id, UpdateCommercialNeedsPlanningDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return null;

            // Mapeamos el dto sobre la entidad existente.
            _mapper.Map(dto, entity);
            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<CommercialNeedsPlanningDto>(updated);
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
