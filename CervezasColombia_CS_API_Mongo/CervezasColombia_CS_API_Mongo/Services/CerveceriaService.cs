using CervezasColombia_CS_API_Mongo.Helpers;
using CervezasColombia_CS_API_Mongo.Interfaces;
using CervezasColombia_CS_API_Mongo.Models;

namespace CervezasColombia_CS_API_Mongo.Services
{
    public class CerveceriaService
    {
        private readonly ICerveceriaRepository _cerveceriaRepository;
        private readonly IUbicacionRepository _ubicacionRepository;
        private readonly ICervezaRepository _cervezaRepository;

        public CerveceriaService(ICerveceriaRepository cerveceriaRepository,
                                IUbicacionRepository ubicacionRepository,
                                ICervezaRepository cervezaRepository)
        {
            _cerveceriaRepository = cerveceriaRepository;
            _ubicacionRepository = ubicacionRepository;
            _cervezaRepository = cervezaRepository;
        }

        public async Task<IEnumerable<Cerveceria>> GetAllAsync()
        {
            return await _cerveceriaRepository
                .GetAllAsync();
        }

        public async Task<CerveceriaDetallada> GetDetailsByIdAsync(string cerveceria_id)
        {
            //Validamos que la Cerveceria exista con ese Id
            var unaCerveceriaDetallada = await _cerveceriaRepository
                .GetDetailsByIdAsync(cerveceria_id);

            if (string.IsNullOrEmpty(unaCerveceriaDetallada.Id))
                throw new AppValidationException($"Cerveceria no encontrada con el id {cerveceria_id}");

            //Colocamos los valores de los rangos a las cervezas
            foreach (Cerveza unaCerveza in unaCerveceriaDetallada.Cervezas)
            {
                unaCerveza.Rango_Ibu = await _cervezaRepository.GetIbuRangeNameAsync(unaCerveza.Ibu);
                unaCerveza.Rango_Abv = await _cervezaRepository.GetAbvRangeNameAsync(unaCerveza.Abv);
            }

            return unaCerveceriaDetallada;
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(string cerveceria_id)
        {
            //Validamos que la Cerveceria exista con ese Id
            var unaCerveceria = await _cerveceriaRepository
                .GetByIdAsync(cerveceria_id);

            if (string.IsNullOrEmpty(unaCerveceria.Id))
                throw new AppValidationException($"Cerveceria no encontrada con el id {cerveceria_id}");

            //Si la cerveceria existe, validamos que tenga cervezas asociadas
            var cantidadCervezasAsociadas = await _cerveceriaRepository
                .GetTotalAssociatedBeersAsync(cerveceria_id);

            if (cantidadCervezasAsociadas == 0)
                throw new AppValidationException($"No Existen cervezas asociadas a la cerveceria {unaCerveceria.Nombre}");

            var lasCervezas = await _cerveceriaRepository.GetAssociatedBeersAsync(cerveceria_id);

            //Colocamos los valores de los rangos a las cervezas
            foreach (Cerveza unaCerveza in lasCervezas)
            {
                unaCerveza.Rango_Ibu = await _cervezaRepository.GetIbuRangeNameAsync(unaCerveza.Ibu);
                unaCerveza.Rango_Abv = await _cervezaRepository.GetAbvRangeNameAsync(unaCerveza.Abv);
            }

            return lasCervezas;
        }

        public async Task<Cerveceria> CreateAsync(Cerveceria unaCerveceria)
        {
            //Validamos que la cerveceria tenga nombre
            if (unaCerveceria.Nombre.Length == 0)
                throw new AppValidationException("No se puede insertar una cervecería con nombre nulo");

            //Validamos que la cerveceria tenga sitio_web
            if (unaCerveceria.Sitio_Web.Length == 0)
                throw new AppValidationException("No se puede insertar una cervecería con Sitio Web nulo");

            //Validamos que la cerveceria tenga instagram
            if (unaCerveceria.Instagram.Length == 0)
                throw new AppValidationException("No se puede insertar una cervecería con Instagram nulo");

            //Validamos que la cerveceria tenga ubicación
            if (unaCerveceria.Ubicacion.Length == 0)
                throw new AppValidationException("No se puede insertar una cervecería una ubicación nula");

            //Validamos que la cerveceria tenga ubicación válida
            var ubicacionExistente = await _ubicacionRepository
                .GetByNameAsync(unaCerveceria.Ubicacion);

            if (string.IsNullOrEmpty(ubicacionExistente.Id))
                throw new AppValidationException("No se puede insertar una cervecería sin ubicación conocida");

            //unaCerveceria.Ubicacion = ubicacionExistente;

            //Validamos que el nombre no exista previamente
            var cerveceriaExistente = await _cerveceriaRepository
                .GetByNameAsync(unaCerveceria.Nombre);

            if (string.IsNullOrEmpty(cerveceriaExistente.Id)==false)
                throw new AppValidationException($"Ya existe una cervecería con el nombre {unaCerveceria.Nombre}");

            //Validamos que el sitio_web no exista previamente
            cerveceriaExistente = await _cerveceriaRepository
                .GetBySitioWebAsync(unaCerveceria.Sitio_Web);

            if (string.IsNullOrEmpty(cerveceriaExistente.Id) == false)
                throw new AppValidationException($"Ya existe una cervecería con el sitio web {unaCerveceria.Sitio_Web}");

            //Validamos que el instagram no exista previamente
            cerveceriaExistente = await _cerveceriaRepository
                .GetByInstagramAsync(unaCerveceria.Instagram);

            if (string.IsNullOrEmpty(cerveceriaExistente.Id) == false)
                throw new AppValidationException($"Ya existe una cervecería con el instagram {unaCerveceria.Instagram}");

            try
            {
                bool resultadoAccion = await _cerveceriaRepository
                    .CreateAsync(unaCerveceria);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                cerveceriaExistente = await _cerveceriaRepository
                    .GetByNameAsync(unaCerveceria.Nombre!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return cerveceriaExistente;
        }

        public async Task<Cerveceria> UpdateAsync(string cerveceria_id, Cerveceria unaCerveceria)
        {
            //Validamos que los parametros sean consistentes
            if (cerveceria_id != unaCerveceria.Id)
                throw new AppValidationException($"Inconsistencia en el Id de la cervecería a actualizar. Verifica argumentos");

            //Validamos que la Cerveceria exista con ese Id
            var cerveceriaExistente = await _cerveceriaRepository
                .GetByIdAsync(cerveceria_id);

            if (string.IsNullOrEmpty(cerveceriaExistente.Id))
                throw new AppValidationException($"No existe una cervecería registrada con el id {unaCerveceria.Id}");

            //Validamos que la cerveceria tenga nombre
            if (unaCerveceria.Nombre.Length == 0)
                throw new AppValidationException("No se puede actualizar una cervecería con nombre nulo");

            //Validamos que el nombre no exista previamente en otra cervecería diferente a la que se está actualizando
            cerveceriaExistente = await _cerveceriaRepository
                .GetByNameAsync(unaCerveceria.Nombre);

            if (unaCerveceria.Id != cerveceriaExistente.Id)
                throw new AppValidationException($"Ya existe otra cervecería con el nombre {unaCerveceria.Nombre}. " +
                    $"No se puede Actualizar");

            //Validamos que la cerveceria tenga sitio_web
            if (unaCerveceria.Sitio_Web.Length == 0)
                throw new AppValidationException("No se puede actualizar una cervecería con Sitio Web nulo");

            //Validamos que el sitio_web no exista previamente en otra cervecería diferente a la que se está actualizando
            cerveceriaExistente = await _cerveceriaRepository
                .GetBySitioWebAsync(unaCerveceria.Sitio_Web);

            if (unaCerveceria.Id != cerveceriaExistente.Id)
                throw new AppValidationException($"Ya existe otra cervecería con el sitio web {unaCerveceria.Sitio_Web}. " +
                    $"No se puede Actualizar");

            //Validamos que la cerveceria tenga instagram
            if (unaCerveceria.Instagram.Length == 0)
                throw new AppValidationException("No se puede actualizar una cervecería con Instagram nulo");

            //Validamos que el instagram no exista previamente en otra cervecería diferente a la que se está actualizando
            cerveceriaExistente = await _cerveceriaRepository
                .GetByInstagramAsync(unaCerveceria.Instagram);

            if (unaCerveceria.Id != cerveceriaExistente.Id)
                throw new AppValidationException($"Ya existe otra cervecería con el instagram {unaCerveceria.Instagram}. " +
                    $"No se puede Actualizar");

            //Validamos que la cerveceria tenga ubicación
            if (unaCerveceria.Ubicacion.Length == 0)
                throw new AppValidationException("No se puede actualizar una cervecería con ubicación nula");

            //Validamos que la cerveceria tenga ubicación válida
            var ubicacionExistente = await _ubicacionRepository
                            .GetByNameAsync(unaCerveceria.Ubicacion);


            if (string.IsNullOrEmpty(ubicacionExistente.Id))
                throw new AppValidationException("No se puede actualizar una cervecería sin ubicación conocida");

            unaCerveceria.Ubicacion = ubicacionExistente.Municipio + ", " + ubicacionExistente.Departamento;

            //Validamos que haya al menos un cambio en las propiedades
            if (unaCerveceria.Equals(cerveceriaExistente))
                throw new AppValidationException("No hay cambios en los atributos de la cervecería. No se realiza actualización.");

            try
            {
                bool resultadoAccion = await _cerveceriaRepository
                    .UpdateAsync(unaCerveceria);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                cerveceriaExistente = await _cerveceriaRepository
                    .GetByNameAsync(unaCerveceria.Nombre!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return cerveceriaExistente;
        }

        public async Task DeleteAsync(string cerveceria_id)
        {
            // validamos que el cerveceria a eliminar si exista con ese Id
            var cerveceriaExistente = await _cerveceriaRepository
                .GetByIdAsync(cerveceria_id);

            if (string.IsNullOrEmpty(cerveceriaExistente.Id))
                throw new AppValidationException($"No existe una cerveceria con el Id {cerveceria_id} que se pueda eliminar");


            // Validamos que la cerveceria no tenga asociadas cervezas
            var cantidadCervezasAsociadas = await _cerveceriaRepository
                .GetTotalAssociatedBeersAsync(cerveceriaExistente.Id);

            //Si existen, se borran previamente
            if (cantidadCervezasAsociadas > 0)
                await _cerveceriaRepository.DeleteAssociatedBeersAsync(cerveceriaExistente.Id);

            //Finalmente se borra la cerveceria
            try
            {
                bool resultadoAccion = await _cerveceriaRepository
                    .DeleteAsync(cerveceriaExistente);

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
