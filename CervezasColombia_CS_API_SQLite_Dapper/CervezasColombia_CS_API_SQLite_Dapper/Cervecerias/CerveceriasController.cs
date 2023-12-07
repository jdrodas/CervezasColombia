using Microsoft.AspNetCore.Mvc;

namespace CervezasColombia_CS_API_SQLite_Dapper.Cervecerias
{
    [Route("api/[controller]")]
    [ApiController]
    public class CerveceriasController(CerveceriaService cerveceriaService) : Controller
    {
        private readonly CerveceriaService _cerveceriaService = cerveceriaService;

        [HttpGet]
        public async Task<IActionResult> GetDetailsByParameterAsync([FromQuery] CerveceriaQueryParameters parametrosConsultaCerveceria)
        {
            //Validamos los parámetros de página y elementos por página
            if (parametrosConsultaCerveceria.Pagina <= 0)
                return BadRequest("El número de página debe ser mayor que 0.");

            if (parametrosConsultaCerveceria.ElementosPorPagina <= 0)
                return BadRequest("El número de elementos por página debe ser mayor que 0.");

            //Si todos los parameros son nulos, se traen todas las cervecerias
            if (parametrosConsultaCerveceria.Id == 0 &&
               string.IsNullOrEmpty(parametrosConsultaCerveceria.Nombre) &&
               string.IsNullOrEmpty(parametrosConsultaCerveceria.Instagram))
            {
                try
                {
                    var respuestaCervecerias = await _cerveceriaService
                        .GetAllAsync(parametrosConsultaCerveceria);

                    return Ok(respuestaCervecerias);

                }
                catch (AppValidationException error)
                {
                    return BadRequest(error.Message);
                }
            }
            else
            {
                //De lo contrario, se trae una Cervecería por el resto de parámetros
                CerveceriaDetallada unaCerveceriaDetallada = new();
                try
                {
                    // Por Id
                    if (parametrosConsultaCerveceria.Id != 0)
                        unaCerveceriaDetallada = await _cerveceriaService
                            .GetByAttributeAsync<int>(parametrosConsultaCerveceria.Id, "id");

                    //Por Nombre
                    if (!string.IsNullOrEmpty(parametrosConsultaCerveceria.Nombre))
                        unaCerveceriaDetallada = await _cerveceriaService
                            .GetByAttributeAsync<string>(parametrosConsultaCerveceria.Nombre, "nombre");

                    //Por Instagram
                    if (!string.IsNullOrEmpty(parametrosConsultaCerveceria.Instagram))
                        unaCerveceriaDetallada = await _cerveceriaService
                            .GetByAttributeAsync<string>(parametrosConsultaCerveceria.Instagram, "instagram");

                    return Ok(unaCerveceriaDetallada);
                }
                catch (AppValidationException error)
                {
                    return NotFound(error.Message);
                }
            }
        }

        [HttpGet("{cerveceria_id:int}")]
        public async Task<IActionResult> GetByIdAsync(int cerveceria_id)
        {
            try
            {
                var unaCerveceriaDetallada = await _cerveceriaService
                    .GetByAttributeAsync<int>(cerveceria_id, "id");

                return Ok(unaCerveceriaDetallada);
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
                var lasCervezasPorEstilo = await _cerveceriaService
                    .GetAssociatedBeersAsync(cerveceria_id);

                return Ok(lasCervezasPorEstilo);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Cerveceria cerveceria)
        {
            try
            {
                var cerveceriaCreada = await _cerveceriaService
                    .CreateAsync(cerveceria);

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
        public async Task<IActionResult> UpdateAsync(int cerveceria_id, Cerveceria cerveceria)
        {
            try
            {
                var cerveceriaActualizada = await _cerveceriaService
                    .UpdateAsync(cerveceria_id, cerveceria);

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