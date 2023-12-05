using CervezasColombia_CS_API_SQLite_Dapper.Cervecerias;
using CervezasColombia_CS_API_SQLite_Dapper.Cervezas;

namespace CervezasColombia_CS_API_SQLite_Dapper.Estilos
{
    public class EstiloService(IEstiloRepository estiloRepository,
                        ICervezaRepository cervezaRepository)
    {
        private readonly IEstiloRepository _estiloRepository = estiloRepository;
        private readonly ICervezaRepository _cervezaRepository = cervezaRepository;

        public async Task<IEnumerable<Estilo>> GetAllAsync()
        {
            return await _estiloRepository
                .GetAllAsync();
        }

        public async Task<EstiloResponse> GetByIdAsync(int estilo_id)
        {
            //Validamos que el estilo exista con ese Id
            var unEstilo = await _estiloRepository
                .GetByIdAsync(estilo_id);

            if (unEstilo.Id == 0)
                throw new AppValidationException($"Estilo no encontrado con el id {estilo_id}");

            var unEstiloDetallado = await BuildEstiloResponseAsync(unEstilo);
            return unEstiloDetallado;
        }

        public async Task<EstiloResponse> GetByNameAsync(string estilo_name)
        {
            //Validamos que la Cerveceria exista con ese nombre
            var unEstilo = await _estiloRepository
                .GetByNameAsync(estilo_name);

            if (unEstilo.Id == 0)
                throw new AppValidationException($"Cerveceria no encontrada con el nombre {estilo_name}");

            var unEstiloDetallado = await BuildEstiloResponseAsync(unEstilo);
            return unEstiloDetallado;
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int estilo_id)
        {
            //Validamos que el estilo exista con ese Id
            var unEstilo = await _estiloRepository
                .GetByIdAsync(estilo_id);

            if (unEstilo.Id == 0)
                throw new AppValidationException($"Estilo no encontrado con el id {estilo_id}");

            //Si el estilo existe, validamos que tenga cervezas asociadas
            // Validamos que el estilo no tenga asociadas cervezas
            var cantidadCervezasAsociadas = await _estiloRepository
                .GetTotalAssociatedBeersAsync(estilo_id);

            if (cantidadCervezasAsociadas == 0)
                throw new AppValidationException($"No existen cervezas asociadas al estilo {unEstilo.Nombre}");

            var lasCervezas = await _estiloRepository
                .GetAssociatedBeersAsync(estilo_id);

            //Colocamos los valores de los rangos a las cervezas
            foreach (Cerveza unaCerveza in lasCervezas)
                unaCerveza.Rango_Abv = await _cervezaRepository
                    .GetAbvRangeNameAsync(unaCerveza.Abv);

            return lasCervezas;
        }

        public async Task<Estilo> CreateAsync(Estilo unEstilo)
        {
            //Validamos que el estilo tenga nombre
            if (unEstilo.Nombre.Length == 0)
                throw new AppValidationException("No se puede insertar un estilo con nombre nulo");

            // validamos que el estilo a crear no esté previamente creado
            var estiloExistente = await _estiloRepository
                .GetByNameAsync(unEstilo.Nombre!);

            if (estiloExistente.Id != 0)
                throw new AppValidationException($"Ya existe un estilo con el nombre {unEstilo.Nombre}");

            try
            {
                bool resultadoAccion = await _estiloRepository
                    .CreateAsync(unEstilo);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                estiloExistente = await _estiloRepository
                    .GetByNameAsync(unEstilo.Nombre!);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return estiloExistente;
        }

        public async Task<Estilo> UpdateAsync(int estilo_id, Estilo unEstilo)
        {
            //Validamos que los parametros sean consistentes
            if (estilo_id != unEstilo.Id)
                throw new AppValidationException($"Inconsistencia en el Id del estilo a actualizar. Verifica argumentos");

            //Validamos que el estilo tenga nombre
            if (unEstilo.Nombre.Length == 0)
                throw new AppValidationException($"No se puede actualizar el estilo {unEstilo.Id} para que tenga nombre nulo");

            //Validamos que el nuevo nombre no exista previamente con otro Id
            var estiloExistente = await _estiloRepository
                .GetByNameAsync(unEstilo.Nombre!);

            if (estiloExistente.Id != 0)
                throw new AppValidationException($"Ya existe un estilo con el nombre {unEstilo.Nombre}");

            // validamos que el estilo a actualizar si exista con ese Id
            estiloExistente = await _estiloRepository
                .GetByIdAsync(unEstilo.Id);

            if (estiloExistente.Id == 0)
                throw new AppValidationException($"No existe un estilo con el Id {unEstilo.Id} que se pueda actualizar");

            try
            {
                bool resultadoAccion = await _estiloRepository
                    .UpdateAsync(unEstilo);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                estiloExistente = await _estiloRepository
                    .GetByNameAsync(unEstilo.Nombre!);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return estiloExistente;
        }

        public async Task DeleteAsync(int estilo_id)
        {
            // validamos que el estilo a eliminar si exista con ese Id
            var estiloExistente = await _estiloRepository
                .GetByIdAsync(estilo_id);

            if (estiloExistente.Id == 0)
                throw new AppValidationException($"No existe un estilo con el Id {estilo_id} que se pueda eliminar");

            var cantidadCervezasAsociadas = await _estiloRepository
                .GetTotalAssociatedBeersAsync(estiloExistente.Id);

            //Para este caso, el Estilo es considerado atributo secundario.
            //Si hay cervezas asociadas, no se puede borrar el estilo
            if (cantidadCervezasAsociadas > 0)
                throw new AppValidationException($"Existen {cantidadCervezasAsociadas} cervezas " +
                    $"asociadas al estilo {estiloExistente.Nombre}. No se puede eliminar");

            //Si existe y no tiene cervezas asociadas, se puede eliminar
            try
            {
                bool resultadoAccion = await _estiloRepository
                    .DeleteAsync(estiloExistente);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");
            }
            catch (DbOperationException)
            {
                throw;
            }
        }

        private async Task<EstiloResponse> BuildEstiloResponseAsync(Estilo estilo)
        {
            //TODO: Modificar para implementar patrón decorador
            EstiloResponse estiloResponse = new()
            {
                Id = estilo.Id,
                Nombre = estilo.Nombre
            };

            var cervezasAsociadas = await _estiloRepository
                .GetAssociatedBeersAsync(estilo.Id);
            estiloResponse.Cervezas = cervezasAsociadas.ToList();

            //Colocamos los valores del rango ABV a las cervezas
            foreach (Cerveza unaCerveza in estiloResponse.Cervezas)
                unaCerveza.Rango_Abv = await _cervezaRepository
                    .GetAbvRangeNameAsync(unaCerveza.Abv);

            return estiloResponse;
        }
    }
}
