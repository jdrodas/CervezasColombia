using CervezasColombia_CS_API_SQLite_Dapper.Estilos;
using Microsoft.AspNetCore.Mvc;

namespace CervezasColombia_CS_API_SQLite_Dapper.Cervecerias
{
    [Route("api/[controller]")]
    [ApiController]
    public class CerveceriasController(CerveceriaService cerveceriaService) : Controller
    {
        private readonly CerveceriaService _cerveceriaService = cerveceriaService;

        [HttpGet]
        public async Task<IActionResult> GetDetailsByParameterAsync([FromQuery] CerveceriaQuery cerveceriaQuery)
        {
            //Si todos los parameros son nulos, se traen todas las cervecerias
            if (cerveceriaQuery.Id == 0 &&
               string.IsNullOrEmpty(cerveceriaQuery.Nombre) &&
               string.IsNullOrEmpty(cerveceriaQuery.Instagram))
            {
                var lasCervecerias = await _cerveceriaService
                    .GetAllAsync();

                return Ok(lasCervecerias);
            }
            else
            {
                //De lo contrario, se trae una Cervecería por el resto de parámetros
                CerveceriaResponse unaCerveceriaDetallada = new();
                try
                {
                    // Por Id
                    if (cerveceriaQuery.Id != 0)
                    {
                        unaCerveceriaDetallada = await _cerveceriaService
                        .GetDetailsByIdAsync(cerveceriaQuery.Id);
                    }

                    //Por Nombre
                    if (!string.IsNullOrEmpty(cerveceriaQuery.Nombre))
                    {
                        unaCerveceriaDetallada = await _cerveceriaService
                        .GetByNameAsync(cerveceriaQuery.Nombre);
                    }

                    //Por Instagram
                    if (!string.IsNullOrEmpty(cerveceriaQuery.Instagram))
                    {
                        unaCerveceriaDetallada = await _cerveceriaService
                        .GetByInstagramAsync(cerveceriaQuery.Instagram);
                    }

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
                    .GetDetailsByIdAsync(cerveceria_id);

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