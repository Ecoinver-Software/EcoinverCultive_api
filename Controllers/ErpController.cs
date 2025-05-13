using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcoinverGMAO_api.Data;
using EcoinverGMAO_api.Models.Dto;
using EcoinverGMAO_api.Models.Entities;

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
            var cultivesFromErp = _erpDataService.GetCultivosSincronizados();
            var idsEnErp = cultivesFromErp.Select(x => x.IdCultivo).ToList();

            foreach (var dto in cultivesFromErp)
            {
                var existing = await _dbContext.Cultives
                    .SingleOrDefaultAsync(c => c.IdCultivo == dto.IdCultivo);

                if (existing != null)
                {
                    existing.IdAgricultor = dto.IdAgricultor;
                    existing.NombreAgricultor = dto.NombreAgricultor;
                    existing.IdFinca = dto.IdFinca;
                    existing.NombreFinca = dto.NombreFinca;
                    existing.IdNave = dto.IdNave;
                    existing.NombreNave = dto.NombreNave;
                    existing.IdGenero = dto.IdGenero;
                    existing.NombreGenero = dto.NombreGenero;
                    existing.NombreVariedad = dto.NombreVariedad;
                    existing.Superficie = dto.Superficie;
                    existing.ProduccionEstimada = dto.ProduccionEstimada;
                    existing.FechaSiembra = dto.FechaSiembra;
                    existing.FechaFin = dto.FechaFin;
                    existing.Latitud = dto.Latitud;
                    existing.Longitud = dto.Longitud;
                    existing.Tecnico = dto.Tecnico;
                    existing.Provincia = dto.Provincia;
                }
                else
                {
                    _dbContext.Cultives.Add(new Cultive
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
                    });
                }
            }

            // Opcional: eliminar locales que ya no estén en ERP
            var toRemoveCultives = _dbContext.Cultives
                .Where(c => !idsEnErp.Contains(c.IdCultivo));
            _dbContext.Cultives.RemoveRange(toRemoveCultives);

            await _dbContext.SaveChangesAsync();

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
            var clientsFromErp = _erpDataService.GetClientsSincronizados();
            var idsEnErp = clientsFromErp.Select(x => x.ClientId).ToList();

            foreach (var dto in clientsFromErp)
            {
                var existing = await _dbContext.Clients
                    .SingleOrDefaultAsync(c => c.ClientId == dto.ClientId);

                if (existing != null)
                {
                    existing.Name = dto.Name;
                }
                else
                {
                    _dbContext.Clients.Add(new Client
                    {
                        ClientId = dto.ClientId,
                        Name = dto.Name
                    });
                }
            }

            var toRemoveClients = _dbContext.Clients
                .Where(c => !idsEnErp.Contains(c.ClientId));
            _dbContext.Clients.RemoveRange(toRemoveClients);

            await _dbContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Clientes sincronizados correctamente (Upsert).",
                Count = clientsFromErp.Count
            });
        }

        // GET api/erp/genders
        [HttpGet("genders")]
        public async Task<IActionResult> GetGenerosAndSave()
        {
            var generosFromErp = _erpDataService.GetGenerosSincronizados();
            var idsEnErp = generosFromErp.Select(x => x.IdGenero).ToList();

            foreach (var dto in generosFromErp)
            {
                var existing = await _dbContext.Gender
                    .SingleOrDefaultAsync(g => g.IdGenero == dto.IdGenero);

                if (existing != null)
                {
                    existing.NombreGenero = dto.NombreGenero;
                    existing.IdFamilia = dto.IdFamilia;
                    existing.NombreFamilia = dto.NombreFamilia;
                }
                else
                {
                    _dbContext.Gender.Add(new Gender
                    {
                        IdGenero = dto.IdGenero,
                        NombreGenero = dto.NombreGenero,
                        IdFamilia = dto.IdFamilia,
                        NombreFamilia = dto.NombreFamilia
                    });
                }
            }

            var toRemoveGenders = _dbContext.Gender
                .Where(g => !idsEnErp.Contains(g.IdGenero));
            _dbContext.Gender.RemoveRange(toRemoveGenders);

            await _dbContext.SaveChangesAsync();

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
            var produccionReal = _erpDataService.GetCultivosProduccionReal();
            var idsEnErp = produccionReal.Select(x => x.IdCultivo).ToList();

            foreach (var dto in produccionReal)
            {
                var existing = await _dbContext.CultiveProductionReal
                    .SingleOrDefaultAsync(r => r.IdCultivo == dto.IdCultivo);

                if (existing != null)
                {
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
                    _dbContext.CultiveProductionReal.Add(new CultiveDataReal
                    {
                        IdCultivo = dto.IdCultivo,
                        IdAgricultor = dto.IdAgricultor,
                        NombreAgricultor = dto.NombreAgricultor,
                        IdFinca = dto.IdFinca,
                        IdGenero = dto.IdGenero,
                        Superficie = dto.Superficie,
                        KilosNetos = dto.KilosNetos,
                        KilosM2 = dto.KilosM2
                    });
                }
            }

            var toRemoveProdReal = _dbContext.CultiveProductionReal
                .Where(r => !idsEnErp.Contains(r.IdCultivo));
            _dbContext.CultiveProductionReal.RemoveRange(toRemoveProdReal);

            await _dbContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Producción real sincronizada correctamente (Upsert).",
                Count = produccionReal.Count,
                Data = produccionReal
            });
        }

        /// <summary>
        /// GET api/erp/production-time
        /// </summary>
        [HttpGet("production-time")]
        public ActionResult<List<CultiveDataRealDto>> GetProduccionPorTiempo(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] int idGenero)
        {
            var produccion = _erpDataService.GetProduccionPorTiempo(fechaInicio, idGenero);

            if (produccion == null || produccion.Count == 0)
                return NoContent();

            return Ok(produccion);
        }

        /// <summary>
        /// GET api/erp/palets/partida/{idPartida}
        /// </summary>
        [HttpPut("palets/partida/{idPartida}")]
        public async Task<IActionResult> UpdatePaletsPorPartida(int idPartida)
        {
            // 1) Obtener datos del ERP
            var detalles = _erpDataService.GetPaletPorPartida(idPartida);
            if (detalles == null || !detalles.Any())
                return NotFound(new { Message = $"No se encontraron palets para la partida {idPartida} en el ERP." });

            // 2) Traer los registros locales cuyo CodigoPartida coincide
            var entidadesLocales = await _dbContext.ControlStockDetails
                .Where(x => x.CodigoPartida == idPartida)
                .ToListAsync();

            if (!entidadesLocales.Any())
                return NotFound(new { Message = $"No hay registros locales con CodigoPartida = {idPartida}." });

            // 3) Actualizar IdGenero y Categoria
            //    Aquí tomo el primer DTO, pero podrías mapear uno-a-uno si la lista coincide
            var primerDetalle = detalles.First();
            foreach (var entidad in entidadesLocales)
            {
                entidad.IdGenero = primerDetalle.IdGenero;
                entidad.Categoria = primerDetalle.Categoria;
            }

            // 4) Guardar cambios
            await _dbContext.SaveChangesAsync();

            return Ok(new
            {
                Message = $"Actualizados {entidadesLocales.Count} registros locales de la partida {idPartida}.",
                Partida = idPartida,
                IdGenero = primerDetalle.IdGenero,
                Categoria = primerDetalle.Categoria
            });
        }
    }
}
