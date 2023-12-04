using CervezasColombia_CS_API_SQLite_Dapper.Cervezas;

namespace CervezasColombia_CS_API_SQLite_Dapper.Ingredientes
{
    public class IngredienteService(IIngredienteRepository ingredienteRepository,
        ICervezaRepository cervezaRepository)
    {
        private readonly IIngredienteRepository _ingredienteRepository = ingredienteRepository;
        private readonly ICervezaRepository _cervezaRepository = cervezaRepository;

        public async Task<IEnumerable<Ingrediente>> GetAllAsync()
        {
            return await _ingredienteRepository
                .GetAllAsync();
        }

        public async Task<Ingrediente> GetByIdAsync(int ingrediente_id)
        {
            //Validamos que el ingrediente exista con ese Id
            var unIngrediente = await _ingredienteRepository
                .GetByIdAsync(ingrediente_id);

            if (string.IsNullOrEmpty(unIngrediente.Id))
                throw new AppValidationException($"Ingrediente no encontrado con el id {ingrediente_id}");

            return unIngrediente;
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int ingrediente_id)
        {
            //Validamos que el ingrediente exista con ese Id
            var unIngrediente = await _ingredienteRepository
                .GetByIdAsync(ingrediente_id);

            if (string.IsNullOrEmpty(unIngrediente.Id))
                throw new AppValidationException($"Ingrediente no encontrado con el id {ingrediente_id}");

            //Si la ingrediente existe, validamos que tenga cervezas asociadas
            var cantidadCervezasAsociadas = await _ingredienteRepository
                .GetTotalAssociatedBeersAsync(ingrediente_id);

            if (cantidadCervezasAsociadas == 0)
                throw new AppValidationException($"No Existen cervezas asociadas al ingrediente {unIngrediente.Nombre}");

            var losIngredientesCervezas = await _ingredienteRepository
                .GetAssociatedBeersAsync(ingrediente_id);

            //Aqui completamos la información de las cervezas
            Cerveza unaCerveza;
            List<Cerveza> lasCervezas = [];

            foreach (IngredienteCerveza unIngredienteCerveza in losIngredientesCervezas)
            {
                unaCerveza = await _cervezaRepository
                    .GetByNameAndBreweryAsync(unIngredienteCerveza.Cerveza, unIngredienteCerveza.Cerveceria);
                lasCervezas.Add(unaCerveza);
            }

            return lasCervezas;
        }

        public async Task<Ingrediente> CreateAsync(Ingrediente unIngrediente)
        {
            //Validamos que el ingrediente tenga nombre
            if (unIngrediente.Nombre.Length == 0)
                throw new AppValidationException("No se puede insertar un ingrediente con nombre nulo");

            //Validamos que el ingrediente tenga tipo
            if (unIngrediente.Tipo_Ingrediente.Length == 0)
                throw new AppValidationException("No se puede insertar un ingrediente con tipo nulo");

            // validamos que el ingrediente a crear no esté previamente creado
            var ingredienteExistente = await _ingredienteRepository
                .GetByNameAndTypeAsync(unIngrediente.Nombre!, unIngrediente.Tipo_Ingrediente!);

            if (string.IsNullOrEmpty(ingredienteExistente.Id) == false)
                throw new AppValidationException($"Ya existe un ingrediente con el nombre {unIngrediente.Nombre}");

            // Validamos que el tipo de ingrediente exista
            var idTipoIngredienteExistente = await _ingredienteRepository
                .GetAssociatedIngredientTypeIdAsync(unIngrediente.Tipo_Ingrediente);

            if (string.IsNullOrEmpty(idTipoIngredienteExistente))
                throw new AppValidationException($"No existe un tipo de ingrediente con el nombre {unIngrediente.Tipo_Ingrediente}");

            unIngrediente.Tipo_Ingrediente_Id = idTipoIngredienteExistente;

            try
            {
                bool resultadoAccion = await _ingredienteRepository
                    .CreateAsync(unIngrediente);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                ingredienteExistente = await _ingredienteRepository
                    .GetByNameAndTypeAsync(unIngrediente.Nombre!, unIngrediente.Tipo_Ingrediente!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return ingredienteExistente;
        }

        public async Task<Ingrediente> UpdateAsync(int ingrediente_id, Ingrediente unIngrediente)
        {
            //Validamos que los parametros sean consistentes
            if (ingrediente_id != unIngrediente.Id)
                throw new AppValidationException($"Inconsistencia en el Id del ingrediente a actualizar. Verifica argumentos");

            //Validamos que el ingrediente tenga nombre
            if (unIngrediente.Nombre.Length == 0)
                throw new AppValidationException($"No se puede actualizar el ingrediente {unIngrediente.Id} para que tenga nombre nulo");

            //Validamos que el ingrediente tenga tipo
            if (unIngrediente.Tipo_Ingrediente.Length == 0)
                throw new AppValidationException($"No se puede actualizar el ingrediente {unIngrediente.Id} para que tenga tipo nulo");

            //Validamos que el nuevo nombre no exista previamente con otro Id
            var ingredienteExistente = await _ingredienteRepository
                .GetByNameAndTypeAsync(unIngrediente.Nombre!, unIngrediente.Tipo_Ingrediente!);

            if (string.IsNullOrEmpty(ingredienteExistente.Id) == false)
                throw new AppValidationException($"Ya existe un ingrediente con el nombre {unIngrediente.Nombre} y el tipo {unIngrediente.Tipo_Ingrediente}");

            // validamos que el ingrediente a actualizar si exista con ese Id
            ingredienteExistente = await _ingredienteRepository
                .GetByIdAsync(unIngrediente.Id);

            if (string.IsNullOrEmpty(ingredienteExistente.Id))
                throw new AppValidationException($"No existe un ingrediente con el Id {unIngrediente.Id} que se pueda actualizar");

            // Validamos que el tipo de ingrediente exista
            var idTipoIngredienteExistente = await _ingredienteRepository
                .GetAssociatedIngredientTypeIdAsync(unIngrediente.Tipo_Ingrediente);

            if (string.IsNullOrEmpty(idTipoIngredienteExistente))
                throw new AppValidationException($"No existe un tipo de ingrediente con el nombre {unIngrediente.Tipo_Ingrediente}");

            unIngrediente.Tipo_Ingrediente_Id = idTipoIngredienteExistente;

            //Validamos que haya al menos un cambio en las propiedades
            if (unIngrediente.Equals(ingredienteExistente))
                throw new AppValidationException("No hay cambios en los atributos del ingrediente. No se realiza actualización.");

            try
            {
                bool resultadoAccion = await _ingredienteRepository
                    .UpdateAsync(unIngrediente);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                ingredienteExistente = await _ingredienteRepository
                    .GetByNameAndTypeAsync(unIngrediente.Nombre!, unIngrediente.Tipo_Ingrediente!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return ingredienteExistente;
        }

        public async Task DeleteAsync(int ingrediente_id)
        {
            // validamos que el ingrediente a eliminar si exista con ese Id
            var ingredienteExistente = await _ingredienteRepository
                .GetByIdAsync(ingrediente_id);

            if (string.IsNullOrEmpty(ingredienteExistente.Id))
                throw new AppValidationException($"No existe un ingrediente con el Id {ingrediente_id} que se pueda eliminar");

            // Validamos que el ingrediente no tenga asociadas cervezas
            var cantidadCervezasAsociadas = await _ingredienteRepository
                .GetTotalAssociatedBeersAsync(ingredienteExistente.Id);

            if (cantidadCervezasAsociadas > 0)
                throw new AppValidationException($"Existen {cantidadCervezasAsociadas} cervezas " +
                    $"asociadas al ingrediente {ingredienteExistente.Nombre}. No se puede eliminar");

            //Si existe y no tiene cervezas asociadas, se puede eliminar
            try
            {
                bool resultadoAccion = await _ingredienteRepository
                    .DeleteAsync(ingredienteExistente);

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
