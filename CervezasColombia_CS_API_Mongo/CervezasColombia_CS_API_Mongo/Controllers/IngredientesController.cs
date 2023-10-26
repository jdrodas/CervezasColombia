using CervezasColombia_CS_API_Mongo.Helpers;
using CervezasColombia_CS_API_Mongo.Models;
using CervezasColombia_CS_API_Mongo.Services;
using Microsoft.AspNetCore.Mvc;

namespace CervezasColombia_CS_API_Mongo.Controllers
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

        [HttpGet("{ingrediente_id:length(24)}")]
        public async Task<IActionResult> GetByIdAsync(string ingrediente_id)
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

        [HttpGet("{ingrediente_id:length(24)}/Cervezas")]
        public async Task<IActionResult> GetAssociatedBeersAsync(string ingrediente_id)
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

        [HttpPut("{ingrediente_id:length(24)}")]
        public async Task<IActionResult> UpdateAsync(string ingrediente_id, Ingrediente unIngrediente)
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

        [HttpDelete("{ingrediente_id:length(24)}")]
        public async Task<IActionResult> DeleteAsync(string ingrediente_id)
        {
            try
            {
                await _ingredienteService
                    .DeleteAsync(ingrediente_id);

                return Ok($"Ingrediente {ingrediente_id} fue eliminado");

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