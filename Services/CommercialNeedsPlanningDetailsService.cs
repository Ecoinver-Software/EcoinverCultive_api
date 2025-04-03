using AutoMapper;
using EcoinverGMAO_api.Data;
using EcoinverGMAO_api.Models.Dto; // Ajusta el namespace de tus DTOs
using EcoinverGMAO_api.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoinverGMAO_api.Services
{
    public interface ICommercialNeedsPlanningDetailsService
    {
        Task<IEnumerable<CommercialNeedsPlanningDetailsDto>> GetAllAsync();
        Task<CommercialNeedsPlanningDetailsDto> GetByIdAsync(int id);
        Task<CommercialNeedsPlanningDetailsDto> CreateAsync(CreateCommercialNeedsPlanningDetailsDto dto);
        Task<CommercialNeedsPlanningDetailsDto> UpdateAsync(int id, UpdateCommercialNeedsPlanningDetailsDto dto);
        Task<bool> DeleteAsync(int id);
    }
    public class CommercialNeedsPlanningDetailsService : ICommercialNeedsPlanningDetailsService
    {
        private readonly IRepository<CommercialNeedsPlanningDetails> _repository;
        private readonly IMapper _mapper;

        public CommercialNeedsPlanningDetailsService(
            IRepository<CommercialNeedsPlanningDetails> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommercialNeedsPlanningDetailsDto>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CommercialNeedsPlanningDetailsDto>>(items);
        }

        public async Task<CommercialNeedsPlanningDetailsDto> GetByIdAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
                return null;

            return _mapper.Map<CommercialNeedsPlanningDetailsDto>(item);
        }

        public async Task<CommercialNeedsPlanningDetailsDto> CreateAsync(CreateCommercialNeedsPlanningDetailsDto dto)
        {
            var entity = _mapper.Map<CommercialNeedsPlanningDetails>(dto);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<CommercialNeedsPlanningDetailsDto>(created);
        }

        public async Task<CommercialNeedsPlanningDetailsDto> UpdateAsync(int id, UpdateCommercialNeedsPlanningDetailsDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return null;

            // Mapeamos el DTO sobre la entidad existente
            _mapper.Map(dto, entity);

            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<CommercialNeedsPlanningDetailsDto>(updated);
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
