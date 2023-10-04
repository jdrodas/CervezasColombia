using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;
using CervezasColombia_CS_API_SQLite_Dapper.Models;

namespace CervezasColombia_CS_API_SQLite_Dapper.Services
{
    public class UbicacionService
    {
        private readonly IUbicacionRepository _ubicacionRepository;

        public UbicacionService(IUbicacionRepository ubicacionRepository)
        {
            _ubicacionRepository = ubicacionRepository;
        }

        public async Task<IEnumerable<Ubicacion>> GetAllAsync()
        {
            return await _ubicacionRepository
                .GetAllAsync();
        }

        public async Task<Ubicacion> GetByIdAsync(int ubicacion_id)
        {
            //Validamos que la ubicación exista con ese Id
            var unaUbicacion = await _ubicacionRepository
                .GetByIdAsync(ubicacion_id);

            if (unaUbicacion.Id == 0)
                throw new AppValidationException($"Ubicación no encontrada con el id {ubicacion_id}");

            return unaUbicacion;
        }

        public async Task<IEnumerable<Cerveceria>> GetAssociatedBreweriesAsync(int ubicacion_id)
        {
            //Validamos que la ubicacion exista con ese Id
            var unaUbicacion = await _ubicacionRepository
                .GetByIdAsync(ubicacion_id);

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
            if (unaUbicacion.Latitud == 0 || unaUbicacion.Latitud < -90 || unaUbicacion.Latitud > 90)
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
            catch (DbOperationException error)
            {
                throw error;
            }

            return ubicacionExistente;
        }

        public async Task<Ubicacion> UpdateAsync(int ubicacion_id, Ubicacion unaUbicacion)
        {
            //Validamos que los parametros sean consistentes
            if (ubicacion_id != unaUbicacion.Id)
                throw new AppValidationException($"Inconsistencia en el Id de la ubicación a actualizar. Verifica argumentos");

            //Validamos que la ubicación tenga municipio
            if (unaUbicacion.Municipio.Length == 0)
                throw new AppValidationException("No se puede actualizar una ubicación con Municipio nulo");

            //Validamos que la ubicación tenga departamento
            if (unaUbicacion.Departamento.Length == 0)
                throw new AppValidationException("No se puede actualizar una ubicación con Departamento nulo");

            //Validamos que la ubicación tenga latitud en su coordenada geográfica y que esta sea válida
            if (unaUbicacion.Latitud == 0 || unaUbicacion.Latitud < -90 || unaUbicacion.Latitud > 90)
                throw new AppValidationException($"No se puede actualizar una ubicación en Colombia con valor de latitud en {unaUbicacion.Latitud} para su coordenada geográfica");

            //Validamos que la ubicación tenga longitud en su coordenada geográfica y que esta sea válida
            if (unaUbicacion.Longitud == 0 || unaUbicacion.Longitud < -180 || unaUbicacion.Longitud > 180)
                throw new AppValidationException($"No se puede actualizar una ubicación en Colombia con valor de longitud en {unaUbicacion.Longitud} para su coordenada geográfica");

            //Validamos que el nuevo municipio,departamento no exista previamente con otro Id
            var ubicacionExistente = await _ubicacionRepository
                .GetByNameAsync(unaUbicacion.Municipio!, unaUbicacion.Departamento!);

            if (unaUbicacion.Equals(ubicacionExistente))
                return ubicacionExistente;

            // validamos que la ubicación a actualizar si exista con ese Id
            ubicacionExistente = await _ubicacionRepository
                .GetByIdAsync(unaUbicacion.Id);

            if (ubicacionExistente.Id == 0)
                throw new AppValidationException($"No existe una ubicación con el Id {unaUbicacion.Id} que se pueda actualizar");

            try
            {
                bool resultadoAccion = await _ubicacionRepository
                    .UpdateAsync(unaUbicacion);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                ubicacionExistente = await _ubicacionRepository
                    .GetByNameAsync(unaUbicacion.Municipio!, unaUbicacion.Departamento!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return ubicacionExistente;
        }

        public async Task DeleteAsync(int id)
        {
            // validamos que la ubicación a eliminar si exista con ese Id
            var ubicacionExistente = await _ubicacionRepository
                .GetByIdAsync(id);

            if (ubicacionExistente.Id == 0)
                throw new AppValidationException($"No existe una ubicación con el Id {id} que se pueda eliminar");

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
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");
            }
            catch (DbOperationException error)
            {
                throw error;
            }
        }
    }
}