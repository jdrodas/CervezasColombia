using CervezasColombia_CS_API_SQLite_Dapper.Models;
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var unEnvasado = await _envasadoService
                    .GetByIdAsync(id);

                return Ok(unEnvasado);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{id:int}/Cervezas")]
        public async Task<IActionResult> GetAssociatedBeersAsync(int id)
        {
            try
            {
                var lasCervezasPorEnvasado = await _envasadoService.
                    GetAssociatedBeersAsync(id);

                return Ok(lasCervezasPorEnvasado);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }
    }
}