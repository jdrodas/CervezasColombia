using CervezasColombia_CS_API_PostgreSQL_Dapper.Helpers;
using CervezasColombia_CS_API_PostgreSQL_Dapper.Interfaces;
using CervezasColombia_CS_API_PostgreSQL_Dapper.Models;

namespace CervezasColombia_CS_API_PostgreSQL_Dapper.Services
{
    public class CervezaService
    {
        private readonly ICervezaRepository _cervezaRepository;
        private readonly ICerveceriaRepository _cerveceriaRepository;
        private readonly IEstiloRepository _estiloRepository;

        public CervezaService(ICervezaRepository cervezaRepository,
                              ICerveceriaRepository cerveceriaRepository,
                              IEstiloRepository estiloRepository)
        {
            _cervezaRepository = cervezaRepository;
            _cerveceriaRepository = cerveceriaRepository;
            _estiloRepository = estiloRepository;
        }

        public async Task<IEnumerable<Cerveza>> GetAllAsync()
        {
            return await _cervezaRepository
                .GetAllAsync();
        }

        public async Task<Cerveza> GetByIdAsync(int cerveza_id)
        {
            //Validamos que el estilo exista con ese Id
            var unaCerveza = await _cervezaRepository
                .GetByIdAsync(cerveza_id);

            if (unaCerveza.Id == 0)
                throw new AppValidationException($"Cerveza no encontrada con el id {cerveza_id}");

            return unaCerveza;
        }

        public async Task<IEnumerable<Ingrediente>> GetAssociatedIngredientsAsync(int cerveza_id)
        {
            //Validamos que la cerveza exista con ese Id
            var unaCerveza = await _cervezaRepository
                .GetByIdAsync(cerveza_id);

            if (unaCerveza.Id == 0)
                throw new AppValidationException($"Cerveza no encontrada con el id {cerveza_id}");

            //Si la cerveza existe, validamos que tenga ingredientes asociados
            var cantidadIngredientesAsociados = await _cervezaRepository
                .GetTotalAssociatedIngredientsAsync(cerveza_id);

            if (cantidadIngredientesAsociados == 0)
                throw new AppValidationException($"No Existen ingredientes asociados a la cerveza {unaCerveza.Nombre}");

            return await _cervezaRepository
                .GetAssociatedIngredientsAsync(cerveza_id);
        }

        public async Task<IEnumerable<EnvasadoCerveza>> GetAssociatedPackagingsAsync(int cerveza_id)
        {
            //Validamos que la cerveza exista con ese Id
            var unaCerveza = await _cervezaRepository
                .GetByIdAsync(cerveza_id);

            if (unaCerveza.Id == 0)
                throw new AppValidationException($"Cerveza no encontrada con el id {cerveza_id}");

            //Si la cerveza existe, validamos que tenga envasados asociados
            var cantidadEnvasadosAsociados = await _cervezaRepository
                .GetTotalAssociatedPackagingsAsync(cerveza_id);

            if (cantidadEnvasadosAsociados == 0)
                throw new AppValidationException($"No existen envasados asociados a la cerveza {unaCerveza.Nombre}");

            return await _cervezaRepository
                .GetAssociatedPackagingsAsync(cerveza_id);
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
                .GetByNameAsync(unaCerveza.Cerveceria!);

            if (cerveceriaExistente.Id == 0)
                throw new AppValidationException($"La cervecería {unaCerveza.Cerveceria} no se encuentra registrada");

            unaCerveza.Cerveceria_id = cerveceriaExistente.Id;

            //Validamos que la cerveza tenga estilo
            if (unaCerveza.Estilo.Length == 0)
                throw new AppValidationException("No se puede insertar una cerveza con estilo nulo");

            //Validamos que el estilo exista
            var estiloExistente = await _estiloRepository
                .GetByNameAsync(unaCerveza.Estilo!);

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
            catch (DbOperationException error)
            {
                throw error;
            }

            return (cervezaExistente);
        }

        public async Task<Cerveza>  UpdateAsync(int cerveza_id, Cerveza unaCerveza)
        {
            //Validamos que los parametros sean consistentes
            if (cerveza_id != unaCerveza.Id)
                throw new AppValidationException($"Inconsistencia en el Id de la cerveza a actualizar. Verifica argumentos");

            //Validamos que la cerveza exista con ese Id
            var cervezaExistente = await _cervezaRepository
                .GetByIdAsync(cerveza_id);

            if (cervezaExistente.Id == 0)
                throw new AppValidationException($"No existe una cerveza registrada con el id {unaCerveza.Id}");

            //Validamos que la cerveza tenga nombre
            if (unaCerveza.Nombre.Length == 0)
                throw new AppValidationException("No se puede actualizar una cerveza con nombre nulo");

            //Validamos que la cerveza tenga estilo
            if (unaCerveza.Estilo.Length == 0)
                throw new AppValidationException("No se puede insertar una cerveza con estilo nulo");

            //Validamos que el estilo exista
            var estiloExistente = await _estiloRepository
                .GetByNameAsync(unaCerveza.Estilo!);

            if (estiloExistente.Id == 0)
                throw new AppValidationException($"El estilo {unaCerveza.Estilo} no se encuentra registrado");

            unaCerveza.Estilo_id = estiloExistente.Id;

            //Validamos que la cerveza tenga asociada una cerveceria
            if (unaCerveza.Cerveceria.Length == 0)
                throw new AppValidationException("No se puede actualizar una cerveza sin una cerveceria");

            //Validamos que la cervecería exista
            var cerveceriaExistente = await _cerveceriaRepository
                .GetByNameAsync(unaCerveza.Cerveceria!);

            if (cerveceriaExistente.Id == 0)
                throw new AppValidationException($"La cervecería {unaCerveza.Cerveceria} no se encuentra registrada");

            unaCerveza.Cerveceria_id = cerveceriaExistente.Id;

            //Validamos que haya al menos un cambio en las propiedades
            if (unaCerveza.Equals(cervezaExistente))
                throw new AppValidationException("No hay cambios en los atributos de la cerveza. No se realiza actualización.");

            try
            {
                bool resultadoAccion = await _cervezaRepository
                    .UpdateAsync(unaCerveza);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                cervezaExistente = await _cervezaRepository
                    .GetByNameAndBreweryAsync(unaCerveza.Nombre!, unaCerveza.Cerveceria);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return cervezaExistente;
        }

        public async Task DeleteAsync(int cerveza_id)
        {
            // validamos que el cerveza a eliminar si exista con ese Id
            var cervezaExistente = await _cervezaRepository
                .GetByIdAsync(cerveza_id);

            if (cervezaExistente.Id == 0)
                throw new AppValidationException($"No existe una cerveceria con el Id {cerveza_id} que se pueda eliminar");

            //Si existe y no tiene cervezas asociadas, se puede eliminar
            try
            {
                bool resultadoAccion = await _cervezaRepository
                    .DeleteAsync(cervezaExistente);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");
            }
            catch (DbOperationException error)
            {
                throw error;
            }
        }
    }
}
