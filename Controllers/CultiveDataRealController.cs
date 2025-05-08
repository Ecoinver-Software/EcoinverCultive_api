using EcoinverGMAO_api.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoinverGMAO_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CultiveDataRealController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public CultiveDataRealController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/cultivedatareal
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CultiveDataReal>>> GetAll()
        {
            var data = await _dbContext.CultiveProductionReal.ToListAsync();
            return Ok(data);
        }
}

}
