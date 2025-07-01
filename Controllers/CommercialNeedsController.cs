using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EcoinverGMAO_api.Models;
using EcoinverGMAO_api.Models.Dto;

namespace EcoinverGMAO_api.Controllers
{
    [ApiController]
    [Route("api/commercialneeds")]
    public class CommercialNeedsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CommercialNeedsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/commercialneeds
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var commercialNeeds = await _context.CommercialNeeds.ToListAsync();
            var commercialNeedsDto = _mapper.Map<IEnumerable<CommercialNeedsDto>>(commercialNeeds);
            return Ok(commercialNeedsDto);
        }

        // GET: api/commercialneeds/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var commercialNeed = await _context.CommercialNeeds.FindAsync(id);
            if (commercialNeed == null)
                return NotFound(new { message = "Commercial need not found." });

            var commercialNeedDto = _mapper.Map<CommercialNeedsDto>(commercialNeed);
            return Ok(commercialNeedDto);
        }

        // POST: api/commercialneeds
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommercialNeedsDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var commercialNeed = _mapper.Map<CommercialNeeds>(createDto);
            
            _context.CommercialNeeds.Add(commercialNeed);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Commercial need created successfully.",
            data = commercialNeed
            });
        }

        // PUT: api/commercialneeds/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCommercialNeedsDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var commercialNeed = await _context.CommercialNeeds.FindAsync(id);
            if (commercialNeed == null)
                return NotFound(new { message = "Commercial need not found." });

            // Mapear los campos actualizables del DTO a la entidad existente
            _mapper.Map(updateDto, commercialNeed);

            var relatedPlannings = await _context.CommercialNeedsPlanning
            .Where(p => p.IdCommercialNeed == id)
            .ToListAsync();
         
            // Actualizar los campos relevantes en cada planificación
            foreach (var planning in relatedPlannings)
            {
                // Aquí debes actualizar los campos específicos que quieres sincronizar
              
                planning.StartDate = commercialNeed.StartDate;
                planning.EndDate = commercialNeed.EndDate;
                planning.Kgs = commercialNeed.Kgs;
                

                _context.CommercialNeedsPlanning.Update(planning);
            }
            _context.CommercialNeeds.Update(commercialNeed);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Commercial need updated successfully." });
        }

        // DELETE: api/commercialneeds/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var commercialNeed = await _context.CommercialNeeds.FindAsync(id);
            if (commercialNeed == null)
                return NotFound(new { message = "Commercial need not found." });

            _context.CommercialNeeds.Remove(commercialNeed);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Commercial need deleted successfully." });
        }
    }
}
