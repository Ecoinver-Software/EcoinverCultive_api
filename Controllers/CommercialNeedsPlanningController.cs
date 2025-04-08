using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EcoinverGMAO_api.Models;
using EcoinverGMAO_api.Models.Dto;
using EcoinverGMAO_api.Models.Entities;

namespace EcoinverGMAO_api.Controllers
{
    [ApiController]
    [Route("api/commercialneedsplanning")]
    public class CommercialNeedsPlanningController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CommercialNeedsPlanningController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/commercialneedsplanning
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _context.CommercialNeedsPlanning.ToListAsync();
            var itemsDto = _mapper.Map<IEnumerable<CommercialNeedsPlanningDto>>(items);
            return Ok(itemsDto);
        }

        // GET: api/commercialneedsplanning/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.CommercialNeedsPlanning.FindAsync(id);
            if (item == null)
                return NotFound(new { message = "Commercial Needs Planning not found." });

            var itemDto = _mapper.Map<CommercialNeedsPlanningDto>(item);
            return Ok(itemDto);
        }

        // POST: api/commercialneedsplanning
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommercialNeedsPlanningDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validar que el idCommercialNeed existe en la BD
            var exists = await _context.CommercialNeeds.AnyAsync(c => c.Id == createDto.IdCommercialNeed);
            if (!exists)
                return BadRequest(new { message = "El ID de necesidad comercial no existe." });

            // Mapear y asignar el IdCommercialNeed
            var entity = _mapper.Map<CommercialNeedsPlanning>(createDto);
            entity.IdCommercialNeed = createDto.IdCommercialNeed; // Asegurar que el ID está presente

            _context.CommercialNeedsPlanning.Add(entity);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Commercial Needs Planning created successfully." ,
                entity

            
            });
        }


        // PUT: api/commercialneedsplanning/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCommercialNeedsPlanningDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await _context.CommercialNeedsPlanning.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Commercial Needs Planning not found." });

            _mapper.Map(updateDto, entity);
            _context.CommercialNeedsPlanning.Update(entity);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Commercial Needs Planning updated successfully." });
        }

        // DELETE: api/commercialneedsplanning/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.CommercialNeedsPlanning.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Commercial Needs Planning not found." });

            _context.CommercialNeedsPlanning.Remove(entity);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Commercial Needs Planning deleted successfully." });
        }
    }
}
