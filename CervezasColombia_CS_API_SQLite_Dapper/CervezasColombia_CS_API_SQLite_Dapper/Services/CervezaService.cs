using CervezasColombia_CS_API_SQLite_Dapper.Models;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;

namespace CervezasColombia_CS_API_SQLite_Dapper.Services
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
            return await _cervezaRepository.GetAllAsync();
        }

        public async Task<Cerveza> GetByIdAsync(int id)
        {
            //Validamos que el estilo exista con ese Id
            var unaCerveza = await _cervezaRepository.GetByIdAsync(id);

            if (unaCerveza.Id == 0)
                throw new AppValidationException($"Cerveza no encontrada con el id {id}");

            return unaCerveza;
        }

        public async Task<IEnumerable<Ingrediente>> GetAssociatedIngredientsAsync(int id)
        {
            //Validamos que la cerveza exista con ese Id
            var unaCerveza = await _cervezaRepository.GetByIdAsync(id);

            if (unaCerveza.Id == 0)
                throw new AppValidationException($"Cerveza no encontrada con el id {id}");

            //Si la cerveza existe, validamos que tenga ingredientes asociados
            var cantidadIngredientesAsociados = await _cervezaRepository.GetTotalAssociatedIngredientsAsync(id);

            if (cantidadIngredientesAsociados == 0)
                throw new AppValidationException($"No Existen ingredientes asociados a la cerveza {unaCerveza.Nombre}");

            return await _cervezaRepository.GetAssociatedIngredientsAsync(id);
        }

        public async Task<IEnumerable<Envasado>> GetAssociatedPackagingsAsync(int id)
        {
            //Validamos que la cerveza exista con ese Id
            var unaCerveza = await _cervezaRepository.GetByIdAsync(id);

            if (unaCerveza.Id == 0)
                throw new AppValidationException($"Cerveza no encontrada con el id {id}");

            //Si la cerveza existe, validamos que tenga envasados asociados
            var cantidadEnvasadosAsociados = await _cervezaRepository.GetTotalAssociatedPackagingsAsync(id);

            if (cantidadEnvasadosAsociados == 0)
                throw new AppValidationException($"No existen envasados asociados a la cerveza {unaCerveza.Nombre}");

            return await _cervezaRepository.GetAssociatedPackagingsAsync(id);
        }

        public async Task CreateAsync(Cerveza unaCerveza)
        {
            //Validamos que la cerveza tenga nombre
            if (unaCerveza.Nombre.Length == 0)
                throw new AppValidationException("No se puede insertar una cerveza con nombre nulo");

            //Validamos que la cerveza tenga asociada una cerveceria
            if (unaCerveza.Cerveceria.Length == 0)
                throw new AppValidationException("No se puede insertar una cerveza sin una cerveceria");

            //Validamos que la cervecería exista
            var cerveceriaExistente = await _cerveceriaRepository.GetByNameAsync(unaCerveza.Cerveceria!);

            if (cerveceriaExistente.Id == 0)
                throw new AppValidationException($"La cervecería {unaCerveza.Cerveceria} no se encuentra registrada");

            unaCerveza.Cerveceria_id = cerveceriaExistente.Id;

            //Validamos que la cerveza teng estilo
            if(unaCerveza.Estilo.Length == 0)
                throw new AppValidationException("No se puede insertar una cerveza con estilo nulo");

            //Validamos que el estilo exista
            var estiloExistente = await _estiloRepository.GetByNameAsync(unaCerveza.Estilo!);

            if (estiloExistente.Id == 0)
                throw new AppValidationException($"El estilo {unaCerveza.Estilo} no se encuentra registrado");

            unaCerveza.Estilo_id = estiloExistente.Id;

            //Validar que no exista para esa cerveceria, una cerveza con ese nombre
            int totalCerveceriasAsociadas = await _cervezaRepository
                .GetTotalAssociatedBreweriesAsync(unaCerveza.Nombre!, unaCerveza.Cerveceria);

            if (totalCerveceriasAsociadas > 0)
                throw new AppValidationException($"Ya existe la cerveza {unaCerveza.Nombre} " +
                    $"para la cerveceria {unaCerveza.Cerveceria}");

            try
            {
                bool resultadoAccion = await _cervezaRepository.CreateAsync(unaCerveza);

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
