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
        public async Task<List<Estilo>> GetAllEstilosAsync()
        {
            var losEstilos = await _estiloService
                .GetAllEstilosAsync();

            return losEstilos;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Estilo>> GetEstiloByIdAsync(int id)
        {
            var unEstilo = await _estiloService
                .GetEstiloByIdAsync(id);

            if (unEstilo is null || unEstilo.Id==0)
                return NotFound();

            return unEstilo;
        }
    }
}
