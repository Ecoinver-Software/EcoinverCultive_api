using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EcoinverGMAO_api.Models.Dto;
using EcoinverGMAO_api.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoinverGMAO_api.Controllers
{
    [ApiController]
    [Route("api/cultives")]
    public class CultiveController : ControllerBase
    {
        private readonly ICultiveService _cultiveService;
        
        
        public CultiveController(ICultiveService cultiveService,AppDbContext context)
        {
            _cultiveService = cultiveService;
            
        }

        // GET: api/cultives
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<CultiveDto> cultives = await _cultiveService.GetAllCultivesAsync();
            return Ok(cultives);
        }

        // GET: api/cultives/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            CultiveDto cultive = await _cultiveService.GetCultiveByIdAsync(id);
            if (cultive == null)
                return NotFound(new { message = "Cultive not found." });
            return Ok(cultive);
        }

        // PATCH: api/cultives/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCultive(int id, [FromBody] UpdateCultiveDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "No data provided." });

            var updated = await _cultiveService.UpdateCultiveAsync(id, dto);
            if (updated == null)
                return NotFound(new { message = "Cultive not found." });

            return Ok(updated);
        }

        
        [HttpPatch("{id}/prod")]
        public async Task<IActionResult> UpdateProduccion(int id, [FromBody] double kilosAjustados)
        {
            var updated = await _cultiveService.UpdateProduccionEstimadaAsync(id, kilosAjustados);
            if (updated == null)
                return NotFound(new { message = "Cultive not found." });

            return Ok(updated);
        }

    }
}
