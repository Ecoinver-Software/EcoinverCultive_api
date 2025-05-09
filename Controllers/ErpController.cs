using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoinverGMAO_api.Data;
using EcoinverGMAO_api.Models.Entities;
using System.Threading.Tasks;
using System.Linq;
using EcoinverGMAO_api.Models.Dto;

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
                    // Actualizar propiedades existentes
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
                    existingCultive.Latitud = dto.Latitud;
                    existingCultive.Longitud = dto.Longitud;
                    existingCultive.Tecnico = dto.Tecnico;
                    existingCultive.Provincia = dto.Provincia;


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
                        FechaFin = dto.FechaFin,
                        Latitud = dto.Latitud,
                        Longitud = dto.Longitud,
                        Tecnico = dto.Tecnico,
                        Provincia = dto.Provincia
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

        // GET api/erp/generos
        [HttpGet("genders")]
        public async Task<IActionResult> GetGenerosAndSave()
        {
            // 1) Obtener géneros desde NetagroComer (ERP de géneros)
            var generosFromErp = _erpDataService.GetGenerosSincronizados();
            var idsEnErp = generosFromErp.Select(x => x.IdGenero).ToList();

            // 2) Upsert para géneros: insertar o actualizar según corresponda
            foreach (var dto in generosFromErp)
            {
                var existingGenero = await _dbContext.Gender
                    .SingleOrDefaultAsync(g => g.IdGenero == dto.IdGenero);

                if (existingGenero != null)
                {
                    // Actualizar propiedades
                    existingGenero.NombreGenero = dto.NombreGenero;
                    existingGenero.IdFamilia = dto.IdFamilia;
                    existingGenero.NombreFamilia = dto.NombreFamilia;
                }
                else
                {
                    // Insertar nuevo registro
                    var newGenero = new Gender
                    {
                        IdGenero = dto.IdGenero,
                        NombreGenero = dto.NombreGenero,
                        IdFamilia = dto.IdFamilia,
                        NombreFamilia = dto.NombreFamilia
                    };
                    _dbContext.Gender.Add(newGenero);
                }
            }

            // 3) (Opcional) Eliminar los géneros locales que ya no están en el ERP
            var generosToRemove = _dbContext.Gender
                .Where(g => !idsEnErp.Contains(g.IdGenero));
            _dbContext.Gender.RemoveRange(generosToRemove);

            // 4) Guardar cambios en la base de datos
            await _dbContext.SaveChangesAsync();

            // 5) Retornar respuesta
            return Ok(new
            {
                Message = "Géneros sincronizados correctamente (Upsert).",
                Count = generosFromErp.Count
            });
        }
        // GET api/erp/production-real
        [HttpGet("production-real")]
        public async Task<IActionResult> GetCultivesProductionRealAndSave()
        {
            // 1) Obtener los datos reales de producción desde el ERP
            var produccionReal = _erpDataService.GetCultivosProduccionReal();
            var idsEnErp = produccionReal.Select(x => x.IdCultivo).ToList();

            // 2) Upsert en la tabla CultiveDataReal
            foreach (var dto in produccionReal)
            {
                var existing = await _dbContext.CultiveProductionReal
                    .SingleOrDefaultAsync(r => r.IdCultivo == dto.IdCultivo);

                if (existing != null)
                {
                    // actualizar
                    existing.IdAgricultor = dto.IdAgricultor;
                    existing.NombreAgricultor = dto.NombreAgricultor;
                    existing.IdFinca = dto.IdFinca;
                    existing.IdGenero = dto.IdGenero;
                    existing.Superficie = dto.Superficie;
                    existing.KilosNetos = dto.KilosNetos;
                    existing.KilosM2 = dto.KilosM2;
                }
                else
                {
                    // insertar
                    var nueva = new CultiveDataReal
                    {
                        IdCultivo = dto.IdCultivo,
                        IdAgricultor = dto.IdAgricultor,
                        NombreAgricultor = dto.NombreAgricultor,
                        IdFinca = dto.IdFinca,
                        IdGenero = dto.IdGenero,
                        Superficie = dto.Superficie,
                        KilosNetos = dto.KilosNetos,
                        KilosM2 = dto.KilosM2
                    };
                    _dbContext.CultiveProductionReal.Add(nueva);
                }
            }

            // 3) Eliminar los registros locales que ya no existen en el ERP (opcional)
            var toRemove = _dbContext.CultiveProductionReal
                .Where(r => !idsEnErp.Contains(r.IdCultivo));
            _dbContext.CultiveProductionReal.RemoveRange(toRemove);

            // 4) Guardar
            await _dbContext.SaveChangesAsync();

            // 5) Devolver al cliente
            return Ok(new
            {
                Message = "Producción real sincronizada correctamente (Upsert).",
                Count = produccionReal.Count,
                Data = produccionReal
            });
        }
        /// <summary>
        /// GET api/erp/production-time
        /// Obtiene la producción total de los cultivos para un género dado en un rango de fechas.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio del rango (yyyy-MM-dd)</param>
        /// <param name="fechaFin">Fecha de fin del rango (yyyy-MM-dd)</param>
        /// <param name="idGenero">Identificador del género a filtrar</param>
        /// <returns>Lista de CultivoProduccionPorTiempoDto</returns>
        [HttpGet("production-time")]
        public ActionResult<List<CultiveDataRealDto>> GetProduccionPorTiempo(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] int idGenero)
        {
            // 1) Ejecuta la consulta en el servicio
            var produccion = _erpDataService.GetProduccionPorTiempo(fechaInicio,idGenero);

            // 2) Si no hay datos, devuelve 204 No Content (opcional)
            if (produccion == null || produccion.Count == 0)
            {
                return NoContent();
            }

            // 3) Devuelve directamente la lista
            return Ok(produccion);
        }


    }
}
