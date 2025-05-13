using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoinverGMAO_api.Models;
using EcoinverGMAO_api.Data;
using EcoinverGMAO_api.Models.Dto;
using EcoinverGMAO_api.Models.Dto.Create;

namespace EcoinverGMAO_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlStockDetailsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ControlStockDetailsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ControlStockDetails
        [HttpGet]
        public async Task<ActionResult> GetControlStocks()
        {
           
     var controlStock = await _context.ControlStockDetails
    .Select(x => new ControlStockDetailsDto
    {
        id=x.Id,
        NumBultos= x.numBultos,
        CodigoPartida = x.CodigoPartida,
        IdGenero = x.IdGenero,
        Categoria = x.Categoria,
        idControl = x.IdControl
    })
    .ToListAsync();



            return Ok(controlStock);
        }

        // GET: api/ControlStock/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetControlStock(int id)
        {
            var controlStock = await _context.ControlStockDetails.FindAsync(id);

            if (controlStock == null)
            {
                return NotFound();
            }
            return Ok(controlStock);
        }

        // PUT: api/ControlStockDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutControlStock(int id, CreateStockDetailsDto controlStockDto)
        {
            var buscarControl = await _context.ControlStockDetails.FindAsync(id);

            if (buscarControl == null)
            {
                return NotFound("No se ha encontrado el registro");
            }

            buscarControl.numBultos = controlStockDto.NumBultos;
            buscarControl.CodigoPartida= controlStockDto.CodigoPartida;
            buscarControl.IdGenero = controlStockDto.IdGenero;
            buscarControl.Categoria = controlStockDto.Categoria;
            buscarControl.IdControl = controlStockDto.idControl;
            await _context.SaveChangesAsync();

            return Ok("Se ha actualizado correctamente");
        }

        // POST: api/ControlStockDetails
        [HttpPost]
        public async Task<ActionResult> PostControlStock(CreateStockDetailsDto controlStockdDto)
        {
            var controlStock = new ControlStockDetails
            {
                numBultos = controlStockdDto.NumBultos,
                CodigoPartida = controlStockdDto.CodigoPartida,
                IdGenero= controlStockdDto.IdGenero,
                Categoria=controlStockdDto.Categoria,
                IdControl= controlStockdDto.idControl
            };

            _context.ControlStockDetails.Add(controlStock);
            await _context.SaveChangesAsync();

            return Ok("Creado correctamenete");
        }

        // DELETE: api/ControlStockDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteControlStock(int id)
        {
            var controlStock = await _context.ControlStockDetails.FindAsync(id);
            if (controlStock == null)
            {
                return NotFound();
            }

            _context.ControlStockDetails.Remove(controlStock);
            await _context.SaveChangesAsync();

            return Ok("Se ha eliminado correctamente");
        }


    }
}