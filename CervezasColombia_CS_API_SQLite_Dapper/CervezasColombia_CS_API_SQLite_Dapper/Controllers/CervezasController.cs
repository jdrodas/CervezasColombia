using CervezasColombia_CS_API_SQLite_Dapper.Models;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Services;
using Microsoft.AspNetCore.Mvc;

namespace CervezasColombia_CS_API_SQLite_Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CervezasController : Controller
    {
        private readonly CervezaService _cervezaService;

        public CervezasController(CervezaService cervezaService)
        {
            _cervezaService = cervezaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var lasCervezas = await _cervezaService
                .GetAllAsync();

            return Ok(lasCervezas);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var unaCerveza = await _cervezaService
                    .GetByIdAsync(id);
                return Ok(unaCerveza);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{id:int}/Ingredientes")]
        public async Task<IActionResult> GetAssociatedIngredientsAsync(int id)
        {
            try
            {
                var losIngredientesPorCerveza = await _cervezaService.
                    GetAssociatedIngredientsAsync(id);

                return Ok(losIngredientesPorCerveza);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{id:int}/Envasados")]
        public async Task<IActionResult> GetAssociatedPackagingsAsync(int id)
        {
            try
            {
                var losEnvasadosPorCerveza = await _cervezaService.
                    GetAssociatedPackagingsAsync(id);

                return Ok(losEnvasadosPorCerveza);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cerveza unaCerveza)
        {
            try
            {
                await _cervezaService.CreateAsync(unaCerveza);
                return Ok($"Cerveza {unaCerveza.Nombre} creada correctamente");
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
