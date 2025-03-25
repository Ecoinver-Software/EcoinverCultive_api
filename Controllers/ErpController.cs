using Microsoft.AspNetCore.Mvc;
using EcoinverGMAO_api.Data;               // donde tengas tu AppDbContext
using EcoinverGMAO_api.Models.Entities;    // donde esté tu entidad "Cultive"
using System.Threading.Tasks;
using System.Linq;

namespace EcoinverGMAO_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErpController : ControllerBase
    {
        private readonly ErpDataService _erpDataService;
        private readonly AppDbContext _dbContext;

        public ErpController(ErpDataService erpDataService, AppDbContext dbContext)
        {
            _erpDataService = erpDataService;
            _dbContext = dbContext;
        }

        // GET api/erp/cultives
        // Lee de la DB del ERP y, a la vez, guarda los registros en tu DB local.
        [HttpGet("cultives")]
        public async Task<IActionResult> GetCultivesAndSave()
        {
            // 1) Obtener "Cultives" (cultivos) desde el ERP
            var cultivesFromErp = _erpDataService.GetCultivosSincronizados();

            // 2) Mapear el DTO al modelo EF "Cultive"
            var newCultives = cultivesFromErp.Select(c => new Cultive
            {
                IdCultivo = c.IdCultivo,
                IdAgricultor = c.IdAgricultor,
                NombreAgricultor = c.NombreAgricultor,
                IdFinca = c.IdFinca,
                NombreFinca = c.NombreFinca,
                IdNave = c.IdNave,
                NombreNave = c.NombreNave,
                IdGenero = c.IdGenero,
                NombreGenero = c.NombreGenero,
                NombreVariedad = c.NombreVariedad,
                Superficie = c.Superficie,
                ProduccionEstimada = c.ProduccionEstimada,
                FechaSiembra = c.FechaSiembra,
                FechaFin = c.FechaFin
            }).ToList();

            // 3) Guardarlos en tu DB local:
            // Opción A: Borrar los anteriores y volver a insertar:
            // _dbContext.Cultives.RemoveRange(_dbContext.Cultives);
            // await _dbContext.SaveChangesAsync();

            // Opción B: Insertar directamente
            _dbContext.Cultives.AddRange(newCultives);
            await _dbContext.SaveChangesAsync();

            // 4) Retornar un mensaje de éxito
            return Ok(new
            {
                Message = "Cultivos sincronizados correctamente.",
                Count = newCultives.Count,
                Cultives = newCultives
            });
        }
    }
}
