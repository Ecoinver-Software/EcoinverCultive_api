using Microsoft.AspNetCore.Mvc;
using EcoinverGMAO_api.Data;               // Ubicación de tu AppDbContext
using EcoinverGMAO_api.Models.Entities;    // Donde están tus entidades (Cultive y Client)
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
        // Lee de la DB del ERP y, a la vez, guarda los registros de cultivos en tu DB local.
        [HttpGet("cultives")]
        public async Task<IActionResult> GetCultivesAndSave()
        {
            // 1) Obtener cultivos desde el ERP
            var cultivesFromErp = _erpDataService.GetCultivosSincronizados();

            // 2) Mapear cada DTO al modelo EF "Cultive"
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

            // 3) Sincronización total: elimina los registros existentes y guarda los nuevos
            _dbContext.Cultives.RemoveRange(_dbContext.Cultives);
            await _dbContext.SaveChangesAsync();

            _dbContext.Cultives.AddRange(newCultives);
            await _dbContext.SaveChangesAsync();

            // 4) Retornar respuesta
            return Ok(new
            {
                Message = "Cultivos sincronizados correctamente.",
                Count = newCultives.Count,
                Cultives = newCultives
            });
        }

        // GET api/erp/clients
        // Lee de la base de datos NetagroComer y guarda los registros de clientes en tu DB local.
        [HttpGet("clients")]
        public async Task<IActionResult> GetClientsAndSave()
        {
            // 1) Obtener clientes desde NetagroComer (ERP de clientes)
            var clientsFromErp = _erpDataService.GetClientsSincronizados();

            // 2) Mapear cada DTO al modelo EF "Client"
            var newClients = clientsFromErp.Select(c => new Client
            {
                ClientId = c.ClientId,
                Name = c.Name,
              // Mapea otras propiedades si tu entidad Client tiene más campos
            }).ToList();

            // 3) Sincronización total: elimina todos los clientes actuales y guarda los nuevos
            _dbContext.Clients.RemoveRange(_dbContext.Clients);
            await _dbContext.SaveChangesAsync();

            _dbContext.Clients.AddRange(newClients);
            await _dbContext.SaveChangesAsync();

            // 4) Retornar respuesta
            return Ok(new
            {
                Message = "Clientes sincronizados correctamente.",
                Count = newClients.Count,
                Clients = newClients
            });
        }
    }
}
