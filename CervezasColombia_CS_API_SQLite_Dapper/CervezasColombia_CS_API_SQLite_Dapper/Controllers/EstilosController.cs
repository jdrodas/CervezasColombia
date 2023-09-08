using CervezasColombia_CS_API_SQLite_Dapper.Data.Entities;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
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
            catch (AppValidationException error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet("{id:int}/Cervezas")]
        public async Task<IActionResult> GetBeersByStyleAsync(int id)
        {
            try 
            {
                var lasCervezasPorEstilo = await _estiloService.
                    GetBeersByStyleAsync(id);

                return Ok(lasCervezasPorEstilo);
            }
            catch (AppValidationException error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Estilo unEstilo)
        {
            try
            {
                await _estiloService.CreateAsync(unEstilo);
                return Ok(new { message = $"Estilo {unEstilo.Nombre} creado correctamente" });
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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, Estilo unEstilo)
        {
            try
            {
                await _estiloService.UpdateAsync(id, unEstilo);
                return Ok(new { message = $"Estilo {unEstilo.Id} actualizado al nombre {unEstilo.Nombre}" });

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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var estiloEliminado = await _estiloService.DeleteAsync(id);
                return Ok(new { message = $"Estilo {estiloEliminado.Id} " +
                    $"con nombre {estiloEliminado.Nombre} fue eliminado" });

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