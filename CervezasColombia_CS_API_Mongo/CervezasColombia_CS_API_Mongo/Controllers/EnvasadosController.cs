using CervezasColombia_CS_API_Mongo.Helpers;
using CervezasColombia_CS_API_Mongo.Models;
using CervezasColombia_CS_API_Mongo.Services;
using Microsoft.AspNetCore.Mvc;

namespace CervezasColombia_CS_API_Mongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnvasadosController : Controller
    {
        private readonly EnvasadoService _envasadoService;

        public EnvasadosController(EnvasadoService envasadoService)
        {
            _envasadoService = envasadoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var losEnvasados = await _envasadoService
                .GetAllAsync();

            return Ok(losEnvasados);
        }

        [HttpGet("{envasado_id:length(24)}")]
        public async Task<IActionResult> GetByIdAsync(string envasado_id)
        {
            try
            {
                var unEnvasado = await _envasadoService
                    .GetByIdAsync(envasado_id);

                return Ok(unEnvasado);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        //[HttpGet("{envasado_id:int}/Cervezas")]
        //public async Task<IActionResult> GetAssociatedBeersAsync(int envasado_id)
        //{
        //    try
        //    {
        //        var lasCervezasPorEnvasado = await _envasadoService
        //            .GetAssociatedBeersAsync(envasado_id);

        //        return Ok(lasCervezasPorEnvasado);
        //    }
        //    catch (AppValidationException error)
        //    {
        //        return NotFound(error.Message);
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Envasado unEnvasado)
        {
            try
            {
                var envasadoCreado = await _envasadoService
                    .CreateAsync(unEnvasado);

                return Ok(envasadoCreado);
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

        [HttpPut("{envasado_id:length(24)}")]
        public async Task<IActionResult> UpdateAsync(string envasado_id, Envasado unEnvasado)
        {
            try
            {
                var envasadoActualizado = await _envasadoService
                    .UpdateAsync(envasado_id, unEnvasado);

                return Ok(envasadoActualizado);

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

        [HttpDelete("{envasado_id:length(24)}")]
        public async Task<IActionResult> DeleteAsync(string envasado_id)
        {
            try
            {
                await _envasadoService
                    .DeleteAsync(envasado_id);

                return Ok($"Envasado {envasado_id} fue eliminado");

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