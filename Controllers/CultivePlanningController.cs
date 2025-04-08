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
    [Route("api/cultivePlannings")]
    public class CultivePlanningController : ControllerBase
    {
        private readonly ICultivePlanningService _cultivePlanningService;
        
        public CultivePlanningController(ICultivePlanningService cultivePlanningService)
        {
            _cultivePlanningService = cultivePlanningService;
        }
        
        // GET: api/cultivePlannings
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<CultivePlanningDto> cultivePlannings = await _cultivePlanningService.GetAllAsync();
            return Ok(cultivePlannings);
        }
        
        // GET: api/cultivePlannings/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            CultivePlanningDto cultivePlanning = await _cultivePlanningService.GetByIdAsync(id);
            if (cultivePlanning == null)
                return NotFound(new { message = "CultivePlanning not found." });
            return Ok(cultivePlanning);
        }
        
        // POST: api/cultivePlannings
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateCultivePlanningDto createCultivePlanningDto)
        {
            CultivePlanningDto createdCultivePlanning = await _cultivePlanningService.CreateAsync(createCultivePlanningDto);
            return CreatedAtAction(nameof(GetById), new { id = createdCultivePlanning.Id }, createdCultivePlanning);
        }

        // PUT: api/cultivePlannings/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, UpdateCultivePlanningDto updateCultivePlanningDto)
        {
            CultivePlanningDto updatedCultivePlanning = await _cultivePlanningService.UpdateAsync(id, updateCultivePlanningDto);
            if (updatedCultivePlanning == null)
                return NotFound(new { message = "CultivePlanning not found." });
            return Ok(updatedCultivePlanning);
        }
        
        // DELETE: api/cultivePlannings/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool result = await _cultivePlanningService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "CultivePlanning not found." });
            return NoContent();
        }
    }
}