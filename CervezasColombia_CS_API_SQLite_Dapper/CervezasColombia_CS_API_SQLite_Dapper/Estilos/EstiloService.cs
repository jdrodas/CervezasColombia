using CervezasColombia_CS_API_SQLite_Dapper.Cervezas;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;

namespace CervezasColombia_CS_API_SQLite_Dapper.Estilos
{
    public class EstiloService(IEstiloRepository estiloRepository,
                        ICervezaRepository cervezaRepository)
    {
        private readonly IEstiloRepository _estiloRepository = estiloRepository;
        private readonly ICervezaRepository _cervezaRepository = cervezaRepository;

        public async Task<EstiloResponse> GetAllAsync(EstiloQueryParameters parametrosConsultaEstilo)
        {
            var losEstilos = await _estiloRepository
                .GetAllAsync();

            // Calculamos items totales y cantidad de páginas
            var totalElementos = losEstilos.Count();
            var totalPaginas = (int)Math.Ceiling((double)totalElementos / parametrosConsultaEstilo.ElementosPorPagina);

            //Validamos que la página solicitada está dentro del rango permitido
            if (parametrosConsultaEstilo.Pagina > totalPaginas && totalPaginas > 0)
                throw new AppValidationException($"La página solicitada No. {parametrosConsultaEstilo.Pagina} excede el número total de página de {totalPaginas}");

            //Aplicamos el ordenamiento
            switch (parametrosConsultaEstilo.Criterio)
            {
                case "nombre":
                    losEstilos = ApplyOrder(
                        losEstilos,
                        p => p.Nombre,
                        parametrosConsultaEstilo.Orden);
                    break;
            }

            //Aplicamos la paginación
            losEstilos = losEstilos
                .Skip((parametrosConsultaEstilo.Pagina - 1) * parametrosConsultaEstilo.ElementosPorPagina)
                .Take(parametrosConsultaEstilo.ElementosPorPagina);

            var respuestaEstilos = new EstiloResponse
            {
                Tipo = "Estilo",
                TotalElementos = totalElementos,
                PaginaActual = parametrosConsultaEstilo.Pagina,
                ElementosPorPagina = parametrosConsultaEstilo.ElementosPorPagina, // PageSize
                TotalPaginas = totalPaginas,
                Data = losEstilos.ToList()
            };

            return respuestaEstilos;
        }

        public async Task<EstiloDetallado> GetByAttributeAsync<T>(T atributo_valor, string atributo_nombre)
        {
            Estilo unEstilo = new();

            switch (atributo_nombre.ToLower())
            {
                case "id":
                    unEstilo = await _estiloRepository
                        .GetByAttributeAsync<T>(atributo_valor, "id");
                    break;

                case "nombre":
                    unEstilo = await _estiloRepository
                        .GetByAttributeAsync<T>(atributo_valor, "nombre");
                    break;
            }

            if (unEstilo.Id == 0)
                throw new AppValidationException($"Estilo no encontrado con el atributo {atributo_nombre} {atributo_valor}");

            var unEstiloDetallado = await BuildDetailedStyleAsync(unEstilo);
            return unEstiloDetallado;
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int estilo_id)
        {
            //Validamos que el estilo exista con ese Id
            var unEstilo = await _estiloRepository
                .GetByAttributeAsync<int>(estilo_id, "id");

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

        public async Task<Estilo> CreateAsync(Estilo estilo)
        {
            //Validamos que el estilo tenga nombre
            if (estilo.Nombre.Length == 0)
                throw new AppValidationException("No se puede insertar un estilo con nombre nulo");

            // validamos que el estilo a crear no esté previamente creado
            var estiloExistente = await _estiloRepository
                .GetByAttributeAsync<string>(estilo.Nombre, "nombre");

            if (estiloExistente.Id != 0)
                throw new AppValidationException($"Ya existe un estilo con el nombre {estilo.Nombre}");

            try
            {
                bool resultadoAccion = await _estiloRepository
                    .CreateAsync(estilo);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                estiloExistente = await _estiloRepository
                    .GetByAttributeAsync<string>(estilo.Nombre, "nombre");
            }
            catch (DbOperationException)
            {
                throw;
            }

            return estiloExistente;
        }

        public async Task<Estilo> UpdateAsync(int estilo_id, Estilo estilo)
        {
            //Validamos que los parametros sean consistentes
            if (estilo_id != estilo.Id)
                throw new AppValidationException($"Inconsistencia en el Id del estilo a actualizar. Verifica argumentos");

            //Validamos que el estilo tenga nombre
            if (estilo.Nombre.Length == 0)
                throw new AppValidationException($"No se puede actualizar el estilo {estilo.Id} para que tenga nombre nulo");

            //Validamos que el nuevo nombre no exista previamente con otro Id
            var estiloExistente = await _estiloRepository
                .GetByAttributeAsync<string>(estilo.Nombre, "nombre");

            if (estiloExistente.Id != 0)
                throw new AppValidationException($"Ya existe un estilo con el nombre {estilo.Nombre}");

            // validamos que el estilo a actualizar si exista con ese Id
            estiloExistente = await _estiloRepository
                .GetByAttributeAsync<int>(estilo.Id, "id");

            if (estiloExistente.Id == 0)
                throw new AppValidationException($"No existe un estilo con el Id {estilo.Id} que se pueda actualizar");

            try
            {
                bool resultadoAccion = await _estiloRepository
                    .UpdateAsync(estilo);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                estiloExistente = await _estiloRepository
                    .GetByAttributeAsync<string>(estilo.Nombre, "nombre");
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
                .GetByAttributeAsync<int>(estilo_id, "id");

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

        private async Task<EstiloDetallado> BuildDetailedStyleAsync(Estilo estilo)
        {
            EstiloDetallado estiloDetallado = new()
            {
                Id = estilo.Id,
                Nombre = estilo.Nombre
            };

            var cervezasAsociadas = await _estiloRepository
                .GetAssociatedBeersAsync(estilo.Id);
            estiloDetallado.Cervezas = cervezasAsociadas.ToList();

            //Colocamos los valores del rango ABV a las cervezas
            foreach (Cerveza unaCerveza in estiloDetallado.Cervezas)
                unaCerveza.Rango_Abv = await _cervezaRepository
                    .GetAbvRangeNameAsync(unaCerveza.Abv);

            return estiloDetallado;
        }

        private static IEnumerable<Estilo> ApplyOrder<T>(IEnumerable<Estilo> estilos, Func<Estilo, T> criterio, string orden)
        {
            if (orden.ToLower().Equals("desc"))
                return estilos.OrderByDescending(criterio);
            else
                return estilos.OrderBy(criterio);
        }
    }
}
