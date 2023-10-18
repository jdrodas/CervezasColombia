using CervezasColombia_CS_API_Mongo.Helpers;
using CervezasColombia_CS_API_Mongo.Interfaces;
using CervezasColombia_CS_API_Mongo.Models;

namespace CervezasColombia_CS_API_Mongo.Services
{
    public class CervezaService
    {
        private readonly ICervezaRepository _cervezaRepository;
        private readonly ICerveceriaRepository _cerveceriaRepository;
        private readonly IEstiloRepository _estiloRepository;
        private readonly IEnvasadoRepository _envasadoRepository;
        //private readonly IUnidadVolumenRepository _unidadVolumenRepository;
        //private readonly IIngredienteRepository _ingredienteRepository;

        public CervezaService(ICervezaRepository cervezaRepository
            ,ICerveceriaRepository cerveceriaRepository
            ,IEstiloRepository estiloRepository
            ,IEnvasadoRepository envasadoRepository
            //,IUnidadVolumenRepository unidadVolumenRepository
            //,IIngredienteRepository ingredienteRepository
            )
        {
            _cervezaRepository = cervezaRepository;
            _cerveceriaRepository = cerveceriaRepository;
            _estiloRepository = estiloRepository;
            _envasadoRepository = envasadoRepository;
            //_unidadVolumenRepository = unidadVolumenRepository;
            //_ingredienteRepository = ingredienteRepository;
        }

        public async Task<IEnumerable<Cerveza>> GetAllAsync()
        {
            return await _cervezaRepository
                .GetAllAsync();
        }

        public async Task<Cerveza> GetByIdAsync(string cerveza_id)
        {
            //Validamos que el estilo exista con ese Id
            var unaCerveza = await _cervezaRepository
                .GetByIdAsync(cerveza_id);

            if (string.IsNullOrEmpty(unaCerveza.Id))
                throw new AppValidationException($"Cerveza no encontrada con el id {cerveza_id}");

            return unaCerveza;
        }

        //TODO: CervezaService: Obtener ingredientes asociados

        //public async Task<IEnumerable<Ingrediente>> GetAssociatedIngredientsAsync(int cerveza_id)
        //{
        //    //Validamos que la cerveza exista con ese Id
        //    var unaCerveza = await _cervezaRepository
        //        .GetByIdAsync(cerveza_id);

        //    if (unaCerveza.Id == 0)
        //        throw new AppValidationException($"Cerveza no encontrada con el id {cerveza_id}");

        //    //Si la cerveza existe, validamos que tenga ingredientes asociados
        //    var cantidadIngredientesAsociados = await _cervezaRepository
        //        .GetTotalAssociatedIngredientsAsync(cerveza_id);

        //    if (cantidadIngredientesAsociados == 0)
        //        throw new AppValidationException($"No Existen ingredientes asociados a la cerveza {unaCerveza.Nombre}");

        //    return await _cervezaRepository
        //        .GetAssociatedIngredientsAsync(cerveza_id);
        //}

        //TODO: CervezaService: Obtener envasados asociados

        //public async Task<IEnumerable<EnvasadoCerveza>> GetAssociatedPackagingsAsync(int cerveza_id)
        //{
        //    //Validamos que la cerveza exista con ese Id
        //    var unaCerveza = await _cervezaRepository
        //        .GetByIdAsync(cerveza_id);

        //    if (unaCerveza.Id == 0)
        //        throw new AppValidationException($"Cerveza no encontrada con el id {cerveza_id}");

        //    //Si la cerveza existe, validamos que tenga envasados asociados
        //    var cantidadEnvasadosAsociados = await _cervezaRepository
        //        .GetTotalAssociatedPackagingsAsync(cerveza_id);

        //    if (cantidadEnvasadosAsociados == 0)
        //        throw new AppValidationException($"No existen envasados asociados a la cerveza {unaCerveza.Nombre}");

        //    return await _cervezaRepository
        //        .GetAssociatedPackagingsAsync(cerveza_id);
        //}

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
                .GetByNameAsync(unaCerveza.Cerveceria!);

            if (string.IsNullOrEmpty(cerveceriaExistente.Id))
                throw new AppValidationException($"La cervecería {unaCerveza.Cerveceria} no se encuentra registrada");

            unaCerveza.Cerveceria_id = cerveceriaExistente.Id;

            //Validamos que la cerveza tenga estilo
            if (unaCerveza.Estilo.Length == 0)
                throw new AppValidationException("No se puede insertar una cerveza con estilo nulo");

            //Validamos que el estilo exista
            var estiloExistente = await _estiloRepository
                .GetByNameAsync(unaCerveza.Estilo!);

            if (string.IsNullOrEmpty(estiloExistente.Id))
                throw new AppValidationException($"El estilo {unaCerveza.Estilo} no se encuentra registrado");

            unaCerveza.Estilo_id = estiloExistente.Id;

            //Validar que no exista para esa cerveceria, una cerveza con ese nombre
            var cervezaExistente = await _cervezaRepository
                .GetByNameAndBreweryAsync(unaCerveza.Nombre!, unaCerveza.Cerveceria!);

            if (string.IsNullOrEmpty(cervezaExistente.Id) == false)
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
            catch (DbOperationException error)
            {
                throw error;
            }

            return (cervezaExistente);
        }

        //TODO: CervezaService: Crear envasados para cervezas

        //public async Task<EnvasadoCerveza> CreateBeerPackagingAsync(int cerveza_id, EnvasadoCerveza unEnvasadoCerveza)
        //{
        //    //Validamos que la cerveza exista con ese Id
        //    var cervezaExistente = await _cervezaRepository
        //        .GetByIdAsync(cerveza_id);

        //    if (cervezaExistente.Id == 0)
        //        throw new AppValidationException($"No existe una cerveza registrada con el id {cerveza_id}");

        //    //Validamos que el envasado tenga nombre
        //    if (unEnvasadoCerveza.Nombre.Length == 0)
        //        throw new AppValidationException("No se puede insertar un envasado de cerveza con nombre nulo");

        //    //Validamos que la unidad de volumen tenga nombre
        //    if (unEnvasadoCerveza.Unidad_Volumen.Length == 0)
        //        throw new AppValidationException("No se puede insertar un envasado de cerveza con unidad de volumen nula");

        //    //Validamos que el envasado exista
        //    var envasadoExistente = await _envasadoRepository
        //        .GetByNameAsync(unEnvasadoCerveza.Nombre!);

        //    if (envasadoExistente.Id == 0)
        //        throw new AppValidationException($"El envasado {unEnvasadoCerveza.Nombre} no se encuentra registrado.");

        //    unEnvasadoCerveza.Id = envasadoExistente.Id;

        //    //Validamos que la unidad de volumen exista
        //    var unidadVolumenExistente = await _unidadVolumenRepository
        //        .GetByNameAsync(unEnvasadoCerveza.Unidad_Volumen!);

        //    if (unidadVolumenExistente.Id == 0)
        //        throw new AppValidationException($"La unidad de Volumen {unEnvasadoCerveza.Unidad_Volumen} no se encuentra registrada.");

        //    unEnvasadoCerveza.Unidad_Volumen_Id = unidadVolumenExistente.Id;

        //    if (unEnvasadoCerveza.Volumen <= 0)
        //        throw new AppValidationException($"el Volumen {unEnvasadoCerveza.Volumen} no corresponde a un valor válido para un envasado.");

        //    //Validamos que este envasado con unidad de volumen y volumen no exista para esta cerveza
        //    var envasadoCervezaExistente = await _envasadoRepository
        //        .GetAssociatedBeerPackagingAsync(cerveza_id, unEnvasadoCerveza.Id, unEnvasadoCerveza.Unidad_Volumen_Id, unEnvasadoCerveza.Volumen);

        //    if (unEnvasadoCerveza.Equals(envasadoCervezaExistente))
        //        throw new AppValidationException($"Ya existe un envasado {unEnvasadoCerveza.Nombre} de {unEnvasadoCerveza.Volumen} " +
        //            $"{unEnvasadoCerveza.Unidad_Volumen} para la cerveza {cervezaExistente.Nombre} de {cervezaExistente.Cerveceria}.");

        //    try
        //    {
        //        bool resultadoAccion = await _cervezaRepository
        //            .CreateBeerPackagingAsync(cerveza_id, unEnvasadoCerveza);

        //        if (!resultadoAccion)
        //            throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

        //        envasadoCervezaExistente = await _cervezaRepository
        //            .GetPackagedBeerByNameAsync(cerveza_id, unEnvasadoCerveza.Nombre!, unEnvasadoCerveza.Unidad_Volumen_Id, unEnvasadoCerveza.Volumen);
        //    }
        //    catch (DbOperationException error)
        //    {
        //        throw error;
        //    }

        //    return envasadoCervezaExistente;
        //}

        //TODO: CervezaService: Crear envasados para cervezas 

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

        //    if (ingredienteExistente.Id == 0)
        //        throw new AppValidationException($"El ingrediente {unIngrediente.Tipo_Ingrediente} - {unIngrediente.Nombre} no se encuentra registrado.");

        //    unIngrediente.Id = ingredienteExistente.Id;

        //    //Validamos que este ingrediente no exista para esta cerveza
        //    var CervezaConIngredienteExistente = await _ingredienteRepository
        //        .GetAssociatedBeerByIdAsync(unIngrediente.Id, cerveza_id);

        //    if (CervezaConIngredienteExistente.Id != 0)
        //        throw new AppValidationException($"Ya existe registro para el ingrediente {unIngrediente.Tipo_Ingrediente} " +
        //            $"- {unIngrediente.Nombre} para la cerveza {cervezaExistente.Nombre} ");

        //    try
        //    {
        //        bool resultadoAccion = await _cervezaRepository
        //            .CreateBeerIngredientAsync(cerveza_id, unIngrediente);

        //        if (!resultadoAccion)
        //            throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");
        //    }
        //    catch (DbOperationException error)
        //    {
        //        throw error;
        //    }
        //}

        //TODO: CervezaService: Actualizar cerveza

        //public async Task<Cerveza> UpdateAsync(int cerveza_id, Cerveza unaCerveza)
        //{
        //    //Validamos que los parametros sean consistentes
        //    if (cerveza_id != unaCerveza.Id)
        //        throw new AppValidationException($"Inconsistencia en el Id de la cerveza a actualizar. Verifica argumentos");

        //    //Validamos que la cerveza exista con ese Id
        //    var cervezaExistente = await _cervezaRepository
        //        .GetByIdAsync(cerveza_id);

        //    if (cervezaExistente.Id == 0)
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

        //    if (estiloExistente.Id == 0)
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
        //    catch (DbOperationException error)
        //    {
        //        throw error;
        //    }

        //    return cervezaExistente;
        //}

        //TODO: CervezaService: Borrar cerveza

        //public async Task DeleteAsync(int cerveza_id)
        //{
        //    // validamos que el cerveza a eliminar si exista con ese Id
        //    var cervezaExistente = await _cervezaRepository
        //        .GetByIdAsync(cerveza_id);

        //    if (cervezaExistente.Id == 0)
        //        throw new AppValidationException($"No existe una cerveceria con el Id {cerveza_id} que se pueda eliminar");

        //    //Si existe y no tiene cervezas asociadas, se puede eliminar
        //    try
        //    {
        //        bool resultadoAccion = await _cervezaRepository
        //            .DeleteAsync(cervezaExistente);

        //        if (!resultadoAccion)
        //            throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");
        //    }
        //    catch (DbOperationException error)
        //    {
        //        throw error;
        //    }
        //}

        //TODO: CervezaService: Borrar envasado Cervezas

        //public async Task DeleteBeerPackagingAsync(int cerveza_id, EnvasadoCerveza unEnvasadoCerveza)
        //{
        //    //Validamos que la cerveza exista con ese Id
        //    var cervezaExistente = await _cervezaRepository
        //        .GetByIdAsync(cerveza_id);

        //    if (cervezaExistente.Id == 0)
        //        throw new AppValidationException($"No existe una cerveza registrada con el id {cerveza_id}");

        //    //Validamos que el envasado tenga nombre
        //    if (unEnvasadoCerveza.Nombre.Length == 0)
        //        throw new AppValidationException("No se puede encontrar un envasado de cerveza para eliminar con nombre nulo");

        //    //Validamos que la unidad de volumen tenga nombre
        //    if (unEnvasadoCerveza.Unidad_Volumen.Length == 0)
        //        throw new AppValidationException("No se puede encontrar un envasado de cerveza para eliminar con unidad de volumen nula");

        //    //Validamos que el envasado exista
        //    var envasadoExistente = await _envasadoRepository
        //        .GetByNameAsync(unEnvasadoCerveza.Nombre!);

        //    if (envasadoExistente.Id == 0)
        //        throw new AppValidationException($"El envasado {unEnvasadoCerveza.Nombre} no se encuentra registrado.");

        //    unEnvasadoCerveza.Id = envasadoExistente.Id;

        //    //Validamos que la unidad de volumen exista
        //    var unidadVolumenExistente = await _unidadVolumenRepository
        //        .GetByNameAsync(unEnvasadoCerveza.Unidad_Volumen!);

        //    if (unidadVolumenExistente.Id == 0)
        //        throw new AppValidationException($"La unidad de Volumen {unEnvasadoCerveza.Unidad_Volumen} no se encuentra registrada.");

        //    unEnvasadoCerveza.Unidad_Volumen_Id = unidadVolumenExistente.Id;

        //    if (unEnvasadoCerveza.Volumen <= 0)
        //        throw new AppValidationException($"el Volumen {unEnvasadoCerveza.Volumen} no corresponde a un valor válido para un envasado.");

        //    //Validamos que este envasado con unidad de volumen y volumen exista para esta cerveza
        //    var envasadoCervezaExistente = await _envasadoRepository
        //        .GetAssociatedBeerPackagingAsync(cerveza_id, unEnvasadoCerveza.Id, unEnvasadoCerveza.Unidad_Volumen_Id, unEnvasadoCerveza.Volumen);

        //    if (!unEnvasadoCerveza.Equals(envasadoCervezaExistente))
        //        throw new AppValidationException($"Ya existe un envasado {unEnvasadoCerveza.Nombre} de {unEnvasadoCerveza.Volumen} " +
        //            $"{unEnvasadoCerveza.Unidad_Volumen} para la cerveza {cervezaExistente.Nombre} de {cervezaExistente.Cerveceria}.");

        //    try
        //    {
        //        bool resultadoAccion = await _cervezaRepository
        //            .DeleteBeerPackagingAsync(cerveza_id, unEnvasadoCerveza);

        //        if (!resultadoAccion)
        //            throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

        //    }
        //    catch (DbOperationException error)
        //    {
        //        throw error;
        //    }
        //}

        //TODO: CervezaService: Borrar envasado Cervezas

        //public async Task DeleteBeerIngredientAsync(int cerveza_id, Ingrediente unIngrediente)
        //{
        //    //Validamos que la cerveza exista con ese Id
        //    var cervezaExistente = await _cervezaRepository
        //        .GetByIdAsync(cerveza_id);

        //    if (cervezaExistente.Id == 0)
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

        //    if (ingredienteExistente.Id == 0)
        //        throw new AppValidationException($"El ingrediente {unIngrediente.Tipo_Ingrediente} - {unIngrediente.Nombre} no se encuentra registrado.");

        //    unIngrediente.Id = ingredienteExistente.Id;

        //    //Validamos que este ingrediente no exista para esta cerveza
        //    var CervezaConIngredienteExistente = await _ingredienteRepository
        //        .GetAssociatedBeerByIdAsync(unIngrediente.Id, cerveza_id);

        //    if (CervezaConIngredienteExistente.Id == 0)
        //        throw new AppValidationException($"No existe registro para el ingrediente {unIngrediente.Tipo_Ingrediente} " +
        //            $"- {unIngrediente.Nombre} para la cerveza {cervezaExistente.Nombre} ");

        //    try
        //    {
        //        bool resultadoAccion = await _cervezaRepository
        //            .DeleteBeerIngredientAsync(cerveza_id, unIngrediente);

        //        if (!resultadoAccion)
        //            throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");
        //    }
        //    catch (DbOperationException error)
        //    {
        //        throw error;
        //    }
        //}
    }
}
