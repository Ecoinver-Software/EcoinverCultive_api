using Microsoft.AspNetCore.Mvc;

namespace EcoinverGMAO_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErpController : ControllerBase
    {
        private readonly ErpDataService _erpDataService;

        public ErpController(ErpDataService erpDataService)
        {
            _erpDataService = erpDataService;
        }

        // GET api/erp/cultivos
        [HttpGet("cultivos")]
        public IActionResult GetCultivos()
        {
            var cultivos = _erpDataService.GetCultivos();
            return Ok(cultivos);
        }
    }
}
