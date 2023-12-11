using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CervezasColombia_CS_API_SQLite_Dapper.Unidades
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadesController(UnidadService unidadService) : Controller
    {
        private readonly UnidadService _unidadService = unidadService;
        [HttpGet]
        public async Task<IActionResult> GetDetailsByParameterAsync([FromQuery] UnidadQueryParameters parametrosConsultaUnidad)
        {
            //Validamos los parámetros de página y elementos por página
            if (parametrosConsultaUnidad.Pagina <= 0)
                return BadRequest("El número de página debe ser mayor que 0.");

            if (parametrosConsultaUnidad.ElementosPorPagina <= 0)
                return BadRequest("El número de elementos por página debe ser mayor que 0.");

            //Si todos los parameros son nulos, se traen todos los estilos
            if (parametrosConsultaUnidad.Id == 0 &&
               string.IsNullOrEmpty(parametrosConsultaUnidad.Nombre) &&
               string.IsNullOrEmpty(parametrosConsultaUnidad.Abreviatura))
            {
                try
                {
                    var respuestaEstilos = await _unidadService
                        .GetAllAsync(parametrosConsultaUnidad);

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
                Unidad unaUnidad = new();
                try
                {
                    // Por Id
                    if (parametrosConsultaUnidad.Id != 0)
                    {
                        unaUnidad = await _unidadService
                            .GetByAttributeAsync<int>(parametrosConsultaUnidad.Id, "id");
                    }

                    //Por Nombre
                    if (!string.IsNullOrEmpty(parametrosConsultaUnidad.Nombre))
                    {
                        unaUnidad = await _unidadService
                            .GetByAttributeAsync<string>(parametrosConsultaUnidad.Nombre, "nombre");
                    }

                    //Por Abreviatura
                    if (!string.IsNullOrEmpty(parametrosConsultaUnidad.Abreviatura))
                    {
                        unaUnidad = await _unidadService
                            .GetByAttributeAsync<string>(parametrosConsultaUnidad.Abreviatura, "abreviatura");
                    }

                    return Ok(unaUnidad);
                }
                catch (AppValidationException error)
                {
                    return NotFound(error.Message);
                }
            }
        }

        [HttpGet("{unidad_id:int}")]
        public async Task<IActionResult> GetByIdAsync(int unidad_id)
        {
            try
            {
                var unaUnidad = await _unidadService
                    .GetByAttributeAsync<int>(unidad_id, "id");

                return Ok(unaUnidad);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }
    }
}
