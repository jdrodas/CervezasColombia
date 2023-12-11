using CervezasColombia_CS_API_SQLite_Dapper.Helpers;

namespace CervezasColombia_CS_API_SQLite_Dapper.Unidades
{
    public class UnidadService(IUnidadRepository unidadRepository)
    {
        private readonly IUnidadRepository _unidadRepository = unidadRepository;

        public async Task<UnidadResponse> GetAllAsync(UnidadQueryParameters parametrosConsultaUnidad)
        {
            var lasUnidades = await _unidadRepository
                .GetAllAsync();

            // Calculamos items totales y cantidad de páginas
            var totalElementos = lasUnidades.Count();
            var totalPaginas = (int)Math.Ceiling((double)totalElementos / parametrosConsultaUnidad.ElementosPorPagina);

            //Validamos que la página solicitada está dentro del rango permitido
            if (parametrosConsultaUnidad.Pagina > totalPaginas && totalPaginas > 0)
                throw new AppValidationException($"La página solicitada No. {parametrosConsultaUnidad.Pagina} excede el número total de página de {totalPaginas}");

            //Aplicamos el ordenamiento
            switch (parametrosConsultaUnidad.Criterio)
            {
                case "nombre":
                    lasUnidades = ApplyOrder(
                        lasUnidades,
                        p => p.Nombre,
                        parametrosConsultaUnidad.Orden);
                    break;
            }

            //Aplicamos la paginación
            lasUnidades = lasUnidades
                .Skip((parametrosConsultaUnidad.Pagina - 1) * parametrosConsultaUnidad.ElementosPorPagina)
                .Take(parametrosConsultaUnidad.ElementosPorPagina);

            var respuestaUnidades = new UnidadResponse
            {
                Tipo = "Unidad",
                TotalElementos = totalElementos,
                PaginaActual = parametrosConsultaUnidad.Pagina,
                ElementosPorPagina = parametrosConsultaUnidad.ElementosPorPagina, // PageSize
                TotalPaginas = totalPaginas,
                Data = lasUnidades.ToList()
            };

            return respuestaUnidades;
        }

        public async Task<Unidad> GetByAttributeAsync<T>(T atributo_valor, string atributo_nombre)
        {
            Unidad unaUnidad = new();

            switch (atributo_nombre.ToLower())
            {
                case "id":
                    unaUnidad = await _unidadRepository
                        .GetByAttributeAsync<T>(atributo_valor, "id");
                    break;

                case "nombre":
                    unaUnidad = await _unidadRepository
                        .GetByAttributeAsync<T>(atributo_valor, "nombre");
                    break;

                case "abreviatura":
                    unaUnidad = await _unidadRepository
                        .GetByAttributeAsync<T>(atributo_valor, "abreviatura");
                    break;
            }

            if (unaUnidad.Id == 0)
                throw new AppValidationException($"Estilo no encontrado con el atributo {atributo_nombre} {atributo_valor}");

            return unaUnidad;
        }

        private static IEnumerable<Unidad> ApplyOrder<T>(IEnumerable<Unidad> unidades, Func<Unidad, T> criterio, string orden)
        {
            if (orden.ToLower().Equals("desc"))
                return unidades.OrderByDescending(criterio);
            else
                return unidades.OrderBy(criterio);
        }
    }
}
