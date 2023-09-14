using CervezasColombia_CS_API_SQLite_Dapper.Models;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Services;
using Microsoft.AspNetCore.Mvc;

namespace CervezasColombia_CS_API_SQLite_Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UbicacionesController : Controller
    {
        private readonly UbicacionService _ubicacionService;

        public UbicacionesController(UbicacionService ubicacionService)
        {
            _ubicacionService = ubicacionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var lasUbicaciones = await _ubicacionService
                .GetAllAsync();

            return Ok(lasUbicaciones);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var unaUbicacion = await _ubicacionService
                    .GetByIdAsync(id);
                return Ok(unaUbicacion);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{id:int}/Cervecerias")]
        public async Task<IActionResult> GetAssociatedBreweriesAsync(int id)
        {
            try
            {
                var lasCerveceriasPorUbicacion = await _ubicacionService.
                    GetAssociatedBreweriesAsync(id);

                return Ok(lasCerveceriasPorUbicacion);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

    }
}