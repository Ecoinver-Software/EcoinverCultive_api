using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoinverGMAO_api.Data;
using EcoinverGMAO_api.Models.Entities;
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
        [HttpGet("cultives")]
        public async Task<IActionResult> GetCultivesAndSave()
        {
            // 1) Obtener cultivos desde el ERP
            var cultivesFromErp = _erpDataService.GetCultivosSincronizados();

            // Preparamos una lista con todos los IdCultivo que vienen del ERP
            var idsEnErp = cultivesFromErp.Select(x => x.IdCultivo).ToList();

            // 2) Upsert para cultivos: insertar o actualizar según corresponda
            foreach (var dto in cultivesFromErp)
            {
                var existingCultive = await _dbContext.Cultives
                    .SingleOrDefaultAsync(c => c.IdCultivo == dto.IdCultivo);

                if (existingCultive != null)
                {
                    // Actualizar propiedades
                    existingCultive.IdAgricultor = dto.IdAgricultor;
                    existingCultive.NombreAgricultor = dto.NombreAgricultor;
                    existingCultive.IdFinca = dto.IdFinca;
                    existingCultive.NombreFinca = dto.NombreFinca;
                    existingCultive.IdNave = dto.IdNave;
                    existingCultive.NombreNave = dto.NombreNave;
                    existingCultive.IdGenero = dto.IdGenero;
                    existingCultive.NombreGenero = dto.NombreGenero;
                    existingCultive.NombreVariedad = dto.NombreVariedad;
                    existingCultive.Superficie = dto.Superficie;
                    existingCultive.ProduccionEstimada = dto.ProduccionEstimada;
                    existingCultive.FechaSiembra = dto.FechaSiembra;
                    existingCultive.FechaFin = dto.FechaFin;
                }
                else
                {
                    // Insertar nuevo registro
                    var newCultive = new Cultive
                    {
                        IdCultivo = dto.IdCultivo,
                        IdAgricultor = dto.IdAgricultor,
                        NombreAgricultor = dto.NombreAgricultor,
                        IdFinca = dto.IdFinca,
                        NombreFinca = dto.NombreFinca,
                        IdNave = dto.IdNave,
                        NombreNave = dto.NombreNave,
                        IdGenero = dto.IdGenero,
                        NombreGenero = dto.NombreGenero,
                        NombreVariedad = dto.NombreVariedad,
                        Superficie = dto.Superficie,
                        ProduccionEstimada = dto.ProduccionEstimada,
                        FechaSiembra = dto.FechaSiembra,
                        FechaFin = dto.FechaFin
                    };
                    _dbContext.Cultives.Add(newCultive);
                }
            }

            // 3) (Opcional) Eliminar los cultivos locales que ya no están en el ERP
            var cultivesToRemove = _dbContext.Cultives
                .Where(c => !idsEnErp.Contains(c.IdCultivo));
            _dbContext.Cultives.RemoveRange(cultivesToRemove);

            // 4) Guardar cambios en la base de datos
            await _dbContext.SaveChangesAsync();

            // 5) Retornar respuesta
            return Ok(new
            {
                Message = "Cultivos sincronizados correctamente (Upsert).",
                Count = cultivesFromErp.Count
            });
        }

        // GET api/erp/clients
        [HttpGet("clients")]
        public async Task<IActionResult> GetClientsAndSave()
        {
            // 1) Obtener clientes desde NetagroComer (ERP de clientes)
            var clientsFromErp = _erpDataService.GetClientsSincronizados();
            var idsEnErp = clientsFromErp.Select(x => x.ClientId).ToList();

            // 2) Upsert para clientes: insertar o actualizar según corresponda
            foreach (var dto in clientsFromErp)
            {
                var existingClient = await _dbContext.Clients
                    .SingleOrDefaultAsync(c => c.ClientId == dto.ClientId);

                if (existingClient != null)
                {
                    // Actualizar propiedades
                    existingClient.Name = dto.Name;
                    // Actualiza otras propiedades si tu entidad Client tiene más campos
                }
                else
                {
                    // Insertar nuevo registro
                    var newClient = new Client
                    {
                        ClientId = dto.ClientId,
                        Name = dto.Name
                    };
                    _dbContext.Clients.Add(newClient);
                }
            }

            // 3) (Opcional) Eliminar los clientes locales que ya no están en el ERP
            var clientsToRemove = _dbContext.Clients
                .Where(c => !idsEnErp.Contains(c.ClientId));
            _dbContext.Clients.RemoveRange(clientsToRemove);

            // 4) Guardar cambios en la base de datos
            await _dbContext.SaveChangesAsync();

            // 5) Retornar respuesta
            return Ok(new
            {
                Message = "Clientes sincronizados correctamente (Upsert).",
                Count = clientsFromErp.Count
            });
        }
    }
}
