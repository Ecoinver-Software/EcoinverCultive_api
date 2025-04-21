using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcoinverGMAO_api.Models.Dto;
using EcoinverGMAO_api.Services;

namespace EcoinverGMAO_api.Controllers
{
    [ApiController]
    [Route("api/cultivePlanningsDetails")]
    public class CultivePlanningDetailsController : ControllerBase
    {
        private readonly ICultivePlanningDetailsService _service;

        public CultivePlanningDetailsController(ICultivePlanningDetailsService service)
        {
            _service = service;
        }

        // GET: api/cultivePlanningsDetails
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<CultivePlanningDetailsDto> all = await _service.GetAllAsync();
            return Ok(all);
        }

        // GET: api/cultivePlanningsDetails/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            CultivePlanningDetailsDto dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound(new { message = "Detalle no encontrado." });

            return Ok(dto);
        }

        // POST: api/cultivePlanningsDetails
        [HttpPost]
        public async Task<IActionResult> CreateAsync(
            [FromBody] CreateCultivePlanningDetailsDto createCultivePlanningDetailsDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            CultivePlanningDetailsDto created =
                await _service.CreateAsync(createCultivePlanningDetailsDto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created
            );
        }

        // PUT: api/cultivePlanningsDetails/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(
            int id,
            [FromBody] UpdateCultivePlanningDetailsDto updateCultivePlanningDetailsDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            CultivePlanningDetailsDto updated =
                await _service.UpdateAsync(id, updateCultivePlanningDetailsDto);

            if (updated == null)
                return NotFound(new { message = "Detalle no encontrado." });

            return Ok(updated);
        }

        // DELETE: api/cultivePlanningsDetails/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = "Detalle no encontrado." });

            return NoContent();
        }
    }
}
