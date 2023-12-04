using Microsoft.AspNetCore.Mvc;

namespace CervezasColombia_CS_API_SQLite_Dapper.Resumen
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumenController(ResumenService resumenService) : Controller
    {
        private readonly ResumenService _resumenService = resumenService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var elResumen = await _resumenService
                .GetAllAsync();

            return Ok(elResumen);
        }
    }
}
