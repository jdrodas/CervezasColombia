using CervezasColombia_CS_API_SQLite_Dapper.Exceptions;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;
using CervezasColombia_CS_API_SQLite_Dapper.Models;

namespace CervezasColombia_CS_API_SQLite_Dapper.Services
{
    public class EnvasadoService(IEnvasadoRepository envasadoRepository,
                           ICervezaRepository cervezaRepository)
    {
        private readonly IEnvasadoRepository _envasadoRepository = envasadoRepository;
        private readonly ICervezaRepository _cervezaRepository = cervezaRepository;

        public async Task<IEnumerable<Envasado>> GetAllAsync()
        {
            return await _envasadoRepository
                .GetAllAsync();
        }

        public async Task<Envasado> GetByIdAsync(int envasado_id)
        {
            //Validamos que la envasado exista con ese Id
            var unEnvasado = await _envasadoRepository
                .GetByIdAsync(envasado_id);

            if (unEnvasado.Id == 0)
                throw new AppValidationException($"Envasado no encontrado con el id {envasado_id}");

            return unEnvasado;
        }

        public async Task<IEnumerable<CervezaEnvasada>> GetAssociatedBeersAsync(int envasado_id)
        {
            //Validamos que el Envasado exista con ese Id
            var unEnvasado = await _envasadoRepository
                .GetByIdAsync(envasado_id);

            if (unEnvasado.Id == 0)
                throw new AppValidationException($"Envasado no encontrado con el id {envasado_id}");

            //Si la cerveceria existe, validamos que tenga cervezas asociadas
            var cantidadCervezasAsociadas = await _envasadoRepository
                .GetTotalAssociatedBeersAsync(envasado_id);

            if (cantidadCervezasAsociadas == 0)
                throw new AppValidationException($"No Existen cervezas asociadas al envasado {unEnvasado.Nombre}");

            var losEnvasadosCervezas = await _envasadoRepository
                .GetAssociatedPackagedBeersAsync(envasado_id);

            //Aqui completamos la información de las cervezas
            Cerveza unaCerveza;
            CervezaEnvasada unaCervezaEnvasada;
            List<CervezaEnvasada> lasCervezasEnvasadas = [];

            foreach (EnvasadoCerveza unEnvasadoCerveza in losEnvasadosCervezas)
            {
                unaCerveza = await _cervezaRepository
                    .GetByNameAndBreweryAsync(unEnvasadoCerveza.Cerveza, unEnvasadoCerveza.Cerveceria);

                unaCervezaEnvasada = new()
                {
                    Id = unaCerveza.Id,
                    Nombre = unaCerveza.Nombre,
                    Cerveceria = unEnvasadoCerveza.Cerveceria,
                    Estilo = unaCerveza.Estilo,                    
                    Abv = unaCerveza.Abv,
                    Rango_Abv = unaCerveza.Rango_Abv,
                    Unidad_Volumen = unEnvasadoCerveza.Unidad_Volumen,
                    Volumen = unEnvasadoCerveza.Volumen,
                    Envasado = unEnvasadoCerveza.Envasado
                };

                lasCervezasEnvasadas.Add(unaCervezaEnvasada);
            }

            return lasCervezasEnvasadas;
        }

        public async Task<Envasado> CreateAsync(Envasado unEnvasado)
        {
            //Validamos que el envasado tenga nombre
            if (unEnvasado.Nombre.Length == 0)
                throw new AppValidationException("No se puede insertar un envasado con nombre nulo");

            // validamos que el envasado a crear no esté previamente creado
            var envasadoExistente = await _envasadoRepository
                .GetByNameAsync(unEnvasado.Nombre!);

            if (envasadoExistente.Id != 0)
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

        public async Task<Envasado> UpdateAsync(int envasado_id, Envasado unEnvasado)
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

            if (envasadoExistente.Id != 0)
                throw new AppValidationException($"Ya existe un envasado con el nombre {unEnvasado.Nombre}");

            // validamos que el envasado a actualizar si exista con ese Id
            envasadoExistente = await _envasadoRepository
                .GetByIdAsync(unEnvasado.Id);

            if (envasadoExistente.Id == 0)
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

        public async Task DeleteAsync(int envasado_id)
        {
            // validamos que el envasado a eliminar si exista con ese Id
            var envasadoExistente = await _envasadoRepository
                .GetByIdAsync(envasado_id);

            if (envasadoExistente.Id == 0)
                throw new AppValidationException($"No existe un envasado con el Id {envasado_id} que se pueda eliminar");

            // Validamos que el envasado no tenga asociadas cervezas
            var cantidadCervezasAsociadas = await _envasadoRepository
                .GetTotalAssociatedBeersAsync(envasadoExistente.Id);

            if (cantidadCervezasAsociadas > 0)
            {
                //Borrar envasado asociado a esta cerveza
                await _envasadoRepository
                    .DeleteAssociatedBeersAsync(envasadoExistente.Id);
            }

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
