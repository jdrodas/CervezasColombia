using CervezasColombia_CS_API_SQLite_Dapper.Models;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;

namespace CervezasColombia_CS_API_SQLite_Dapper.Services
{
    public class CervezaService
    {
        private readonly ICervezaRepository _cervezaRepository;

        public CervezaService(ICervezaRepository cervezaRepository)
        {
            _cervezaRepository = cervezaRepository;
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
    }
}
