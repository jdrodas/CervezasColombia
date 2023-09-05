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
        public async Task<List<Estilo>> GetAllIngredientes()
        {
            var losEstilos = await _estiloService
                .GetAllEstilosAsync();

            return losEstilos;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Estilo>> GetEstilo(int id)
        {
            var unEstilo = await _estiloService
                .GetEstiloAsync(id);

            if (unEstilo is null)
                return NotFound();

            return unEstilo;
        }
    }
}
