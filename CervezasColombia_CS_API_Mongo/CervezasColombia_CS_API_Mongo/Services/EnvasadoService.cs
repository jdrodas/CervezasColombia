using CervezasColombia_CS_API_Mongo.Helpers;
using CervezasColombia_CS_API_Mongo.Interfaces;
using CervezasColombia_CS_API_Mongo.Models;

namespace CervezasColombia_CS_API_Mongo.Services
{
    public class EnvasadoService
    {
        private readonly IEnvasadoRepository _envasadoRepository;

        public EnvasadoService(IEnvasadoRepository envasadoRepository)
        {
            _envasadoRepository = envasadoRepository;
        }

        public async Task<IEnumerable<Envasado>> GetAllAsync()
        {
            return await _envasadoRepository
                .GetAllAsync();
        }

        public async Task<Envasado> GetByIdAsync(string envasado_id)
        {
            //Validamos que la envasado exista con ese Id
            var unEnvasado = await _envasadoRepository
                .GetByIdAsync(envasado_id);

            if (string.IsNullOrEmpty(unEnvasado.Id))
                throw new AppValidationException($"Envasado no encontrado con el id {envasado_id}");

            return unEnvasado;
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(string envasado_id)
        {
            //Validamos que el Envasado exista con ese Id
            var unEnvasado = await _envasadoRepository
                .GetByIdAsync(envasado_id);

            if (string.IsNullOrEmpty(unEnvasado.Id))
                throw new AppValidationException($"Envasado no encontrado con el id {envasado_id}");

            //Si la cerveceria existe, validamos que tenga cervezas asociadas
            var cantidadCervezasAsociadas = await _envasadoRepository
                .GetTotalAssociatedBeersAsync(envasado_id);

            if (cantidadCervezasAsociadas == 0)
                throw new AppValidationException($"No Existen cervezas asociadas al envasado {unEnvasado.Nombre}");

            return await _envasadoRepository
                .GetAssociatedBeersAsync(envasado_id);
        }

        public async Task<Envasado> CreateAsync(Envasado unEnvasado)
        {
            //Validamos que el envasado tenga nombre
            if (unEnvasado.Nombre.Length == 0)
                throw new AppValidationException("No se puede insertar un envasado con nombre nulo");

            // validamos que el envasado a crear no esté previamente creado
            var envasadoExistente = await _envasadoRepository
                .GetByNameAsync(unEnvasado.Nombre!);

            if (!string.IsNullOrEmpty(envasadoExistente.Id))
                throw new AppValidationException($"Ya existe un envasado con el nombre {unEnvasado.Nombre}");

            try
            {
                bool resultadoAccion = await _envasadoRepository
                    .CreateAsync(unEnvasado);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                envasadoExistente = await _envasadoRepository
                    .GetByNameAsync(unEnvasado.Nombre!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return envasadoExistente;
        }

        public async Task<Envasado> UpdateAsync(string envasado_id, Envasado unEnvasado)
        {
            //Validamos que los parametros sean consistentes
            if (envasado_id != unEnvasado.Id)
                throw new AppValidationException($"Inconsistencia en el Id del envasado a actualizar. Verifica argumentos");

            //Validamos que el envasado tenga nombre
            if (unEnvasado.Nombre.Length == 0)
                throw new AppValidationException($"No se puede actualizar el envasado {unEnvasado.Id} para que tenga nombre nulo");

            //Validamos que el nuevo nombre no exista previamente con otro Id
            var envasadoExistente = await _envasadoRepository
                .GetByNameAsync(unEnvasado.Nombre!);

            if (!string.IsNullOrEmpty(envasadoExistente.Id))
                throw new AppValidationException($"Ya existe un envasado con el nombre {unEnvasado.Nombre}");

            // validamos que el envasado a actualizar si exista con ese Id
            envasadoExistente = await _envasadoRepository
                .GetByIdAsync(unEnvasado.Id);

            if (string.IsNullOrEmpty(envasadoExistente.Id))
                throw new AppValidationException($"No existe un envasado con el Id {unEnvasado.Id} que se pueda actualizar");

            try
            {
                bool resultadoAccion = await _envasadoRepository
                    .UpdateAsync(unEnvasado);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                envasadoExistente = await _envasadoRepository
                    .GetByNameAsync(unEnvasado.Nombre!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return envasadoExistente;
        }

        public async Task DeleteAsync(string envasado_id)
        {
            // validamos que el envasado a eliminar si exista con ese Id
            var envasadoExistente = await _envasadoRepository
                .GetByIdAsync(envasado_id);

            if (string.IsNullOrEmpty(envasadoExistente.Id))
                throw new AppValidationException($"No existe un envasado con el Id {envasado_id} que se pueda eliminar");

            // Validamos que el envasado no tenga asociadas cervezas
            // TODO: Validar como se puede borrar el envasado de una cerveza
            //var cantidadCervezasAsociadas = await _envasadoRepository
            //    .GetTotalAssociatedBeersAsync(envasadoExistente.Id);

            //if (cantidadCervezasAsociadas > 0)
            //    throw new AppValidationException($"Existen {cantidadCervezasAsociadas} cervezas " +
            //        $"asociadas al envasado {envasadoExistente.Nombre}. No se puede eliminar");

            //Si existe y no tiene cervezas asociadas, se puede eliminar
            try
            {
                bool resultadoAccion = await _envasadoRepository
                    .DeleteAsync(envasadoExistente);

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
