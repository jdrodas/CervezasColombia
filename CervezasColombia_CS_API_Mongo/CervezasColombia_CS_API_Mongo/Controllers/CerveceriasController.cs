﻿using CervezasColombia_CS_API_Mongo.Helpers;
using CervezasColombia_CS_API_Mongo.Models;
using CervezasColombia_CS_API_Mongo.Services;
using Microsoft.AspNetCore.Mvc;

namespace CervezasColombia_CS_API_Mongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CerveceriasController : Controller
    {
        private readonly CerveceriaService _cerveceriaService;

        public CerveceriasController(CerveceriaService cerveceriaService)
        {
            _cerveceriaService = cerveceriaService;
        }

        [HttpGet("{cerveceria_id:length(24)}")]
        public async Task<IActionResult> GetDetailsByIdAsync(string cerveceria_id)
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

        [HttpGet]
        public async Task<IActionResult> GetDetailsByParameterAsync([FromQuery] ConsultaCerveceria parametros)
        {
            //Si todos los parameros son nulos, se traen todas las cervecerias
            if (string.IsNullOrEmpty(parametros.Id) &&
               string.IsNullOrEmpty(parametros.Nombre) &&
               string.IsNullOrEmpty(parametros.Instagram))
            {
                var lasCervecerias = await _cerveceriaService
                    .GetAllAsync();

                return Ok(lasCervecerias);
            }
            else
            {
                //De lo contrario, se trae una Cervecería por el resto de parámetros
                CerveceriaDetallada unaCerveceriaDetallada = new();
                try
                {
                    // Por Id
                    if (!string.IsNullOrEmpty(parametros.Id))
                    {
                        unaCerveceriaDetallada = await _cerveceriaService
                        .GetDetailsByIdAsync(parametros.Id);
                    }

                    //Por Nombre
                    if (!string.IsNullOrEmpty(parametros.Nombre))
                    {
                        unaCerveceriaDetallada = await _cerveceriaService
                        .GetByNameAsync(parametros.Nombre);
                    }

                    //Por Instagram
                    if (!string.IsNullOrEmpty(parametros.Instagram))
                    {
                        unaCerveceriaDetallada = await _cerveceriaService
                        .GetByInstagramAsync(parametros.Instagram);
                    }

                    return Ok(unaCerveceriaDetallada);
                }
                catch (AppValidationException error)
                {
                    return NotFound(error.Message);
                }
            }
        }


        [HttpGet("{cerveceria_id:length(24)}/Cervezas")]
        public async Task<IActionResult> GetAssociatedBeersAsync(string cerveceria_id)
        {
            try
            {
                var lasCervezasPorCerveceria = await _cerveceriaService
                    .GetAssociatedBeersAsync(cerveceria_id);

                return Ok(lasCervezasPorCerveceria);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Cerveceria unaCerveceria)
        {
            try
            {
                var cerveceriaCreada = await _cerveceriaService
                    .CreateAsync(unaCerveceria);

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

        [HttpPut("{cerveceria_id:length(24)}")]
        public async Task<IActionResult> UpdateAsync(string cerveceria_id, Cerveceria unaCerveceria)
        {
            try
            {
                var cerveceriaActualizada = await _cerveceriaService
                    .UpdateAsync(cerveceria_id, unaCerveceria);

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

        [HttpDelete("{cerveceria_id:length(24)}")]
        public async Task<IActionResult> DeleteAsync(string cerveceria_id)
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