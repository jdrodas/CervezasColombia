using CervezasColombia_CS_API_Mongo.Helpers;
using CervezasColombia_CS_API_Mongo.Models;
using CervezasColombia_CS_API_Mongo.Services;
using Microsoft.AspNetCore.Mvc;

namespace CervezasColombia_CS_API_Mongo.Controllers
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
        public async Task<IActionResult> GetDetailsByParameterAsync([FromQuery] ConsultaCerveza parametros)
        {
            //Si todos los parameros son nulos, se traen todas las cervezas
            if (string.IsNullOrEmpty(parametros.Id) &&
               string.IsNullOrEmpty(parametros.Nombre) &&
               string.IsNullOrEmpty(parametros.Cerveceria))
            {
                var lasCervezas = await _cervezaService
                    .GetAllAsync();

                return Ok(lasCervezas);
            }
            else
            {
                //De lo contrario, se trae una Cervecería por el resto de parámetros
                CervezaDetallada unaCervezaDetallada = new();
                try
                {
                    // Por Id
                    if (!string.IsNullOrEmpty(parametros.Id))
                    {

                        unaCervezaDetallada = await _cervezaService
                        .GetDetailsByIdAsync(parametros.Id);                                                
                    }

                    // Por Nombre Y Cerveceria
                    if (!string.IsNullOrEmpty(parametros.Nombre) &&
                        !string.IsNullOrEmpty(parametros.Cerveceria))
                    {

                        unaCervezaDetallada = await _cervezaService
                        .GetByNameAndBreweryAsync(parametros.Nombre, parametros.Cerveceria);
                    }
                    else
                    {
                        var lasCervezas = await _cervezaService
                            .GetAllAsync();

                        return Ok(lasCervezas);
                    }

                    return Ok(unaCervezaDetallada);
                }
                catch (AppValidationException error)
                {
                    return NotFound(error.Message);
                }
            }
        }

        [HttpGet("{cerveza_id:length(24)}")]
        public async Task<IActionResult> GetDetailsByIdAsync(string cerveza_id)
        {
            try
            {
                var unaCerveza = await _cervezaService
                    .GetDetailsByIdAsync(cerveza_id);
                return Ok(unaCerveza);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }       

        [HttpGet("{cerveza_id:length(24)}/Ingredientes")]
        public async Task<IActionResult> GetAssociatedIngredientsAsync(string cerveza_id)
        {
            try
            {
                var losIngredientesPorCerveza = await _cervezaService
                    .GetAssociatedIngredientsAsync(cerveza_id);

                return Ok(losIngredientesPorCerveza);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{cerveza_id:length(24)}/Envasados")]
        public async Task<IActionResult> GetAssociatedPackagingsAsync(string cerveza_id)
        {
            try
            {
                var losEnvasadosPorCerveza = await _cervezaService
                    .GetAssociatedPackagingsAsync(cerveza_id);

                return Ok(losEnvasadosPorCerveza);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Cerveza unaCerveza)
        {
            try
            {
                var cervezaCreada = await _cervezaService
                    .CreateAsync(unaCerveza);

                return Ok(cervezaCreada);
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

        [HttpPost("{cerveza_id:length(24)}/Envasados")]
        public async Task<IActionResult> CreateBeerPackagingAsync(string cerveza_id, EnvasadoCerveza unEnvasadoCerveza)
        {
            try
            {
                var cervezaEnvasada = await _cervezaService
                    .CreateBeerPackagingAsync(cerveza_id, unEnvasadoCerveza);

                return Ok(cervezaEnvasada);
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

        [HttpPost("{cerveza_id:length(24)}/Ingredientes")]
        public async Task<IActionResult> CreateBeerIngredientAsync(string cerveza_id, Ingrediente unIngrediente)
        {
            try
            {
                await _cervezaService
                   .CreateBeerIngredientAsync(cerveza_id, unIngrediente);

                return Ok($"Ingrediente {unIngrediente.Tipo_Ingrediente} - {unIngrediente.Nombre} asociado a esta Cerveza");
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

        [HttpPut("{cerveza_id:length(24)}")]
        public async Task<IActionResult> UpdateAsync(string cerveza_id, Cerveza unaCerveza)
        {
            try
            {
                var cervezaActualizada = await _cervezaService
                    .UpdateAsync(cerveza_id, unaCerveza);

                return Ok(cervezaActualizada);

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

        [HttpDelete("{cerveza_id:length(24)}")]
        public async Task<IActionResult> DeleteAsync(string cerveza_id)
        {
            try
            {
                await _cervezaService
                    .DeleteAsync(cerveza_id);

                return Ok($"Cerveza {cerveza_id} fue eliminada");

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

        [HttpDelete("{cerveza_id:length(24)}/Envasados")]
        public async Task<IActionResult> DeleteBeerPackagingAsync(string cerveza_id, EnvasadoCerveza unEnvasadoCerveza)
        {
            try
            {
                await _cervezaService
                    .DeleteBeerPackagingAsync(cerveza_id, unEnvasadoCerveza);

                return Ok($"Envasado {unEnvasadoCerveza.Envasado} de {unEnvasadoCerveza.Volumen} {unEnvasadoCerveza.Unidad_Volumen} fue eliminado para esta cerveza");
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

        [HttpDelete("{cerveza_id:length(24)}/Ingredientes")]
        public async Task<IActionResult> DeleteBeerIngredientAsync(string cerveza_id, Ingrediente unIngrediente)
        {
            try
            {
                await _cervezaService
                    .DeleteBeerIngredientAsync(cerveza_id, unIngrediente);

                return Ok($"Ingrediente {unIngrediente.Tipo_Ingrediente} - {unIngrediente.Nombre} fue eliminado para esta cerveza");
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
