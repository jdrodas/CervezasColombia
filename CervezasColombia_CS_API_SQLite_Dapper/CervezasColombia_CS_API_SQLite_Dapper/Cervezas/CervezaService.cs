﻿using CervezasColombia_CS_API_SQLite_Dapper.Cervecerias;
using CervezasColombia_CS_API_SQLite_Dapper.Estilos;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;

namespace CervezasColombia_CS_API_SQLite_Dapper.Cervezas
{
    public class CervezaService(ICervezaRepository cervezaRepository
            , ICerveceriaRepository cerveceriaRepository
            , IEstiloRepository estiloRepository
            //, IEnvasadoRepository envasadoRepository
            //, IUnidadVolumenRepository unidadVolumenRepository
            //, IIngredienteRepository ingredienteRepository
            )
    {
        private readonly ICervezaRepository _cervezaRepository = cervezaRepository;
        private readonly ICerveceriaRepository _cerveceriaRepository = cerveceriaRepository;
        private readonly IEstiloRepository _estiloRepository = estiloRepository;
        //private readonly IEnvasadoRepository _envasadoRepository = envasadoRepository;
        //private readonly IUnidadVolumenRepository _unidadVolumenRepository = unidadVolumenRepository;
        //private readonly IIngredienteRepository _ingredienteRepository = ingredienteRepository;

        public async Task<CervezaResponse> GetAllAsync(CervezaQueryParameters parametrosConsultaCerveza)
        {
            var lasCervezas = await _cervezaRepository
                .GetAllAsync();

            // Calculamos items totales y cantidad de páginas
            var totalElementos = lasCervezas.Count();
            var totalPaginas = (int)Math.Ceiling((double)totalElementos / parametrosConsultaCerveza.ElementosPorPagina);

            //Validamos que la página solicitada está dentro del rango permitido
            if (parametrosConsultaCerveza.Pagina > totalPaginas && totalPaginas > 0)
                throw new AppValidationException($"La página solicitada No. {parametrosConsultaCerveza.Pagina} excede el número total de página de {totalPaginas}");

            //Aplicamos el ordenamiento
            switch (parametrosConsultaCerveza.Criterio)
            {
                case "nombre":
                    lasCervezas = ApplyOrder(
                        lasCervezas,
                        p => p.Nombre,
                        parametrosConsultaCerveza.Orden);
                    break;

                case "cerveceria":
                    lasCervezas = ApplyOrder(
                        lasCervezas,
                        p => p.Cerveceria,
                        parametrosConsultaCerveza.Orden);
                    break;
            }

            //Aplicamos la paginación
            lasCervezas = lasCervezas
                .Skip((parametrosConsultaCerveza.Pagina - 1) * parametrosConsultaCerveza.ElementosPorPagina)
                .Take(parametrosConsultaCerveza.ElementosPorPagina);

            var respuestaCervezas = new CervezaResponse
            {
                Tipo = "Cerveza",
                TotalElementos = totalElementos,
                PaginaActual = parametrosConsultaCerveza.Pagina,
                ElementosPorPagina = parametrosConsultaCerveza.ElementosPorPagina, // PageSize
                TotalPaginas = totalPaginas,
                Data = lasCervezas.ToList()
            };

            return respuestaCervezas;
        }

        public async Task<CervezaDetallada> GetByAttributeAsync<T>(T atributo_valor, string atributo_nombre)
        {
            Cerveza unaCerveza = new();

            switch (atributo_nombre.ToLower())
            {
                case "id":
                    unaCerveza = await _cervezaRepository
                        .GetByAttributeAsync<T>(atributo_valor, "id");
                    break;

                case "nombre":
                    unaCerveza = await _cervezaRepository
                        .GetByAttributeAsync<T>(atributo_valor, "nombre");
                    break;
            }

            if (unaCerveza.Id == 0)
                throw new AppValidationException($"Cerveza no encontrada con el atributo {atributo_nombre} {atributo_valor}");

            var unaCervezaDetallada = await BuildDetailedBeerAsync(unaCerveza);
            return unaCervezaDetallada;
        }

        public async Task<Cerveza> GetByNameAndBreweryAsync(string cerveza_nombre, string cerveceria_nombre)
        {
            //Validamos que el estilo exista con ese Id
            var unaCerveza = await _cervezaRepository
                .GetByNameAndBreweryAsync(cerveza_nombre, cerveceria_nombre);

            if (unaCerveza.Id == 0)
                throw new AppValidationException($"Cerveza no encontrada con el nombre {cerveza_nombre} " +
                    $"de la cervecería {cerveceria_nombre}");

            return unaCerveza;
        }

        public async Task<Cerveza> CreateAsync(Cerveza unaCerveza)
        {
            //Validamos que la cerveza tenga nombre
            if (unaCerveza.Nombre.Length == 0)
                throw new AppValidationException("No se puede insertar una cerveza con nombre nulo");

            //Validamos que la cerveza tenga asociada una cerveceria
            if (unaCerveza.Cerveceria.Length == 0)
                throw new AppValidationException("No se puede insertar una cerveza sin una cerveceria");

            //Validamos que la cervecería exista
            var cerveceriaExistente = await _cerveceriaRepository
                        .GetByAttributeAsync<string>(unaCerveza.Cerveceria, "nombre");

            if (cerveceriaExistente.Id == 0)
                throw new AppValidationException($"La cervecería {unaCerveza.Cerveceria} no se encuentra registrada");

            unaCerveza.Cerveceria_id = cerveceriaExistente.Id;

            //Validamos que la cerveza tenga estilo
            if (unaCerveza.Estilo.Length == 0)
                throw new AppValidationException("No se puede insertar una cerveza con estilo nulo");

            //Validamos que el estilo exista
            var estiloExistente = await _estiloRepository
                .GetByAttributeAsync<string>(unaCerveza.Estilo, "nombre");

            if (estiloExistente.Id == 0)
                throw new AppValidationException($"El estilo {unaCerveza.Estilo} no se encuentra registrado");

            unaCerveza.Estilo_id = estiloExistente.Id;

            //Validar que no exista para esa cerveceria, una cerveza con ese nombre
            var cervezaExistente = await _cervezaRepository
                .GetByNameAndBreweryAsync(unaCerveza.Nombre!, unaCerveza.Cerveceria!);

            if (cervezaExistente.Id != 0)
                throw new AppValidationException($"Ya existe la cerveza {unaCerveza.Nombre} " +
                    $"para la cerveceria {unaCerveza.Cerveceria}");

            try
            {
                bool resultadoAccion = await _cervezaRepository
                    .CreateAsync(unaCerveza);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                cervezaExistente = await _cervezaRepository
                    .GetByNameAndBreweryAsync(unaCerveza.Nombre!, unaCerveza.Cerveceria!);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return cervezaExistente;
        }

        //public async Task<EnvasadoCerveza> CreateBeerPackagingAsync(int cerveza_id, EnvasadoCerveza unEnvasadoCerveza)
        //{
        //    //Validamos que la cerveza referenciada en el EnvasadoCerveza exista
        //    var cervezaExistente = await _cervezaRepository
        //        .GetByNameAndBreweryAsync(unEnvasadoCerveza.Cerveza, unEnvasadoCerveza.Cerveceria);

        //    if (cervezaExistente.Id != cerveza_id)
        //        throw new AppValidationException($"Inconsistencia en los parámetros. " +
        //            $"Los Ids de las cervezas no coinciden entre si.");

        //    if (cervezaExistente.Id==0)
        //        throw new AppValidationException($"No existe una cerveza registrada con el id {cerveza_id}");

        //    //Validamos que el envasado tenga nombre
        //    if (unEnvasadoCerveza.Envasado.Length == 0)
        //        throw new AppValidationException("No se puede insertar un envasado de cerveza con nombre nulo");

        //    //Validamos que la unidad de volumen tenga nombre
        //    if (unEnvasadoCerveza.Unidad_Volumen.Length == 0)
        //        throw new AppValidationException("No se puede insertar un envasado de cerveza con unidad de volumen nula");

        //    //Validamos que el envasado exista
        //    var envasadoExistente = await _envasadoRepository
        //        .GetByNameAsync(unEnvasadoCerveza.Envasado!);

        //    if (envasadoExistente.Id == 0)
        //        throw new AppValidationException($"El envasado {unEnvasadoCerveza.Envasado} no se encuentra registrado.");

        //    //Validamos que la unidad de volumen exista
        //    var unidadVolumenExistente = await _unidadVolumenRepository
        //        .GetByNameAsync(unEnvasadoCerveza.Unidad_Volumen!);

        //    if (unidadVolumenExistente.Id == 0)
        //        throw new AppValidationException($"La unidad de Volumen {unEnvasadoCerveza.Unidad_Volumen} no se encuentra registrada.");

        //    if (unEnvasadoCerveza.Volumen <= 0)
        //        throw new AppValidationException($"el Volumen {unEnvasadoCerveza.Volumen} no corresponde a un valor válido para un envasado.");

        //    unEnvasadoCerveza.Cerveceria = cervezaExistente.Cerveceria;
        //    unEnvasadoCerveza.Cerveza = cervezaExistente.Nombre;

        //    //Validamos que este envasado con unidad de volumen y volumen no exista para esta cerveza
        //    var envasadoCervezaExistente = await _envasadoRepository
        //        .GetAssociatedBeerPackagingAsync(unEnvasadoCerveza.Id);

        //    if (envasadoCervezaExistente.Id !=0)
        //        throw new AppValidationException($"Ya existe un envasado {unEnvasadoCerveza.Envasado} de {unEnvasadoCerveza.Volumen} " +
        //            $"{unEnvasadoCerveza.Unidad_Volumen} para la cerveza {cervezaExistente.Nombre} de {cervezaExistente.Cerveceria}.");

        //    try
        //    {
        //        bool resultadoAccion = await _cervezaRepository
        //            .CreateBeerPackagingAsync(cerveza_id, unEnvasadoCerveza);

        //        if (!resultadoAccion)
        //            throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

        //        envasadoCervezaExistente = await _cervezaRepository
        //            .GetPackagedBeerByNameAsync(cerveza_id, unEnvasadoCerveza.Envasado!, unEnvasadoCerveza.Unidad_Volumen, unEnvasadoCerveza.Volumen);
        //    }
        //    catch (DbOperationException)
        //    {
        //        throw;
        //    }

        //    return envasadoCervezaExistente;
        //}

        //public async Task CreateBeerIngredientAsync(int cerveza_id, Ingrediente unIngrediente)
        //{
        //    //Validamos que la cerveza exista con ese Id
        //    var cervezaExistente = await _cervezaRepository
        //        .GetByIdAsync(cerveza_id);

        //    if (cervezaExistente.Id == 0)
        //        throw new AppValidationException($"No existe una cerveza registrada con el id {cerveza_id}");

        //    //Validamos que el ingrediente tenga nombre
        //    if (unIngrediente.Nombre.Length == 0)
        //        throw new AppValidationException("No se puede insertar un ingrediente de cerveza con nombre nulo");

        //    //Validamos que el tipo de ingrediente tenga nombre
        //    if (unIngrediente.Tipo_Ingrediente.Length == 0)
        //        throw new AppValidationException("No se puede insertar un ingrediente de cerveza con tipo de ingrediente nulo");

        //    //Validamos que el ingrediente exista
        //    var ingredienteExistente = await _ingredienteRepository
        //        .GetByNameAndTypeAsync(unIngrediente.Nombre!, unIngrediente.Tipo_Ingrediente);

        //    if (ingredienteExistente.Id ==0)
        //        throw new AppValidationException($"El ingrediente {unIngrediente.Tipo_Ingrediente} - {unIngrediente.Nombre} no se encuentra registrado.");

        //    //Validamos que este ingrediente no exista para esta cerveza
        //    var CervezaConIngredienteExistente = await _ingredienteRepository
        //        .GetAssociatedBeerByIdAsync(ingredienteExistente.Id, cervezaExistente.Id);

        //    if (CervezaConIngredienteExistente.Id !=0)
        //        throw new AppValidationException($"Ya existe registro para el ingrediente {unIngrediente.Tipo_Ingrediente} " +
        //            $"- {unIngrediente.Nombre} para la cerveza {cervezaExistente.Nombre} ");

        //    try
        //    {
        //        bool resultadoAccion = await _cervezaRepository
        //            .CreateBeerIngredientAsync(cerveza_id, unIngrediente);

        //        if (!resultadoAccion)
        //            throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");
        //    }
        //    catch (DbOperationException)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<Cerveza> UpdateAsync(int cerveza_id, Cerveza unaCerveza)
        //{
        //    //Validamos que los parametros sean consistentes
        //    if (cerveza_id != unaCerveza.Id)
        //        throw new AppValidationException($"Inconsistencia en el Id de la cerveza a actualizar. Verifica argumentos");

        //    //Validamos que la cerveza exista con ese Id
        //    var cervezaExistente = await _cervezaRepository
        //        .GetByIdAsync(cerveza_id);

        //    if (cervezaExistente.Id ==0)
        //        throw new AppValidationException($"No existe una cerveza registrada con el id {unaCerveza.Id}");

        //    //Validamos que la cerveza tenga nombre
        //    if (unaCerveza.Nombre.Length == 0)
        //        throw new AppValidationException("No se puede actualizar una cerveza con nombre nulo");

        //    //Validamos que la cerveza tenga estilo
        //    if (unaCerveza.Estilo.Length == 0)
        //        throw new AppValidationException("No se puede insertar una cerveza con estilo nulo");

        //    //Validamos que el estilo exista
        //    var estiloExistente = await _estiloRepository
        //        .GetByNameAsync(unaCerveza.Estilo!);

        //    if (estiloExistente.Id ==0)
        //        throw new AppValidationException($"El estilo {unaCerveza.Estilo} no se encuentra registrado");

        //    unaCerveza.Estilo_id = estiloExistente.Id;

        //    //Validamos que la cerveza tenga asociada una cerveceria
        //    if (unaCerveza.Cerveceria.Length == 0)
        //        throw new AppValidationException("No se puede actualizar una cerveza sin una cerveceria");

        //    //Validamos que la cervecería exista
        //    var cerveceriaExistente = await _cerveceriaRepository
        //        .GetByNameAsync(unaCerveza.Cerveceria!);

        //    if (cerveceriaExistente.Id == 0)
        //        throw new AppValidationException($"La cervecería {unaCerveza.Cerveceria} no se encuentra registrada");

        //    unaCerveza.Cerveceria_id = cerveceriaExistente.Id;

        //    //Validamos que haya al menos un cambio en las propiedades
        //    if (unaCerveza.Equals(cervezaExistente))
        //        throw new AppValidationException("No hay cambios en los atributos de la cerveza. No se realiza actualización.");

        //    try
        //    {
        //        bool resultadoAccion = await _cervezaRepository
        //            .UpdateAsync(unaCerveza);

        //        if (!resultadoAccion)
        //            throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

        //        cervezaExistente = await _cervezaRepository
        //            .GetByNameAndBreweryAsync(unaCerveza.Nombre!, unaCerveza.Cerveceria);
        //    }
        //    catch (DbOperationException)
        //    {
        //        throw ;
        //    }

        //    return cervezaExistente;
        //}

        //public async Task DeleteAsync(int cerveza_id)
        //{
        //    // validamos que el cerveza a eliminar si exista con ese Id
        //    var cervezaExistente = await _cervezaRepository
        //        .GetByIdAsync(cerveza_id);

        //    if (cervezaExistente.Id ==0)
        //        throw new AppValidationException($"No existe una cerveceria con el Id {cerveza_id} que se pueda eliminar");

        //    try
        //    {
        //        //Si existe y tiene ingredientes asociados, se eliminan los ingredientes asociados

        //        bool resultadoAccion = await _cervezaRepository
        //            .DeleteAsync(cervezaExistente);

        //        if (!resultadoAccion)
        //            throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");
        //    }
        //    catch (DbOperationException)
        //    {
        //        throw;
        //    }
        //}

        //public async Task DeleteBeerPackagingAsync(int cerveza_id, EnvasadoCerveza unEnvasadoCerveza)
        //{
        //    //Validamos que la cerveza referenciada en el EnvasadoCerveza exista
        //    var cervezaExistente = await _cervezaRepository
        //        .GetByNameAndBreweryAsync(unEnvasadoCerveza.Cerveza, unEnvasadoCerveza.Cerveceria);

        //    if (cervezaExistente.Id != cerveza_id)
        //        throw new AppValidationException($"Inconsistencia en los parámetros. " +
        //            $"Los Ids de las cervezas no coinciden entre si.");

        //    //Validamos que el envasado tenga nombre
        //    if (unEnvasadoCerveza.Envasado.Length == 0)
        //        throw new AppValidationException("No se puede encontrar un envasado de cerveza para eliminar con nombre nulo");

        //    //Validamos que la unidad de volumen tenga nombre
        //    if (unEnvasadoCerveza.Unidad_Volumen.Length == 0)
        //        throw new AppValidationException("No se puede encontrar un envasado de cerveza para eliminar con unidad de volumen nula");

        //    //Validamos que el envasado exista
        //    var envasadoExistente = await _envasadoRepository
        //        .GetByNameAsync(unEnvasadoCerveza.Envasado!);

        //    if (envasadoExistente.Id == 0)
        //        throw new AppValidationException($"El envasado {unEnvasadoCerveza.Envasado} no se encuentra registrado.");

        //    unEnvasadoCerveza.Id = envasadoExistente.Id;

        //    //Validamos que la unidad de volumen exista
        //    var unidadVolumenExistente = await _unidadVolumenRepository
        //        .GetByNameAsync(unEnvasadoCerveza.Unidad_Volumen!);

        //    if (unidadVolumenExistente.Id ==0)
        //        throw new AppValidationException($"La unidad de Volumen {unEnvasadoCerveza.Unidad_Volumen} no se encuentra registrada.");

        //    if (unEnvasadoCerveza.Volumen <= 0)
        //        throw new AppValidationException($"el Volumen {unEnvasadoCerveza.Volumen} no corresponde a un valor válido para un envasado.");

        //    try
        //    {
        //        bool resultadoAccion = await _cervezaRepository
        //            .DeleteBeerPackagingAsync(cerveza_id, unEnvasadoCerveza);

        //        if (!resultadoAccion)
        //            throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

        //    }
        //    catch (DbOperationException)
        //    {
        //        throw;
        //    }
        //}

        //public async Task DeleteBeerIngredientAsync(int cerveza_id, Ingrediente unIngrediente)
        //{
        //    //Validamos que la cerveza exista con ese Id
        //    var cervezaExistente = await _cervezaRepository
        //        .GetByIdAsync(cerveza_id);

        //    if (cervezaExistente.Id ==0)
        //        throw new AppValidationException($"No existe una cerveza registrada con el id {cerveza_id}");

        //    //Validamos que el ingrediente tenga nombre
        //    if (unIngrediente.Nombre.Length == 0)
        //        throw new AppValidationException("No se puede validar un ingrediente de cerveza con nombre nulo");

        //    //Validamos que el tipo de ingrediente tenga nombre
        //    if (unIngrediente.Tipo_Ingrediente.Length == 0)
        //        throw new AppValidationException("No se puede validar un ingrediente de cerveza con tipo de ingrediente nulo");

        //    //Validamos que el ingrediente exista
        //    var ingredienteExistente = await _ingredienteRepository
        //        .GetByNameAndTypeAsync(unIngrediente.Nombre!, unIngrediente.Tipo_Ingrediente);

        //    if (ingredienteExistente.Id==0)
        //        throw new AppValidationException($"El ingrediente {unIngrediente.Tipo_Ingrediente} - {unIngrediente.Nombre} no se encuentra registrado.");

        //    //Validamos que este ingrediente no exista para esta cerveza
        //    var CervezaConIngredienteExistente = await _ingredienteRepository
        //        .GetAssociatedBeerByIdAsync(ingredienteExistente.Id, cerveza_id);

        //    if (CervezaConIngredienteExistente.Id ==0)
        //        throw new AppValidationException($"No existe registro para el ingrediente {unIngrediente.Tipo_Ingrediente} " +
        //            $"- {unIngrediente.Nombre} para la cerveza {cervezaExistente.Nombre} ");

        //    try
        //    {
        //        bool resultadoAccion = await _cervezaRepository
        //            .DeleteBeerIngredientAsync(cerveza_id, unIngrediente);

        //        if (!resultadoAccion)
        //            throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");
        //    }
        //    catch (DbOperationException)
        //    {
        //        throw;
        //    }
        //}

        private async Task<CervezaDetallada> BuildDetailedBeerAsync(Cerveza cerveza)
        {
            CervezaDetallada cervezaDetallada = new()
            {
                Id = cerveza.Id,
                Nombre = cerveza.Nombre,
                Cerveceria = cerveza.Cerveceria,
                Cerveceria_id = cerveza.Cerveceria_id,
                Estilo = cerveza.Estilo,
                Estilo_id = cerveza.Estilo_id,
                Abv = cerveza.Abv
            };

            var ingredientesAsociados = await _cervezaRepository
                .GetAssociatedIngredientsAsync(cerveza.Id);

            cervezaDetallada.Ingredientes = ingredientesAsociados.ToList();

            return cervezaDetallada;
        }


        private static IEnumerable<Cerveza> ApplyOrder<T>(IEnumerable<Cerveza> cervezas, Func<Cerveza, T> criterio, string orden)
        {
            if (orden.ToLower().Equals("desc"))
                return cervezas.OrderByDescending(criterio);
            else
                return cervezas.OrderBy(criterio);
        }
    }
}
