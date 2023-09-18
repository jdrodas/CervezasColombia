using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Services;
using Microsoft.AspNetCore.Mvc;

namespace CervezasColombia_CS_API_SQLite_Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnvasadosController : Controller
    {
        private readonly EnvasadoService _envasadoService;

        public EnvasadosController(EnvasadoService envasadoService)
        {
            _envasadoService = envasadoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var losEnvasados = await _envasadoService
                .GetAllAsync();

            return Ok(losEnvasados);
        }

        [HttpGet("{envasado_id:int}")]
        public async Task<IActionResult> GetByIdAsync(int envasado_id)
        {
            try
            {
                var unEnvasado = await _envasadoService
                    .GetByIdAsync(envasado_id);

                return Ok(unEnvasado);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{envasado_id:int}/Cervezas")]
        public async Task<IActionResult> GetAssociatedBeersAsync(int envasado_id)
        {
            try
            {
                var lasCervezasPorEnvasado = await _envasadoService
                    .GetAssociatedBeersAsync(envasado_id);

                return Ok(lasCervezasPorEnvasado);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }
    }
}