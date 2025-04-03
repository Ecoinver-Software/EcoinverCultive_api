using EcoinverGMAO_api.Models.Dto;                // Ajusta al namespace de tus DTOs
using EcoinverGMAO_api.Services;                  // Ajusta al namespace de tu servicio
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoinverGMAO_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommercialNeedsPlanningDetailsController : ControllerBase
    {
        private readonly ICommercialNeedsPlanningDetailsService _service;

        public CommercialNeedsPlanningDetailsController(ICommercialNeedsPlanningDetailsService service)
        {
            _service = service;
        }

        // GET: api/commercialneedsplanningdetails
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        // GET: api/commercialneedsplanningdetails/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound(new { message = "CommercialNeedsPlanningDetails not found." });

            return Ok(item);
        }

        // POST: api/commercialneedsplanningdetails
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommercialNeedsPlanningDetailsDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(createDto);
            return Ok(new
            {
                message = "CommercialNeedsPlanningDetails created successfully.",
                data = created
            });
        }

        // PUT: api/commercialneedsplanningdetails/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCommercialNeedsPlanningDetailsDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, updateDto);
            if (updated == null)
                return NotFound(new { message = "CommercialNeedsPlanningDetails not found." });

            return Ok(new
            {
                message = "CommercialNeedsPlanningDetails updated successfully.",
                data = updated
            });
        }

        // DELETE: api/commercialneedsplanningdetails/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = "CommercialNeedsPlanningDetails not found." });

            return Ok(new { message = "CommercialNeedsPlanningDetails deleted successfully." });
        }
    }
}
