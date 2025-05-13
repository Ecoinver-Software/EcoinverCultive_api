using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoinverGMAO_api.Models;
using EcoinverGMAO_api.Data;
using EcoinverGMAO_api.Models.Dto;

namespace EcoinverGMAO_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlStockController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ControlStockController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ControlStock
        [HttpGet]
        public async Task<ActionResult> GetControlStocks()
        {
            var controlStock = await _context.ControlStocks
    .Select(x => new
    {
        x.Id,
        x.Fecha
    })
    .ToListAsync();


            return Ok(controlStock);
        }

        // GET: api/ControlStock/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetControlStock(int id)
        {
            var controlStock = await _context.ControlStocks.FindAsync(id);

            if (controlStock==null)
            {
                return NotFound();
            }
            return Ok(controlStock);
        }

        // PUT: api/ControlStock/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutControlStock(int id, ControlStockDto controlStockDto)
        {
            var buscarControl = await _context.ControlStocks.FindAsync(id);

            if (buscarControl==null)
            {
                return NotFound("No se ha encontrado el registro");
            }

            buscarControl.Fecha = controlStockDto.Fecha;
          
           
            await _context.SaveChangesAsync();

            return Ok("Se ha actualizado correctamente");
        }

        // POST: api/ControlStock
        [HttpPost]
        public async Task<ActionResult> PostControlStock(ControlStockDto controlStockdDto)
        {
            var controlStock = new ControlStock
            {
                Fecha = controlStockdDto.Fecha,
              
                
            };

            _context.ControlStocks.Add(controlStock);
            await _context.SaveChangesAsync();

            return Ok(new {message= "Creado correctamenete" });
        }

        // DELETE: api/ControlStock/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteControlStock(int id)
        {   
            var controlStock = await _context.ControlStocks.FindAsync(id);
            if (controlStock == null)
            {
                return NotFound();
            }

            _context.ControlStocks.Remove(controlStock);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Se ha eliminado correctamente" });
        }

      
    }
}