using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Models;
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

        [HttpGet("{estilo_id:int}")]
        public async Task<IActionResult> GetByIdAsync(int estilo_id)
        {
            try
            {
                var unEstilo = await _estiloService
                    .GetByIdAsync(estilo_id);
                return Ok(unEstilo);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{estilo_id:int}/Cervezas")]
        public async Task<IActionResult> GetAssociatedBeersAsync(int estilo_id)
        {
            try
            {
                var lasCervezasPorEstilo = await _estiloService
                    .GetAssociatedBeersAsync(estilo_id);

                return Ok(lasCervezasPorEstilo);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Estilo unEstilo)
        {
            try
            {
                var estiloCreado = await _estiloService
                    .CreateAsync(unEstilo);

                return Ok(estiloCreado);
            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
        }

        [HttpPut("{estilo_id:int}")]
        public async Task<IActionResult> UpdateAsync(int estilo_id, Estilo unEstilo)
        {
            try
            {
                var estiloActualizado = await _estiloService
                    .UpdateAsync(estilo_id, unEstilo);

                return Ok(estiloActualizado);

            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
        }

        [HttpDelete("{estilo_id:int}")]
        public async Task<IActionResult> DeleteAsync(int estilo_id)
        {
            try
            {
                await _estiloService
                    .DeleteAsync(estilo_id);

                return Ok($"Estilo {estilo_id} fue eliminado");

            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
        }
    }
}