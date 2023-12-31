﻿using CervezasColombia_CS_API_Mongo.Helpers;
using CervezasColombia_CS_API_Mongo.Models;
using CervezasColombia_CS_API_Mongo.Services;
using Microsoft.AspNetCore.Mvc;

namespace CervezasColombia_CS_API_Mongo.Controllers
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

        [HttpGet("{ubicacion_id:length(24)}")]
        public async Task<IActionResult> GetByIdAsync(string ubicacion_id)
        {
            try
            {
                var unaUbicacion = await _ubicacionService
                    .GetByIdAsync(ubicacion_id);

                return Ok(unaUbicacion);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{ubicacion_id:length(24)}/Cervecerias")]
        public async Task<IActionResult> GetAssociatedBreweriesAsync(string ubicacion_id)
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

        [HttpPut("{ubicacion_id:length(24)}")]
        public async Task<IActionResult> UpdateAsync(string ubicacion_id, Ubicacion unaUbicacion)
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

        [HttpDelete("{ubicacion_id:length(24)}")]
        public async Task<IActionResult> DeleteAsync(string ubicacion_id)
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