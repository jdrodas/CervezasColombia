using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CervezasColombia_CS_API_SQLite_Dapper.Ingredientes
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientesController(IngredienteService ingredienteService) : Controller
    {
        private readonly IngredienteService _ingredienteService = ingredienteService;

        [HttpGet]
        public async Task<IActionResult> GetDetailsByParameterAsync([FromQuery] IngredienteQueryParameters parametrosConsultaIngrediente)
        {
            //Validamos los parámetros de página y elementos por página
            if (parametrosConsultaIngrediente.Pagina <= 0)
                return BadRequest("El número de página debe ser mayor que 0.");

            if (parametrosConsultaIngrediente.ElementosPorPagina <= 0)
                return BadRequest("El número de elementos por página debe ser mayor que 0.");

            //Si todos los parameros son nulos, se traen todos los estilos
            if (parametrosConsultaIngrediente.Id == 0 &&
               string.IsNullOrEmpty(parametrosConsultaIngrediente.Nombre))
            {
                try
                {
                    var respuestaEstilos = await _ingredienteService
                        .GetAllAsync(parametrosConsultaIngrediente);

                    return Ok(respuestaEstilos);

                }
                catch (AppValidationException error)
                {
                    return BadRequest(error.Message);
                }

            }
            else
            {
                //De lo contrario, se trae un ingrediente por el resto de parámetros
                IngredienteDetallado unIngredienteDetallado = new();
                try
                {
                    // Por Id
                    if (parametrosConsultaIngrediente.Id != 0)
                    {
                        unIngredienteDetallado = await _ingredienteService
                            .GetByAttributeAsync<int>(parametrosConsultaIngrediente.Id, "id");
                    }

                    //Por Nombre
                    if (!string.IsNullOrEmpty(parametrosConsultaIngrediente.Nombre))
                    {
                        unIngredienteDetallado = await _ingredienteService
                            .GetByAttributeAsync<string>(parametrosConsultaIngrediente.Nombre, "nombre");
                    }

                    return Ok(unIngredienteDetallado);
                }
                catch (AppValidationException error)
                {
                    return NotFound(error.Message);
                }
            }
        }


        [HttpGet("{ingrediente_id:int}")]
        public async Task<IActionResult> GetByIdAsync(int ingrediente_id)
        {
            try
            {
                var unIngrediente = await _ingredienteService
                    .GetByAttributeAsync<int>(ingrediente_id, "id");

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
                var lasCervezas = await _ingredienteService
                    .GetAssociatedBeersAsync(ingrediente_id);

                return Ok(lasCervezas);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Ingrediente ingrediente)
        {
            try
            {
                var ingredienteCreado = await _ingredienteService
                    .CreateAsync(ingrediente);

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
        public async Task<IActionResult> UpdateAsync(int ingrediente_id, Ingrediente ingrediente)
        {
            try
            {
                var ingredienteActualizado = await _ingredienteService
                    .UpdateAsync(ingrediente_id, ingrediente);

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

                return Ok($"Estilo {ingrediente_id} fue eliminado");

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
