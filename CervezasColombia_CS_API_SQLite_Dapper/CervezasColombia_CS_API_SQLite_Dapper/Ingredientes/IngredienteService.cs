using CervezasColombia_CS_API_SQLite_Dapper.Cervezas;
using CervezasColombia_CS_API_SQLite_Dapper.Estilos;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;

namespace CervezasColombia_CS_API_SQLite_Dapper.Ingredientes
{
    public class IngredienteService(IIngredienteRepository ingredienteRepository,
        ICervezaRepository cervezaRepository)
    {
        private readonly IIngredienteRepository _ingredienteRepository = ingredienteRepository;
        private readonly ICervezaRepository _cervezaRepository = cervezaRepository;

        public async Task<IngredienteResponse> GetAllAsync(IngredienteQueryParameters parametrosConsultaIngrediente)
        {
            var losIngredientes = await _ingredienteRepository
                .GetAllAsync();

            // Calculamos items totales y cantidad de páginas
            var totalElementos = losIngredientes.Count();
            var totalPaginas = (int)Math.Ceiling((double)totalElementos / parametrosConsultaIngrediente.ElementosPorPagina);

            //Validamos que la página solicitada está dentro del rango permitido
            if (parametrosConsultaIngrediente.Pagina > totalPaginas && totalPaginas > 0)
                throw new AppValidationException($"La página solicitada No. {parametrosConsultaIngrediente.Pagina} excede el número total de página de {totalPaginas}");

            //Aplicamos el ordenamiento
            switch (parametrosConsultaIngrediente.Criterio)
            {
                case "nombre":
                    losIngredientes = ApplyOrder(
                        losIngredientes,
                        p => p.Nombre,
                        parametrosConsultaIngrediente.Orden);
                    break;

                case "tipo_ingrediente":
                    losIngredientes = ApplyOrder(
                        losIngredientes,
                        p => p.Tipo_Ingrediente,
                        parametrosConsultaIngrediente.Orden);
                    break;
            }

            //Aplicamos la paginación
            losIngredientes = losIngredientes
                .Skip((parametrosConsultaIngrediente.Pagina - 1) * parametrosConsultaIngrediente.ElementosPorPagina)
                .Take(parametrosConsultaIngrediente.ElementosPorPagina);

            var respuestaIngredientes = new IngredienteResponse
            {
                Tipo = "Ingrediente",
                TotalElementos = totalElementos,
                PaginaActual = parametrosConsultaIngrediente.Pagina,
                ElementosPorPagina = parametrosConsultaIngrediente.ElementosPorPagina, // PageSize
                TotalPaginas = totalPaginas,
                Data = losIngredientes.ToList()
            };

            return respuestaIngredientes;
        }

        public async Task<IngredienteDetallado> GetByAttributeAsync<T>(T atributo_valor, string atributo_nombre)
        {
            Ingrediente unIngrediente = new();

            switch (atributo_nombre.ToLower())
            {
                case "id":
                    unIngrediente = await _ingredienteRepository
                        .GetByAttributeAsync<T>(atributo_valor, "id");
                    break;

                case "nombre":
                    unIngrediente = await _ingredienteRepository
                        .GetByAttributeAsync<T>(atributo_valor, "nombre");
                    break;
            }

            if (unIngrediente.Id == 0)
                throw new AppValidationException($"Estilo no encontrado con el atributo {atributo_nombre} {atributo_valor}");

            var unIngredienteDetallado = await BuildDetailedIngredientAsync(unIngrediente);
            return unIngredienteDetallado;
        }


        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int ingrediente_id)
        {
            //Validamos que el ingrediente exista con ese Id
            var unIngrediente = await _ingredienteRepository
                .GetByAttributeAsync<int>(ingrediente_id, "id");

            if (unIngrediente.Id == 0)
                throw new AppValidationException($"Ingrediente no encontrado con el id {ingrediente_id}");

            //Si la ingrediente existe, validamos que tenga cervezas asociadas
            var cantidadCervezasAsociadas = await _ingredienteRepository
                .GetTotalAssociatedBeersAsync(ingrediente_id);

            if (cantidadCervezasAsociadas == 0)
                throw new AppValidationException($"No Existen cervezas asociadas al ingrediente {unIngrediente.Nombre}");

            var lasCervezas = await _ingredienteRepository
                .GetAssociatedBeersAsync(ingrediente_id);

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

            if (ingredienteExistente.Id != 0)
                throw new AppValidationException($"Ya existe un ingrediente con el nombre {unIngrediente.Nombre}");

            // Validamos que el tipo de ingrediente exista
            var idTipoIngredienteExistente = await _ingredienteRepository
                .GetAssociatedIngredientTypeIdAsync(unIngrediente.Tipo_Ingrediente);

            if (idTipoIngredienteExistente ==0)
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
            catch (DbOperationException)
            {
                throw;
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

            if (ingredienteExistente.Id!=0)
                throw new AppValidationException($"Ya existe un ingrediente con el nombre {unIngrediente.Nombre} y el tipo {unIngrediente.Tipo_Ingrediente}");

            // validamos que el ingrediente a actualizar si exista con ese Id
            ingredienteExistente = await _ingredienteRepository
                .GetByAttributeAsync<int>(unIngrediente.Id, "id");


            if (ingredienteExistente.Id==0)
                throw new AppValidationException($"No existe un ingrediente con el Id {unIngrediente.Id} que se pueda actualizar");

            // Validamos que el tipo de ingrediente exista
            var idTipoIngredienteExistente = await _ingredienteRepository
                .GetAssociatedIngredientTypeIdAsync(unIngrediente.Tipo_Ingrediente);

            if (idTipoIngredienteExistente==0)
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
            catch (DbOperationException)
            {
                throw;
            }

            return ingredienteExistente;
        }

        public async Task DeleteAsync(int ingrediente_id)
        {
            // validamos que el ingrediente a eliminar si exista con ese Id
            var ingredienteExistente = await _ingredienteRepository
                .GetByAttributeAsync<int>(ingrediente_id, "id");

            if (ingredienteExistente.Id ==0)
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
            catch (DbOperationException)
            {
                throw;
            }
        }

        private async Task<IngredienteDetallado> BuildDetailedIngredientAsync(Ingrediente ingrediente)
        {
            IngredienteDetallado unIngredienteDetallado = new()
            {
                Id = ingrediente.Id,
                Nombre = ingrediente.Nombre,
                Tipo_Ingrediente_Id = ingrediente.Tipo_Ingrediente_Id,
                Tipo_Ingrediente = ingrediente.Tipo_Ingrediente
            };

            var cervezasAsociadas = await _ingredienteRepository
                .GetAssociatedBeersAsync(ingrediente.Id);
            unIngredienteDetallado.Cervezas = cervezasAsociadas.ToList();

            //Colocamos los valores del rango ABV a las cervezas
            foreach (Cerveza unaCerveza in unIngredienteDetallado.Cervezas)
                unaCerveza.Rango_Abv = await _cervezaRepository
                    .GetAbvRangeNameAsync(unaCerveza.Abv);

            return unIngredienteDetallado;
        }

        private static IEnumerable<Ingrediente> ApplyOrder<T>(IEnumerable<Ingrediente> estilos, Func<Ingrediente, T> criterio, string orden)
        {
            if (orden.ToLower().Equals("desc"))
                return estilos.OrderByDescending(criterio);
            else
                return estilos.OrderBy(criterio);
        }
    }
}
