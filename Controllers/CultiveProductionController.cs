using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcoinverGMAO_api.Models.Dto;
using EcoinverGMAO_api.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EcoinverGMAO_api.Controllers
{
    [ApiController]
    [Route("api/cultiveProductions")]
    public class CultiveProductionController : ControllerBase
    {
        private readonly ICultiveProductionService _cultiveProductionService;
        private readonly AppDbContext _context;
        public CultiveProductionController(ICultiveProductionService cultiveProductionService,AppDbContext context)
        {
            _cultiveProductionService = cultiveProductionService;
            _context = context;
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
        // PATCH: api/cultiveProductions/{id}/kilosAjustados/{value}
        [HttpPatch("{id}/{value:decimal}")]
        public async Task<IActionResult> UpdateKilosAjustados(int id, decimal value)
        {
            // Buscar la entidad
            var cultiveProduction = await _context.CultivesProduction.FindAsync(id);
            if (cultiveProduction == null)
                return NotFound(new { message = "CultiveProduction not found." });

            // Actualizar solo el campo kilosAjustados
            cultiveProduction.KilosAjustados = value.ToString();

            // Guardar los cambios
            await _context.SaveChangesAsync();

            // Devolver la entidad actualizada
            return Ok(cultiveProduction);
        }
    }
}