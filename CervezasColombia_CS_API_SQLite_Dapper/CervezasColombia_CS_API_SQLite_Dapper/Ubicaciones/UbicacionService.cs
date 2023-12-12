using CervezasColombia_CS_API_SQLite_Dapper.Cervecerias;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;

namespace CervezasColombia_CS_API_SQLite_Dapper.Ubicaciones
{
    public class UbicacionService(IUbicacionRepository ubicacionRepository)
    {
        private readonly IUbicacionRepository _ubicacionRepository = ubicacionRepository;

        public async Task<UbicacionResponse> GetAllAsync(UbicacionQueryParameters parametrosConsultaUbicacion)
        {
            var lasUbicaciones = await _ubicacionRepository
                .GetAllAsync();

            // Calculamos items totales y cantidad de páginas
            var totalElementos = lasUbicaciones.Count();
            var totalPaginas = (int)Math.Ceiling((double)totalElementos / parametrosConsultaUbicacion.ElementosPorPagina);

            //Validamos que la página solicitada está dentro del rango permitido
            if (parametrosConsultaUbicacion.Pagina > totalPaginas && totalPaginas > 0)
                throw new AppValidationException($"La página solicitada No. {parametrosConsultaUbicacion.Pagina} excede el número total de página de {totalPaginas}");

            //Aplicamos el ordenamiento
            switch (parametrosConsultaUbicacion.Criterio)
            {
                case "municipio":
                    lasUbicaciones = ApplyOrder(
                        lasUbicaciones,
                        p => p.Municipio,
                        parametrosConsultaUbicacion.Orden);
                    break;

                case "departamento":
                    lasUbicaciones = ApplyOrder(
                        lasUbicaciones,
                        p => p.Departamento,
                        parametrosConsultaUbicacion.Orden);
                    break;
            }

            //Aplicamos la paginación
            lasUbicaciones = lasUbicaciones
                .Skip((parametrosConsultaUbicacion.Pagina - 1) * parametrosConsultaUbicacion.ElementosPorPagina)
                .Take(parametrosConsultaUbicacion.ElementosPorPagina);

            var respuestaUbicaciones = new UbicacionResponse
            {
                Tipo = "Ubicacion",
                TotalElementos = totalElementos,
                PaginaActual = parametrosConsultaUbicacion.Pagina,
                ElementosPorPagina = parametrosConsultaUbicacion.ElementosPorPagina, // PageSize
                TotalPaginas = totalPaginas,
                Data = lasUbicaciones.ToList()
            };

            return respuestaUbicaciones;
        }

        public async Task<UbicacionDetallada> GetByAttributeAsync<T>(T atributo_valor, string atributo_nombre)
        {
            Ubicacion unaUbicacion = new();

            switch (atributo_nombre.ToLower())
            {
                case "id":
                    unaUbicacion = await _ubicacionRepository
                        .GetByAttributeAsync<T>(atributo_valor, "id");
                    break;

                case "nombre":
                    unaUbicacion = await _ubicacionRepository
                        .GetByAttributeAsync<T>(atributo_valor, "nombre");
                    break;
            }

            if (unaUbicacion.Id == 0)
                throw new AppValidationException($"Cerveceria no encontrada con el atributo {atributo_nombre} {atributo_valor}");

            var unaUbicacionDetallada = await BuildDetailedLocationAsync(unaUbicacion);
            return unaUbicacionDetallada;
        }

        public async Task<IEnumerable<Cerveceria>> GetAssociatedBreweriesAsync(int ubicacion_id)
        {
            //Validamos que la ubicacion exista con ese Id
            var unaUbicacion = await _ubicacionRepository
                .GetByAttributeAsync<int>(ubicacion_id, "id");

            if (unaUbicacion.Id == 0)
                throw new AppValidationException($"Ubicación no encontrada con el id {ubicacion_id}");

            //Si la ubicacion existe, validamos que tenga cervecerias asociadas
            var cantidadCerveceriasAsociadas = await _ubicacionRepository
                .GetTotalAssociatedBreweriesAsync(ubicacion_id);

            if (cantidadCerveceriasAsociadas == 0)
                throw new AppValidationException($"No Existen cervecerias asociadas a la ubicación {unaUbicacion.Municipio}, {unaUbicacion.Departamento}");

            return await _ubicacionRepository
                .GetAssociatedBreweriesAsync(ubicacion_id);
        }

        public async Task<Ubicacion> CreateAsync(Ubicacion unaUbicacion)
        {
            //Validamos que la ubicación tenga municipio
            if (unaUbicacion.Municipio.Length == 0)
                throw new AppValidationException("No se puede insertar una ubicación con Municipio nulo");

            //Validamos que la ubicación tenga departamento
            if (unaUbicacion.Departamento.Length == 0)
                throw new AppValidationException("No se puede insertar una ubicación con Departamento nulo");

            //Validamos que la ubicación tenga latitud en su coordenada geográfica y que esta sea válida
            if (unaUbicacion.Latitud < -90 || unaUbicacion.Latitud > 90)
                throw new AppValidationException($"No se puede insertar una ubicación en Colombia con valor de latitud en {unaUbicacion.Latitud} para su coordenada geográfica");

            //Validamos que la ubicación tenga longitud en su coordenada geográfica y que esta sea válida
            if (unaUbicacion.Longitud == 0 || unaUbicacion.Longitud < -180 || unaUbicacion.Longitud > 180)
                throw new AppValidationException($"No se puede insertar una ubicación en Colombia con valor de longitud en {unaUbicacion.Longitud} para su coordenada geográfica");

            // validamos que la ubicación a crear no esté previamente creada
            var ubicacionExistente = await _ubicacionRepository
                .GetByNameAsync(unaUbicacion.Municipio!, unaUbicacion.Departamento!);

            if (ubicacionExistente.Id != 0)
                return ubicacionExistente;

            try
            {
                bool resultadoAccion = await _ubicacionRepository
                    .CreateAsync(unaUbicacion);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                ubicacionExistente = await _ubicacionRepository
                    .GetByNameAsync(unaUbicacion.Municipio!, unaUbicacion.Departamento!);
            }
            catch (AppValidationException)
            {
                throw;
            }

            return ubicacionExistente;
        }

        public async Task<Ubicacion> UpdateAsync(int ubicacion_id, Ubicacion unaUbicacion)
        {
            //Validamos que los parametros sean consistentes
            if (ubicacion_id != unaUbicacion.Id)
                throw new AppValidationException($"Inconsistencia en el Id de la ubicación a actualizar. Verifica argumentos");

            //Validamos que exista una ubicación para actualizar con ese Id
            var ubicacionExistente = await _ubicacionRepository
                .GetByAttributeAsync<int>(unaUbicacion.Id, "id");

            if (ubicacionExistente.Id == 0)
                throw new AppValidationException($"No existe una ubicación con el Id {unaUbicacion.Id} que se pueda actualizar");

            //Validamos que la ubicación tenga municipio
            if (unaUbicacion.Municipio.Length == 0)
                throw new AppValidationException("No se puede actualizar una ubicación con Municipio nulo");

            //Validamos que la ubicación tenga departamento
            if (unaUbicacion.Departamento.Length == 0)
                throw new AppValidationException("No se puede actualizar una ubicación con Departamento nulo");

            //Validamos que la ubicación tenga latitud en su coordenada geográfica y que esta sea válida
            if (unaUbicacion.Latitud < -90 || unaUbicacion.Latitud > 90)
                throw new AppValidationException($"No se puede actualizar una ubicación en Colombia con valor de latitud en {unaUbicacion.Latitud} para su coordenada geográfica");

            //Validamos que la ubicación tenga longitud en su coordenada geográfica y que esta sea válida
            if (unaUbicacion.Longitud == 0 || unaUbicacion.Longitud < -180 || unaUbicacion.Longitud > 180)
                throw new AppValidationException($"No se puede actualizar una ubicación en Colombia con valor de longitud en {unaUbicacion.Longitud} para su coordenada geográfica");

            //Validamos que el nuevo municipio,departamento no exista previamente con otro Id
            ubicacionExistente = await _ubicacionRepository
                .GetByNameAsync(unaUbicacion.Municipio!, unaUbicacion.Departamento!);

            if (unaUbicacion.Equals(ubicacionExistente))
                return ubicacionExistente;

            //Pasadas las validaciones, ejecutamos la actualización
            try
            {
                bool resultadoAccion = await _ubicacionRepository
                    .UpdateAsync(unaUbicacion);

                if (!resultadoAccion)
                    throw new DbOperationException("Operación ejecutada pero no generó cambios en la DB");

                ubicacionExistente = await _ubicacionRepository
                    .GetByNameAsync(unaUbicacion.Municipio!, unaUbicacion.Departamento!);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return ubicacionExistente;
        }

        public async Task DeleteAsync(int ubicacion_id)
        {
            // validamos que la ubicación a eliminar si exista con ese Id
            var ubicacionExistente = await _ubicacionRepository
                .GetByAttributeAsync<int>(ubicacion_id, "id");

            if (ubicacionExistente.Id == 0)
                throw new AppValidationException($"No existe una ubicación con el Id {ubicacion_id} que se pueda eliminar");

            // Validamos que la ubicación no tenga asociadas cervecerias
            var cantidadCerveceriasAsociadas = await _ubicacionRepository
                .GetTotalAssociatedBreweriesAsync(ubicacionExistente.Id);

            if (cantidadCerveceriasAsociadas > 0)
                throw new AppValidationException($"Existen {cantidadCerveceriasAsociadas} cervecerias " +
                    $"asociadas a la ubicación {ubicacionExistente.Municipio}, {ubicacionExistente.Departamento}. No se puede eliminar");

            //Si existe y no tiene cervecerias asociadas, se puede eliminar
            try
            {
                bool resultadoAccion = await _ubicacionRepository
                    .DeleteAsync(ubicacionExistente);

                if (!resultadoAccion)
                    throw new DbOperationException("Operación ejecutada pero no generó cambios en la DB");
            }
            catch (DbOperationException)
            {
                throw;
            }
        }

        private async Task<UbicacionDetallada> BuildDetailedLocationAsync(Ubicacion ubicacion)
        {
            UbicacionDetallada ubicacionDetallada = new()
            {
                Id = ubicacion.Id,
                Municipio = ubicacion.Municipio,
                Departamento = ubicacion.Departamento,
                Latitud = ubicacion.Latitud,
                Longitud = ubicacion.Longitud
            };

            var cerveceriasAsociadas = await _ubicacionRepository
                .GetAssociatedBreweriesAsync(ubicacion.Id);
            ubicacionDetallada.Cervecerias = cerveceriasAsociadas.ToList();

            return ubicacionDetallada;
        }

        private static IEnumerable<Ubicacion> ApplyOrder<T>(IEnumerable<Ubicacion> cervecerias, Func<Ubicacion, T> criterio, string orden)
        {
            if (orden.ToLower().Equals("desc"))
                return cervecerias.OrderByDescending(criterio);
            else
                return cervecerias.OrderBy(criterio);
        }
    }
}
