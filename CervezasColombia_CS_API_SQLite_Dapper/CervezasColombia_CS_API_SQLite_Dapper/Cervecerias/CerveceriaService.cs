using CervezasColombia_CS_API_SQLite_Dapper.Cervezas;
using CervezasColombia_CS_API_SQLite_Dapper.Ubicaciones;

namespace CervezasColombia_CS_API_SQLite_Dapper.Cervecerias
{
    public class CerveceriaService(ICerveceriaRepository cerveceriaRepository,
                            IUbicacionRepository ubicacionRepository,
                            ICervezaRepository cervezaRepository)
    {
        private readonly ICerveceriaRepository _cerveceriaRepository = cerveceriaRepository;
        private readonly IUbicacionRepository _ubicacionRepository = ubicacionRepository;
        private readonly ICervezaRepository _cervezaRepository = cervezaRepository;

        public async Task<IEnumerable<Cerveceria>> GetAllAsync()
        {
            return await _cerveceriaRepository
                .GetAllAsync();
        }

        public async Task<CerveceriaResponse> GetDetailsByIdAsync(int cerveceria_id)
        {
            //Validamos que la Cerveceria exista con ese Id
            var unaCerveceria = await _cerveceriaRepository
                .GetDetailsByIdAsync(cerveceria_id);

            if (unaCerveceria.Id == 0)
                throw new AppValidationException($"Cerveceria no encontrada con el id {cerveceria_id}");

            var unaCerveceriaDetallada = await BuildCerveceriaResponseAsync(unaCerveceria);
            return unaCerveceriaDetallada;
        }

        public async Task<CerveceriaResponse> GetByNameAsync(string cerveceria_name)
        {
            //Validamos que la Cerveceria exista con ese nombre
            var unaCerveceria = await _cerveceriaRepository
                .GetByNameAsync(cerveceria_name);

            if (unaCerveceria.Id == 0)
                throw new AppValidationException($"Cerveceria no encontrada con el nombre {cerveceria_name}");

            var unaCerveceriaDetallada = await BuildCerveceriaResponseAsync(unaCerveceria);
            return unaCerveceriaDetallada;
        }

        public async Task<CerveceriaResponse> GetByInstagramAsync(string cerveceria_Instagram)
        {
            //Validamos que la Cerveceria exista con ese nombre
            var unaCerveceria = await _cerveceriaRepository
                .GetByInstagramAsync(cerveceria_Instagram);

            if (unaCerveceria.Id == 0)
                throw new AppValidationException($"Cerveceria no encontrada con el instagram {cerveceria_Instagram}");

            var unaCerveceriaDetallada = await BuildCerveceriaResponseAsync(unaCerveceria);
            return unaCerveceriaDetallada;
        }

        public async Task<Cerveceria> CreateAsync(Cerveceria cerveceria)
        {
            ValidateBrewery(cerveceria);
            await ValidateBreweryLocation(cerveceria);
            
            //Validamos que el nombre no exista previamente
            var cerveceriaExistente = await _cerveceriaRepository
                .GetByNameAsync(cerveceria.Nombre);

            if (cerveceriaExistente.Id != 0)
                throw new AppValidationException($"Ya existe una cervecería con el nombre {cerveceria.Nombre}");

            //Validamos que el instagram no exista previamente
            cerveceriaExistente = await _cerveceriaRepository
                .GetByInstagramAsync(cerveceria.Instagram);

            if (cerveceriaExistente.Id != 0)
                throw new AppValidationException($"Ya existe una cervecería con el instagram {cerveceria.Instagram}");

            try
            {
                bool resultadoAccion = await _cerveceriaRepository
                    .CreateAsync(cerveceria);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                cerveceriaExistente = await _cerveceriaRepository
                    .GetByNameAsync(cerveceria.Nombre!);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return cerveceriaExistente;
        }

        public async Task<Cerveceria> UpdateAsync(int cerveceria_id, Cerveceria cerveceria)
        {
            //Validamos que los parametros sean consistentes
            if (cerveceria_id != cerveceria.Id)
                throw new AppValidationException($"Inconsistencia en el Id de la cervecería a actualizar. Verifica argumentos");

            ValidateBrewery(cerveceria);

            //Validamos que la Cerveceria exista con ese Id
            var cerveceriaExistente = await _cerveceriaRepository
                .GetByIdAsync(cerveceria_id);

            if (cerveceriaExistente.Id == 0)
                throw new AppValidationException($"No existe una cervecería registrada con el id {cerveceria.Id}");

            //Validamos que el nombre no exista previamente en otra cervecería diferente a la que se está actualizando
            cerveceriaExistente = await _cerveceriaRepository
                .GetByNameAsync(cerveceria.Nombre);

            if (cerveceria.Id != cerveceriaExistente.Id && cerveceriaExistente.Id != 0)
                throw new AppValidationException($"Ya existe otra cervecería con el nombre {cerveceria.Nombre}. " +
                    $"No se puede Actualizar");

            //Validamos que el instagram no exista previamente en otra cervecería diferente a la que se está actualizando
            cerveceriaExistente = await _cerveceriaRepository
                .GetByInstagramAsync(cerveceria.Instagram);

            if (cerveceria.Id != cerveceriaExistente.Id && cerveceriaExistente.Id != 0)
                throw new AppValidationException($"Ya existe otra cervecería con el instagram {cerveceria.Instagram}. " +
                    $"No se puede Actualizar");

            //Validamos que la cerveceria tenga ubicación válida
            await ValidateBreweryLocation(cerveceria);

            //Validamos que haya al menos un cambio en las propiedades
            if (cerveceria.Equals(cerveceriaExistente))
                throw new AppValidationException("No hay cambios en los atributos de la cervecería. No se realiza actualización.");

            try
            {
                bool resultadoAccion = await _cerveceriaRepository
                    .UpdateAsync(cerveceria);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                cerveceriaExistente = await _cerveceriaRepository
                    .GetByNameAsync(cerveceria.Nombre!);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return cerveceriaExistente;
        }

        public async Task DeleteAsync(int cerveceria_id)
        {
            // validamos que el cerveceria a eliminar si exista con ese Id
            var cerveceriaExistente = await _cerveceriaRepository
                .GetByIdAsync(cerveceria_id);

            if (cerveceriaExistente.Id == 0)
                throw new AppValidationException($"No existe una cerveceria con el Id {cerveceria_id} que se pueda eliminar");


            // Validamos que la cerveceria no tenga asociadas cervezas
            var cantidadCervezasAsociadas = await _cerveceriaRepository
                .GetTotalAssociatedBeersAsync(cerveceriaExistente.Id);

            //Si existen, se borran previamente
            if (cantidadCervezasAsociadas > 0)
                await _cerveceriaRepository
                    .DeleteAssociatedBeersAsync(cerveceriaExistente.Id);

            //Finalmente se borra la cerveceria
            try
            {
                bool resultadoAccion = await _cerveceriaRepository
                    .DeleteAsync(cerveceriaExistente);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");
            }
            catch (DbOperationException)
            {
                throw;
            }
        }

        private async Task<CerveceriaResponse> BuildCerveceriaResponseAsync(Cerveceria cerveceria)
        {
            //TODO: Modificar para implementar patrón decorador
            CerveceriaResponse cerveceriaResponse = new()
            {
                Id = cerveceria.Id,
                Nombre = cerveceria.Nombre,
                Instagram = cerveceria.Instagram,
                Ubicacion = cerveceria.Ubicacion
            };

            var cervezasAsociadas = await _cerveceriaRepository
                .GetAssociatedBeersAsync(cerveceria.Id);
            cerveceriaResponse.Cervezas = cervezasAsociadas.ToList();

            //Colocamos los valores del rango ABV a las cervezas
            foreach (Cerveza unaCerveza in cerveceriaResponse.Cervezas)
                unaCerveza.Rango_Abv = await _cervezaRepository
                    .GetAbvRangeNameAsync(unaCerveza.Abv);

            return cerveceriaResponse;
        }

        private void ValidateBrewery(Cerveceria cerveceria)
        {
            //Validamos que la cerveceria tenga nombre
            if (cerveceria.Nombre.Length == 0)
                throw new AppValidationException("No se puede insertar una cervecería con nombre nulo");

            //Validamos que la cerveceria tenga instagram
            if (cerveceria.Instagram.Length == 0)
                throw new AppValidationException("No se puede insertar una cervecería con Instagram nulo");

            //Validamos que la cerveceria tenga ubicación
            if (cerveceria.Ubicacion.Municipio.Length == 0 ||
                cerveceria.Ubicacion.Departamento.Length == 0)
                throw new AppValidationException("No se puede insertar una cervecería una ubicación nula");
        }

        private async Task ValidateBreweryLocation(Cerveceria cerveceria)
        {
            //Validamos que la cerveceria tenga ubicación válida
            var unaUbicacion = await _ubicacionRepository
                .GetByNameAsync(cerveceria.Ubicacion.Municipio, cerveceria.Ubicacion.Departamento);

            if (unaUbicacion.Id == 0)
                throw new AppValidationException("No se puede actualizar una cervecería sin ubicación conocida");

            cerveceria.Ubicacion = unaUbicacion;
        }
    }
}
