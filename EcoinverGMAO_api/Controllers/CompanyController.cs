using EcoinverGMAO_api.Models.Dto;
using EcoinverGMAO_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoinverGMAO_api.Controllers
{
    [ApiController]
    [Route("api/company")]
    [Authorize]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        // GET: api/companies
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var companies = await _companyService.GetAllCompaniesAsync();
            return Ok(companies);
        }

        // GET: api/companies/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null)
                return NotFound();
            return Ok(company);
        }

        // POST: api/companies
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCompanyDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdCompany = await _companyService.CreateCompanyAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdCompany.Id }, createdCompany);
        }

        // PUT: api/companies/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCompanyDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedCompany = await _companyService.UpdateCompanyAsync(id, dto);
            if (updatedCompany == null)
                return NotFound();
            return Ok(updatedCompany);
        }

        // DELETE: api/companies/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _companyService.DeleteCompanyAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
