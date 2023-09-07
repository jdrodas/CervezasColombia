using CervezasColombia_CS_API_SQLite_Dapper.Data.Entities;
using CervezasColombia_CS_API_SQLite_Dapper.Services;
using Microsoft.AspNetCore.Mvc;

namespace CervezasColombia_CS_API_SQLite_Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstilosController : Controller
    {
        private readonly EstiloService _estiloService;

        public EstilosController(EstiloService estiloService)
        {
            _estiloService = estiloService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var losEstilos = await _estiloService
                .GetAllAsync();

            return Ok(losEstilos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var unEstilo = await _estiloService
                    .GetByIdAsync(id);
                return Ok(unEstilo);
            }
            catch (KeyNotFoundException error)
            {
                return BadRequest(error.Message);
            }
        }
    }
}
