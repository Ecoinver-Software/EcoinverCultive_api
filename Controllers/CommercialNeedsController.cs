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
            var commercialNeeds = await _context.CommercialNeeds
      .Include(c => c.Genero)  // Asegura que se traiga la relación
      .Select(c => new CommercialNeedsDto
      {
          Id = c.Id,
          ClientCode = c.ClientCode,
          ClientName = c.ClientName,
          StartDate = c.StartDate,
          EndDate = c.EndDate,
          Kgs = c.Kgs,
          GeneroId = c.GeneroId,
          GeneroName = c.Genero != null ? c.Genero.NombreGenero : null  // Evita nulos
      })
      .ToListAsync();
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

            // Buscar el género en la base de datos usando el valor de GeneroId del DTO.
            // Aquí se asume que en la entidad Gender, la propiedad que queremos usar es IdGenero.
            var genero = await _context.Gender.FirstOrDefaultAsync(g => g.IdGenero == createDto.GeneroId);
            if (genero == null)
                return BadRequest(new { message = "El género con el ID proporcionado no existe." });

            // Mapear el DTO a la entidad CommercialNeeds
            var commercialNeed = _mapper.Map<CommercialNeeds>(createDto);

            // Asignar explícitamente la relación:
            commercialNeed.GeneroId = createDto.GeneroId;
            commercialNeed.Genero = genero;
            commercialNeed.GeneroNombre = genero.NombreGenero; // Asigna el nombre del género

            _context.CommercialNeeds.Add(commercialNeed);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Commercial need created successfully." });
        }


        // PUT: api/commercialneeds/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCommercialNeedsDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var commercialNeed = await _context.CommercialNeeds.FindAsync(id);
            if (commercialNeed == null)
                return NotFound(new { message = "La necesidad comercial no existe." });

            // Actualizar los campos básicos
            commercialNeed.ClientCode = updateDto.ClientCode;
            commercialNeed.ClientName = updateDto.ClientName;
            if (updateDto.StartDate.HasValue)
                commercialNeed.StartDate = updateDto.StartDate.Value;
            commercialNeed.EndDate = updateDto.EndDate;
            if (updateDto.Kgs.HasValue)
                commercialNeed.Kgs = updateDto.Kgs.Value;

            // Si se envía un nuevo GeneroId, actualizar la relación de género
            if (updateDto.GeneroId!=0)
            {
                var genero = await _context.Gender.FirstOrDefaultAsync(g => g.IdGenero == updateDto.GeneroId);
                if (genero == null)
                    return BadRequest(new { message = "El género con el ID proporcionado no existe." });

                commercialNeed.GeneroId = genero.IdGenero;
                commercialNeed.Genero = genero;
                commercialNeed.GeneroNombre = genero.NombreGenero;
            }

            _context.CommercialNeeds.Update(commercialNeed);
            await _context.SaveChangesAsync();

            return Ok(new { message = "La necesidad comercial se actualizó correctamente." });
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
