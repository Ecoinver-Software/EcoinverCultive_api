using EcoinverCultive_api.Models;
using EcoinverCultive_api.Models.Dto.Create;
using EcoinverGMAO_api.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoinverCultive_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VariableController : ControllerBase
    {
        public readonly AppDbContext _context;

        public VariableController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get() {
            var variable = await _context.Variable.Select(x => new
            {
                x.Id,
                x.Name,
                x.IdCultivo,
                x.FechaRegistro,
                x.Valor

            }).ToListAsync();

            return Ok(variable);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var variable = await _context.Variable.FindAsync(id);
            if (variable == null)
            {
                return NotFound(new { message = "No se ha encontrado la variable especificda" });
            }
            return Ok(variable);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateVariableDto createDto)
        {

            var variable = new Variable
            {
                Name = createDto.Name,
                IdCultivo = createDto.IdCultivo,
                FechaRegistro = createDto.FechaRegistro,
                Valor = createDto.Valor
            };
            _context.Variable.AddAsync(variable);
            await _context.SaveChangesAsync(); 
            return Ok(variable);

        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] CreateVariableDto dto)
        {
            var variable = await _context.Variable.FindAsync(id);
            if (variable == null)
            {
                return NotFound(new { message = "No se ha encontrado la variable con el id especificado" });
            }

            variable.IdCultivo = dto.IdCultivo;
            variable.Name = dto.Name;
            variable.Valor = dto.Valor;
            variable.FechaRegistro = dto.FechaRegistro;
            await _context.SaveChangesAsync();
            return Ok(variable);

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var variable = await _context.Variable.FindAsync(id);
            if (variable == null) {
                return NotFound(new { message = "No se ha encontrado la variable con el id especificado" });
            }

            _context.Variable.Remove(variable);
            await _context.SaveChangesAsync();
            return Ok(new {message="Se ha eliminado correctamente la variable"});
        }
    }

}
