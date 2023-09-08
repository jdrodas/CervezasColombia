using CervezasColombia_CS_API_SQLite_Dapper.Data.Entities;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;

namespace CervezasColombia_CS_API_SQLite_Dapper.Services
{
    public class EstiloService
    {
        private readonly IEstiloRepository _estiloRepository;

        public EstiloService(IEstiloRepository estiloRepository)
        {
            _estiloRepository = estiloRepository;
        }
        public async Task<IEnumerable<Estilo>> GetAllAsync()
        {
            return await _estiloRepository.GetAllAsync();
        }

        public async Task<Estilo> GetByIdAsync(int id)
        {
            //Validamos que el estilo exista con ese Id
            var unEstilo = await _estiloRepository.GetByIdAsync(id);

            if (unEstilo.Id ==0)
                throw new AppValidationException($"Estilo no encontrado con el id {id}");

            return unEstilo;
        }

        public async Task<IEnumerable<Cerveza>> GetBeersByStyleAsync(int id)
        {
            //Validamos que el estilo exista con ese Id
            var unEstilo = await _estiloRepository.GetByIdAsync(id);

            if (unEstilo.Id == 0)
                throw new AppValidationException($"Estilo no encontrado con el id {id}");

            //Si el estilo existe, validamos que tenga cervezas asociadas
            // Validamos que el estilo no tenga asociadas cervezas
            var cantidadCervezasAsociadas = await _estiloRepository.GetTotalBeersByStyleAsync(id);

            if (cantidadCervezasAsociadas == 0)
                throw new AppValidationException($"No Existen cervezas asociadas al estilo {unEstilo.Nombre}");

            return await _estiloRepository.GetBeersByStyleAsync(id);
        }


        public async Task CreateAsync(Estilo unEstilo)
        {
            // validamos que el estilo a crear no esté previamente creado
            var estiloExistente = await _estiloRepository.GetByNameAsync(unEstilo.Nombre!);

            if(estiloExistente.Id !=0)
                throw new AppValidationException($"Ya existe un estilo con el nombre {unEstilo.Nombre}");

            try
            {
                await _estiloRepository.CreateAsync(unEstilo);
            }
            catch (DbOperationException error)
            {
                throw error;
            }            
        }

        public async Task UpdateAsync(int id, Estilo unEstilo)
        {
            //Validamos que los parametros sean consistentes
            if(id != unEstilo.Id)
                throw new AppValidationException($"Inconsistencia en el Id del estilo a actualizar. Verifica argumentos");

            //Validamos que el nuevo nombre no exista previamente con otro Id
            var estiloExistente = await _estiloRepository.GetByNameAsync(unEstilo.Nombre!);

            if (estiloExistente.Id != 0)
                throw new AppValidationException($"Ya existe un estilo con el nombre {unEstilo.Nombre}");

            // validamos que el estilo a actualizar si exista con ese Id
            estiloExistente = await _estiloRepository.GetByIdAsync(unEstilo.Id);

            if (estiloExistente.Id == 0)
                throw new AppValidationException($"No existe un estilo con el Id {unEstilo.Id} que se pueda actualizar");

            try 
            {
                await _estiloRepository.UpdateAsync(unEstilo);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

        }

        public async Task<Estilo> DeleteAsync(int id)
        {
            // validamos que el estilo a eliminar si exista con ese Id
            var estiloExistente = await _estiloRepository.GetByIdAsync(id);

            if (estiloExistente.Id == 0)
                throw new AppValidationException($"No existe un estilo con el Id {id} que se pueda eliminar");

            // Validamos que el estilo no tenga asociadas cervezas
            var cantidadCervezasAsociadas = await _estiloRepository.GetTotalBeersByStyleAsync(id);

            if (cantidadCervezasAsociadas > 0)
                throw new AppValidationException($"Existen {cantidadCervezasAsociadas} cervezas " +
                    $"asociadas a ese estilo. No se puede eliminar");

            //Si existe y no tiene cervezas asociadas, se puede eliminar
            try 
            {
                await _estiloRepository.DeleteAsync(id);

                return estiloExistente;
            }
            catch (DbOperationException error)
            {
                throw error;
            }
        }
    }
}
