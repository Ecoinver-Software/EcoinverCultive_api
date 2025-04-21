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
    [Route("api/cultivePlanningsDetails")]
    public class CultivePlanningDetailsController : ControllerBase
    {
        private readonly ICultivePlanningDetailsService _cultivePlanningDetailsService;
        
        public CultivePlanningDetailsController(ICultivePlanningDetailsService cultivePlanningDetailsService)
        {
            _cultivePlanningDetailsService = cultivePlanningDetailsService;
        }
        
        // GET: api/cultivePlannings
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<CultivePlanningDetailsDto> cultivePlanningsDetails = await _cultivePlanningDetailsService.GetAllAsync();
            return Ok(cultivePlanningsDetails);
        }
        
        // GET: api/cultivePlannings/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            CultivePlanningDetailsDto cultivePlanningDetails = await _cultivePlanningDetailsService.GetByIdAsync(id);
            if (cultivePlanningDetails == null)
                return NotFound(new { message = "CultivePlanning not found." });
            return Ok(cultivePlanningDetails);
        }
        
        // POST: api/cultivePlannings
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateCultiveProductionDto createCultivePlanningDetailsDto)
        {
            CultivePlanningDetailsDto createdCultivePlanningDetails = await _cultivePlanningDetailsService.CreateAsync(createCultivePlanningDetailsDto);
            return CreatedAtAction(nameof(GetById), new { id = createdCultivePlanningDetails.CultivePlanningId }, createdCultivePlanningDetails);
        }

        // PUT: api/cultivePlannings/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, UpdateCultivePlanningDetailsDto updateCultivePlanningDetailsDto)
        {
            CultivePlanningDetailsDto updatedCultivePlanningDetails = await _cultivePlanningDetailsService.UpdateAsync(id, updateCultivePlanningDetailsDto);
            if (updatedCultivePlanningDetails == null)
                return NotFound(new { message = "CultivePlanning not found." });
            return Ok(updatedCultivePlanningDetails);
        }

        // DELETE: api/cultivePlannings/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool result = await _cultivePlanningDetailsService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "CultivePlanning not found." });
            return NoContent();
        }
    }
}