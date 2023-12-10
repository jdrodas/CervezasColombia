using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CervezasColombia_CS_API_SQLite_Dapper.Estilos
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstilosController(EstiloService estiloService) : Controller
    {
        private readonly EstiloService _estiloService = estiloService;

        [HttpGet]
        public async Task<IActionResult> GetDetailsByParameterAsync([FromQuery] EstiloQueryParameters parametrosConsultaEstilo)
        {
            //Validamos los parámetros de página y elementos por página
            if (parametrosConsultaEstilo.Pagina <= 0)
                return BadRequest("El número de página debe ser mayor que 0.");

            if (parametrosConsultaEstilo.ElementosPorPagina <= 0)
                return BadRequest("El número de elementos por página debe ser mayor que 0.");

            //Si todos los parameros son nulos, se traen todos los estilos
            if (parametrosConsultaEstilo.Id == 0 &&
               string.IsNullOrEmpty(parametrosConsultaEstilo.Nombre))
            {
                try
                {
                    var respuestaEstilos = await _estiloService
                        .GetAllAsync(parametrosConsultaEstilo);

                    return Ok(respuestaEstilos);

                }
                catch (AppValidationException error)
                {
                    return BadRequest(error.Message);
                }

            }
            else
            {
                //De lo contrario, se trae un estilo por el resto de parámetros
                EstiloDetallado unEstiloDetallado = new();
                try
                {
                    // Por Id
                    if (parametrosConsultaEstilo.Id != 0)
                    {
                        unEstiloDetallado = await _estiloService
                            .GetByAttributeAsync<int>(parametrosConsultaEstilo.Id, "id");
                    }

                    //Por Nombre
                    if (!string.IsNullOrEmpty(parametrosConsultaEstilo.Nombre))
                    {
                        unEstiloDetallado = await _estiloService
                            .GetByAttributeAsync<string>(parametrosConsultaEstilo.Nombre, "nombre");
                    }

                    return Ok(unEstiloDetallado);
                }
                catch (AppValidationException error)
                {
                    return NotFound(error.Message);
                }
            }
        }

        [HttpGet("{estilo_id:int}")]
        public async Task<IActionResult> GetByIdAsync(int estilo_id)
        {
            try
            {
                var unEstilo = await _estiloService
                    .GetByAttributeAsync<int>(estilo_id, "id");

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
        public async Task<IActionResult> CreateAsync(Estilo unEstilo)
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