using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CervezasColombia_CS_API_SQLite_Dapper.Ubicaciones
{
    [Route("api/[controller]")]
    [ApiController]
    public class UbicacionesController(UbicacionService ubicacionService) : Controller
    {
        private readonly UbicacionService _ubicacionService = ubicacionService;

        [HttpGet]
        public async Task<IActionResult> GetDetailsByParameterAsync([FromQuery] UbicacionQueryParameters parametrosConsultaUbicacion)
        {
            //Validamos los parámetros de página y elementos por página
            if (parametrosConsultaUbicacion.Pagina <= 0)
                return BadRequest("El número de página debe ser mayor que 0.");

            if (parametrosConsultaUbicacion.ElementosPorPagina <= 0)
                return BadRequest("El número de elementos por página debe ser mayor que 0.");

            //Si todos los parameros son nulos, se traen todos los estilos
            if (parametrosConsultaUbicacion.Id == 0 &&
               string.IsNullOrEmpty(parametrosConsultaUbicacion.Nombre))
            {
                try
                {
                    var respuestaUbicaciones = await _ubicacionService
                        .GetAllAsync(parametrosConsultaUbicacion);

                    return Ok(respuestaUbicaciones);

                }
                catch (AppValidationException error)
                {
                    return BadRequest(error.Message);
                }

            }
            else
            {
                //De lo contrario, se trae un estilo por el resto de parámetros
                UbicacionDetallada unaUbicacionDetallada = new();
                try
                {
                    // Por Id
                    if (parametrosConsultaUbicacion.Id != 0)
                    {
                        unaUbicacionDetallada = await _ubicacionService
                            .GetByAttributeAsync<int>(parametrosConsultaUbicacion.Id, "id");
                    }

                    //Por Nombre, compuesto de "municipio, departamento"
                    if (!string.IsNullOrEmpty(parametrosConsultaUbicacion.Nombre))
                    {
                        unaUbicacionDetallada = await _ubicacionService
                            .GetByAttributeAsync<string>(parametrosConsultaUbicacion.Nombre, "nombre");
                    }


                    return Ok(unaUbicacionDetallada);
                }
                catch (AppValidationException error)
                {
                    return NotFound(error.Message);
                }
            }
        }

        [HttpGet("{ubicacion_id:int}")]
        public async Task<IActionResult> GetByIdAsync(int ubicacion_id)
        {
            try
            {
                var unaUbicacion = await _ubicacionService
                            .GetByAttributeAsync<int>(ubicacion_id, "id");

                return Ok(unaUbicacion);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{ubicacion_id:int}/Cervecerias")]
        public async Task<IActionResult> GetAssociatedBreweriesAsync(int ubicacion_id)
        {
            try
            {
                var lasCerveceriasPorUbicacion = await _ubicacionService
                    .GetAssociatedBreweriesAsync(ubicacion_id);

                return Ok(lasCerveceriasPorUbicacion);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Ubicacion unaUbicacion)
        {
            try
            {
                var ubicacionCreada = await _ubicacionService
                    .CreateAsync(unaUbicacion);

                return Ok(ubicacionCreada);
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

        [HttpPut("{ubicacion_id:int}")]
        public async Task<IActionResult> UpdateAsync(int ubicacion_id, Ubicacion unaUbicacion)
        {
            try
            {
                var ubicacionActualizada = await _ubicacionService
                    .UpdateAsync(ubicacion_id, unaUbicacion);

                return Ok(ubicacionActualizada);

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

        [HttpDelete("{ubicacion_id:int}")]
        public async Task<IActionResult> DeleteAsync(int ubicacion_id)
        {
            try
            {
                await _ubicacionService
                    .DeleteAsync(ubicacion_id);

                return Ok($"Ubicación {ubicacion_id} fue eliminada");

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