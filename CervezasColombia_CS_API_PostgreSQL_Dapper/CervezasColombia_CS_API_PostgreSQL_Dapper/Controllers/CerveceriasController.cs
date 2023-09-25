using CervezasColombia_CS_API_PostgreSQL_Dapper.Helpers;
using CervezasColombia_CS_API_PostgreSQL_Dapper.Models;
using CervezasColombia_CS_API_PostgreSQL_Dapper.Services;
using Microsoft.AspNetCore.Mvc;

namespace CervezasColombia_CS_API_PostgreSQL_Dapper.Controllers
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

        [HttpGet("{cerveceria_id:int}")]
        public async Task<IActionResult> GetByIdAsync(int cerveceria_id)
        {
            try
            {
                var unaCerveceria = await _cerveceriaService
                    .GetByIdAsync(cerveceria_id);

                return Ok(unaCerveceria);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{cerveceria_id:int}/Cervezas")]
        public async Task<IActionResult> GetAssociatedBeersAsync(int cerveceria_id)
        {
            try
            {
                var lasCervezasPorCerveceria = await _cerveceriaService
                    .GetAssociatedBeersAsync(cerveceria_id);

                return Ok(lasCervezasPorCerveceria);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Cerveceria unaCerveceria)
        {
            try
            {
                var cerveceriaCreada = await _cerveceriaService
                    .CreateAsync(unaCerveceria);

                return Ok(cerveceriaCreada);
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

        [HttpPut("{cerveceria_id:int}")]
        public async Task<IActionResult> UpdateAsync(int cerveceria_id, Cerveceria unaCerveceria)
        {
            try
            {
                var cerveceriaActualizada = await _cerveceriaService
                    .UpdateAsync(cerveceria_id, unaCerveceria);

                return Ok(cerveceriaActualizada);

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

        [HttpDelete("{cerveceria_id:int}")]
        public async Task<IActionResult> DeleteAsync(int cerveceria_id)
        {
            try
            {
                await _cerveceriaService
                    .DeleteAsync(cerveceria_id);

                return Ok($"Cervecería {cerveceria_id} fue eliminada");

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