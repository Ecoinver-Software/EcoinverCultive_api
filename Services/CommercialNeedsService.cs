using AutoMapper;
using EcoinverGMAO_api.Data;
using EcoinverGMAO_api.Models;
using EcoinverGMAO_api.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoinverGMAO_api.Services
{
    public interface ICommercialNeedsService
    {
        Task<IEnumerable<CommercialNeedsDto>> GetAllCommercialNeedsAsync();
        Task<CommercialNeedsDto> GetCommercialNeedsByIdAsync(int id);
        Task<CommercialNeedsDto> CreateCommercialNeedsAsync(CreateCommercialNeedsDto dto);
        Task<CommercialNeedsDto> UpdateCommercialNeedsAsync(int id, UpdateCommercialNeedsDto dto);
        Task<bool> DeleteCommercialNeedsAsync(int id);
    }

    public class CommercialNeedsService : ICommercialNeedsService
    {
        private readonly IRepository<CommercialNeeds> _repository;
        private readonly IMapper _mapper;

        public CommercialNeedsService(IRepository<CommercialNeeds> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommercialNeedsDto>> GetAllCommercialNeedsAsync()
        {
            var commercialNeeds = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CommercialNeedsDto>>(commercialNeeds);
        }

        public async Task<CommercialNeedsDto> GetCommercialNeedsByIdAsync(int id)
        {
            var commercialNeed = await _repository.GetByIdAsync(id);
            if (commercialNeed == null)
                return null;
            return _mapper.Map<CommercialNeedsDto>(commercialNeed);
        }

        public async Task<CommercialNeedsDto> CreateCommercialNeedsAsync(CreateCommercialNeedsDto dto)
        {
            var commercialNeed = _mapper.Map<CommercialNeeds>(dto);
            var addedCommercialNeed = await _repository.AddAsync(commercialNeed);
            return _mapper.Map<CommercialNeedsDto>(addedCommercialNeed);
        }

        public async Task<CommercialNeedsDto> UpdateCommercialNeedsAsync(int id, UpdateCommercialNeedsDto dto)
        {
            var commercialNeed = await _repository.GetByIdAsync(id);
            if (commercialNeed == null)
                return null;
            _mapper.Map(dto, commercialNeed);
            var updatedCommercialNeed = await _repository.UpdateAsync(commercialNeed);
            return _mapper.Map<CommercialNeedsDto>(updatedCommercialNeed);
        }

        public async Task<bool> DeleteCommercialNeedsAsync(int id)
        {
            var commercialNeed = await _repository.GetByIdAsync(id);
            if (commercialNeed == null)
                return false;
            await _repository.DeleteAsync(commercialNeed);
            return true;
        }
    }
}
