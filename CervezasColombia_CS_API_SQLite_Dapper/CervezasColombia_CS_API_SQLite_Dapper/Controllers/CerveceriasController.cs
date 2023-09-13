using CervezasColombia_CS_API_SQLite_Dapper.Models;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Services;
using Microsoft.AspNetCore.Mvc;

namespace CervezasColombia_CS_API_SQLite_Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CerveceriasController : Controller
    {
        private readonly CerveceriaService _cerveceriaService;

        public CerveceriasController(CerveceriaService cerveceriaService)
        {
            _cerveceriaService = cerveceriaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var lasCervecerias = await _cerveceriaService
                .GetAllAsync();

            return Ok(lasCervecerias);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var unaCerveceria = await _cerveceriaService
                    .GetByIdAsync(id);
                
                return Ok(unaCerveceria);
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
                var lasCervezasPorCerveceria = await _cerveceriaService.
                    GetAssociatedBeersAsync(id);

                return Ok(lasCervezasPorCerveceria);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cerveceria unaCerveceria)
        {
            try
            {
                await _cerveceriaService.CreateAsync(unaCerveceria);
                return Ok($"Cervecería {unaCerveceria.Nombre} creada correctamente");
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
        public async Task<IActionResult> UpdateAsync(int id, Cerveceria unaCerveceria)
        {
            try
            {
                await _cerveceriaService.UpdateAsync(id, unaCerveceria);
                return Ok($"Cervecería {unaCerveceria.Id} actualizada correctamente");

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
                await _cerveceriaService.DeleteAsync(id);
                return Ok($"Cervecería {id} fue eliminada");

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