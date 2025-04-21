using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcoinverGMAO_api.Models.Dto;
using EcoinverGMAO_api.Services;

namespace EcoinverGMAO_api.Controllers
{
    [ApiController]
    [Route("api/cultiveProductions")]
    public class CultiveProductionController : ControllerBase
    {
        private readonly ICultiveProductionService _cultiveProductionService;

        public CultiveProductionController(ICultiveProductionService cultiveProductionService)
        {
            _cultiveProductionService = cultiveProductionService;
        }

        // GET: api/cultiveProductions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<CultiveProductionDto> productions = await _cultiveProductionService.GetAllAsync();
            return Ok(productions);
        }

        // GET: api/cultiveProductions/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var production = await _cultiveProductionService.GetByIdAsync(id);
            if (production == null)
                return NotFound(new { message = "CultiveProduction not found." });
            return Ok(production);
        }

        // POST: api/cultiveProductions
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCultiveProductionDto dto)
        {
            var created = await _cultiveProductionService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/cultiveProductions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateCultiveProductionDto dto)
        {
            var updated = await _cultiveProductionService.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound(new { message = "CultiveProduction not found." });
            return Ok(updated);
        }

        // DELETE: api/cultiveProductions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool result = await _cultiveProductionService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "CultiveProduction not found." });
            return NoContent();
        }
    }
}