﻿using Microsoft.AspNetCore.Mvc;

namespace CervezasColombia_CS_API_SQLite_Dapper.Cervezas
{
    [Route("api/[controller]")]
    [ApiController]
    public class CervezasController(CervezaService cervezaService) : Controller
    {
        private readonly CervezaService _cervezaService = cervezaService;

        [HttpGet]
        public async Task<IActionResult> GetDetailsByParameterAsync([FromQuery] ConsultaCerveza parametros)
        {
            //Si todos los parameros son nulos, se traen todas las cervezas
            if (parametros.Id == 0 &&
               string.IsNullOrEmpty(parametros.Nombre) &&
               string.IsNullOrEmpty(parametros.Cerveceria))
            {
                var lasCervezas = await _cervezaService
                    .GetAllAsync();

                return Ok(lasCervezas);
            }
            else
            {
                //De lo contrario, se trae una Cerveza por el resto de parámetros
                Cerveza unaCerveza;
                try
                {
                    // Por Id
                    if (parametros.Id != 0)
                    {
                        unaCerveza = await _cervezaService
                        .GetByIdAsync(parametros.Id);
                    }
                    else
                    {
                        // Por Nombre Y Cerveceria
                        if (!string.IsNullOrEmpty(parametros.Nombre) &&
                            !string.IsNullOrEmpty(parametros.Cerveceria))
                        {

                            unaCerveza = await _cervezaService
                            .GetByNameAndBreweryAsync(parametros.Nombre, parametros.Cerveceria);
                        }
                        else
                        {
                            return NotFound("No se encontró cerveza identificada por esos parámetros");
                        }
                    }
                }
                catch (AppValidationException error)
                {
                    return NotFound(error.Message);
                }

                return Ok(unaCerveza);
            }
        }

        [HttpGet("{cerveza_id:int}")]
        public async Task<IActionResult> GetByIdAsync(int cerveza_id)
        {
            try
            {
                var unaCerveza = await _cervezaService
                    .GetByIdAsync(cerveza_id);
                return Ok(unaCerveza);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        //[HttpGet("{cerveza_id:int}/Ingredientes")]
        //public async Task<IActionResult> GetAssociatedIngredientsAsync(int cerveza_id)
        //{
        //    try
        //    {
        //        var losIngredientesPorCerveza = await _cervezaService
        //            .GetAssociatedIngredientsAsync(cerveza_id);

        //        return Ok(losIngredientesPorCerveza);
        //    }
        //    catch (AppValidationException error)
        //    {
        //        return NotFound(error.Message);
        //    }
        //}

        //[HttpGet("{cerveza_id:int}/Envasados")]
        //public async Task<IActionResult> GetAssociatedPackagingsAsync(int cerveza_id)
        //{
        //    try
        //    {
        //        var losEnvasadosPorCerveza = await _cervezaService
        //            .GetAssociatedPackagingsAsync(cerveza_id);

        //        return Ok(losEnvasadosPorCerveza);
        //    }
        //    catch (AppValidationException error)
        //    {
        //        return NotFound(error.Message);
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Cerveza unaCerveza)
        {
            try
            {
                var cervezaCreada = await _cervezaService
                    .CreateAsync(unaCerveza);

                return Ok(cervezaCreada);
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

        //[HttpPost("{cerveza_id:int}/Envasados")]
        //public async Task<IActionResult> CreateBeerPackagingAsync(int cerveza_id, EnvasadoCerveza unEnvasadoCerveza)
        //{
        //    try
        //    {
        //        var cervezaEnvasada = await _cervezaService
        //            .CreateBeerPackagingAsync(cerveza_id, unEnvasadoCerveza);

        //        return Ok(cervezaEnvasada);
        //    }
        //    catch (AppValidationException error)
        //    {
        //        return BadRequest($"Error de validación: {error.Message}");
        //    }
        //    catch (DbOperationException error)
        //    {
        //        return BadRequest($"Error de operacion en DB: {error.Message}");
        //    }
        //}

        //[HttpPost("{cerveza_id:int}/Ingredientes")]
        //public async Task<IActionResult> CreateBeerIngredientAsync(int cerveza_id, Ingrediente unIngrediente)
        //{
        //    try
        //    {
        //        await _cervezaService
        //           .CreateBeerIngredientAsync(cerveza_id, unIngrediente);

        //        return Ok($"Ingrediente {unIngrediente.Tipo_Ingrediente} - {unIngrediente.Nombre} asociado a esta Cerveza");
        //    }
        //    catch (AppValidationException error)
        //    {
        //        return BadRequest($"Error de validación: {error.Message}");
        //    }
        //    catch (DbOperationException error)
        //    {
        //        return BadRequest($"Error de operacion en DB: {error.Message}");
        //    }
        //}

        //[HttpPut("{cerveza_id:int}")]
        //public async Task<IActionResult> UpdateAsync(int cerveza_id, Cerveza unaCerveza)
        //{
        //    try
        //    {
        //        var cervezaActualizada = await _cervezaService
        //            .UpdateAsync(cerveza_id, unaCerveza);

        //        return Ok(cervezaActualizada);

        //    }
        //    catch (AppValidationException error)
        //    {
        //        return BadRequest($"Error de validación: {error.Message}");
        //    }
        //    catch (DbOperationException error)
        //    {
        //        return BadRequest($"Error de operacion en DB: {error.Message}");
        //    }
        //}

        //[HttpDelete("{cerveza_id:int}")]
        //public async Task<IActionResult> DeleteAsync(int cerveza_id)
        //{
        //    try
        //    {
        //        await _cervezaService
        //            .DeleteAsync(cerveza_id);

        //        return Ok($"Cerveza {cerveza_id} fue eliminada");

        //    }
        //    catch (AppValidationException error)
        //    {
        //        return BadRequest($"Error de validación: {error.Message}");
        //    }
        //    catch (DbOperationException error)
        //    {
        //        return BadRequest($"Error de operacion en DB: {error.Message}");
        //    }
        //}

        //[HttpDelete("{cerveza_id:int}/Envasados")]
        //public async Task<IActionResult> DeleteBeerPackagingAsync(int cerveza_id, EnvasadoCerveza unEnvasadoCerveza)
        //{
        //    try
        //    {
        //        await _cervezaService
        //            .DeleteBeerPackagingAsync(cerveza_id, unEnvasadoCerveza);

        //        return Ok($"Envasado {unEnvasadoCerveza.Envasado} de {unEnvasadoCerveza.Volumen} {unEnvasadoCerveza.Unidad_Volumen} fue eliminado para esta cerveza");
        //    }
        //    catch (AppValidationException error)
        //    {
        //        return BadRequest($"Error de validación: {error.Message}");
        //    }
        //    catch (DbOperationException error)
        //    {
        //        return BadRequest($"Error de operacion en DB: {error.Message}");
        //    }
        //}

        //[HttpDelete("{cerveza_id:int}/Ingredientes")]
        //public async Task<IActionResult> DeleteBeerIngredientAsync(int cerveza_id, Ingrediente unIngrediente)
        //{
        //    try
        //    {
        //        await _cervezaService
        //            .DeleteBeerIngredientAsync(cerveza_id, unIngrediente);

        //        return Ok($"Ingrediente {unIngrediente.Tipo_Ingrediente} - {unIngrediente.Nombre} fue eliminado para esta cerveza");
        //    }
        //    catch (AppValidationException error)
        //    {
        //        return BadRequest($"Error de validación: {error.Message}");
        //    }
        //    catch (DbOperationException error)
        //    {
        //        return BadRequest($"Error de operacion en DB: {error.Message}");
        //    }
        //}
    }
}