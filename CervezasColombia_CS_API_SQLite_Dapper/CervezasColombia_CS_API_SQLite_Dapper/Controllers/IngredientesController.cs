using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Models;
using CervezasColombia_CS_API_SQLite_Dapper.Services;
using Microsoft.AspNetCore.Mvc;

namespace CervezasColombia_CS_API_SQLite_Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientesController : Controller
    {
        private readonly IngredienteService _ingredienteService;

        public IngredientesController(IngredienteService ingredienteService)
        {
            _ingredienteService = ingredienteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var losIngredientes = await _ingredienteService
                .GetAllAsync();

            return Ok(losIngredientes);
        }

        [HttpGet("{ingrediente_id:int}")]
        public async Task<IActionResult> GetByIdAsync(int ingrediente_id)
        {
            try
            {
                var unIngrediente = await _ingredienteService
                    .GetByIdAsync(ingrediente_id);

                return Ok(unIngrediente);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{ingrediente_id:int}/Cervezas")]
        public async Task<IActionResult> GetAssociatedBeersAsync(int ingrediente_id)
        {
            try
            {
                var lasCervezasPorIngrediente = await _ingredienteService
                    .GetAssociatedBeersAsync(ingrediente_id);

                return Ok(lasCervezasPorIngrediente);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Ingrediente unIngrediente)
        {
            try
            {
                var ingredienteCreado = await _ingredienteService
                    .CreateAsync(unIngrediente);

                return Ok(ingredienteCreado);
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

        [HttpPut("{ingrediente_id:int}")]
        public async Task<IActionResult> UpdateAsync(int ingrediente_id, Ingrediente unIngrediente)
        {
            try
            {
                var ingredienteActualizado = await _ingredienteService
                    .UpdateAsync(ingrediente_id, unIngrediente);

                return Ok(ingredienteActualizado);
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

        [HttpDelete("{ingrediente_id:int}")]
        public async Task<IActionResult> DeleteAsync(int ingrediente_id)
        {
            try
            {
                await _ingredienteService
                    .DeleteAsync(ingrediente_id);

                return Ok($"Envasado {ingrediente_id} fue eliminado");

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