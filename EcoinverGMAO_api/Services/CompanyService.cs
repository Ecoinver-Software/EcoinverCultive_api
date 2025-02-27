using AutoMapper;
using EcoinverGMAO_api.Data; 
using EcoinverGMAO_api.Models;
using EcoinverGMAO_api.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace EcoinverGMAO_api.Services
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync();
        Task<CompanyDto> GetCompanyByIdAsync(int id);
        Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto dto);
        Task<CompanyDto> UpdateCompanyAsync(int id, UpdateCompanyDto dto);
        Task<bool> DeleteCompanyAsync(int id);
    }
    public class CompanyService : ICompanyService
    {
        private readonly IRepository<Company> _repository;
        private readonly IMapper _mapper;

        public CompanyService(IRepository<Company> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync()
        {
            var companies = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CompanyDto>>(companies);
        }

        public async Task<CompanyDto> GetCompanyByIdAsync(int id)
        {
            // El repositorio espera un id de tipo long, por lo que se hace el cast
            var company = await _repository.GetByIdAsync(id);
            if (company == null)
                return null;
            return _mapper.Map<CompanyDto>(company);
        }

        public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto dto)
        {
            var company = _mapper.Map<Company>(dto);
            var addedCompany = await _repository.AddAsync(company);
            return _mapper.Map<CompanyDto>(addedCompany);
        }

        public async Task<CompanyDto> UpdateCompanyAsync(int id, UpdateCompanyDto dto)
        {
            var company = await _repository.GetByIdAsync(id);
            if (company == null)
                return null;
            _mapper.Map(dto, company);
            var updatedCompany = await _repository.UpdateAsync(company);
            return _mapper.Map<CompanyDto>(updatedCompany);
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var company = await _repository.GetByIdAsync(id);
            if (company == null)
                return false;
            await _repository.DeleteAsync(company);
            return true;
        }
    }
}
