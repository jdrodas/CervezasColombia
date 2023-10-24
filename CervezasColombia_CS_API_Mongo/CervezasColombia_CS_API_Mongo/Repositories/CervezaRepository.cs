using CervezasColombia_CS_API_Mongo.DbContexts;
using CervezasColombia_CS_API_Mongo.Interfaces;
using CervezasColombia_CS_API_Mongo.Models;
using MongoDB.Driver;

namespace CervezasColombia_CS_API_Mongo.Repositories
{
    public class CervezaRepository : ICervezaRepository
    {
        private readonly MongoDbContext contextoDB;

        public CervezaRepository(MongoDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Cerveza>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();
            var coleccionCervezas = conexion.GetCollection<Cerveza>(contextoDB.configuracionColecciones.ColeccionCervezas);

            var lasCervezas = await coleccionCervezas
                .Find(_ => true)
                .SortBy(cerveza => cerveza.Nombre)
                .ToListAsync();

            //Aqui les colocamos los valores de los rangos de Abv y de Ibu
            foreach (Cerveza unaCerveza in lasCervezas)
            {
                unaCerveza.Rango_Ibu = await GetIbuRangeNameAsync(unaCerveza.Ibu);
                unaCerveza.Rango_Abv = await GetAbvRangeNameAsync(unaCerveza.Abv);
            }

            return lasCervezas;
        }

        public async Task<Cerveza> GetByIdAsync(string cerveza_id)
        {
            Cerveza unaCerveza = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionCervecerias = conexion.GetCollection<Cerveza>(contextoDB.configuracionColecciones.ColeccionCervezas);

            var resultado = await coleccionCervecerias
                .Find(cerveza => cerveza.Id == cerveza_id)
                .FirstOrDefaultAsync();

            if (resultado is not null)
            {
                unaCerveza = resultado;
                unaCerveza.Rango_Ibu = await GetIbuRangeNameAsync(unaCerveza.Ibu);
                unaCerveza.Rango_Abv = await GetAbvRangeNameAsync(unaCerveza.Abv);
            }

            return unaCerveza;
        }

        public async Task<Cerveza> GetByNameAndBreweryAsync(string cerveza_nombre, string cerveceria)
        {
            Cerveza unaCerveza = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionCervezas = conexion.GetCollection<Cerveza>(contextoDB.configuracionColecciones.ColeccionCervezas);

            var builder = Builders<Cerveza>.Filter;
            var filtro = builder.And(
                builder.Eq(cerveza => cerveza.Nombre, cerveza_nombre),
                builder.Eq(cerveza => cerveza.Cerveceria, cerveceria));

            var resultado = await coleccionCervezas
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
            {
                unaCerveza = resultado;
                unaCerveza.Rango_Ibu = await GetIbuRangeNameAsync(unaCerveza.Ibu);
                unaCerveza.Rango_Abv = await GetAbvRangeNameAsync(unaCerveza.Abv);
            }

            return unaCerveza;
        }

        public async Task<int> GetTotalAssociatedIngredientsAsync(string cerveza_id)
        {
            var losIngredientesAsociados = await GetAssociatedIngredientsAsync(cerveza_id);

            return losIngredientesAsociados.Count();               
        }

        public async Task<IEnumerable<IngredienteCerveza>> GetAssociatedIngredientsAsync(string cerveza_id)
        {
            Cerveza unaCerveza = await GetByIdAsync(cerveza_id);
            
            var conexion = contextoDB.CreateConnection();
            var coleccionIngredientesCervezas = conexion.GetCollection<IngredienteCerveza>("ingredientes_cervezas");

            var builder = Builders<IngredienteCerveza>.Filter;
            var filtro = builder.And(
                builder.Eq(ingredienteCerveza => ingredienteCerveza.Cerveceria, unaCerveza.Cerveceria),
                builder.Eq(ingredienteCerveza => ingredienteCerveza.Cerveza, unaCerveza.Nombre));

            var losIngredientesCervezas = await coleccionIngredientesCervezas
                .Find(filtro)
                .SortBy(ingredienteCerveza => ingredienteCerveza.Ingrediente)
                .ToListAsync();

            return losIngredientesCervezas;
        }

        public async Task<int> GetTotalAssociatedPackagingsAsync(string cerveza_id)
        {
            var losEnvasadosAsociados = await GetAssociatedPackagingsAsync(cerveza_id);

            return losEnvasadosAsociados.Count();
        }

        public async Task<IEnumerable<EnvasadoCerveza>> GetAssociatedPackagingsAsync(string cerveza_id)
        {
            Cerveza unaCerveza = await GetByIdAsync(cerveza_id);

            var conexion = contextoDB.CreateConnection();
            var coleccionEnvasadosCerveza = conexion.GetCollection<EnvasadoCerveza>("envasados_cervezas");

            var builder = Builders<EnvasadoCerveza>.Filter;
            var filtro = builder.And(
                builder.Eq(envasadoCerveza => envasadoCerveza.Cerveceria, unaCerveza.Cerveceria),
                builder.Eq(envasadoCerveza => envasadoCerveza.Cerveza, unaCerveza.Nombre));

            var losEnvasadosCervezas = await coleccionEnvasadosCerveza
                .Find(filtro)
                .SortBy(envasadoCerveza => envasadoCerveza.Envasado)
                .ToListAsync();

            return losEnvasadosCervezas;
        }

        public async Task<EnvasadoCerveza> GetPackagedBeerByNameAsync(string cerveza_id, string envasado_nombre, string unidad_volumen, double volumen)
        {
            Cerveza unaCerveza = await GetByIdAsync(cerveza_id);

            var conexion = contextoDB.CreateConnection();
            var coleccionEnvasadosCerveza = conexion.GetCollection<EnvasadoCerveza>("envasados_cervezas");

            var builder = Builders<EnvasadoCerveza>.Filter;
            var filtro = builder.And(
                builder.Eq(envasadoCerveza => envasadoCerveza.Cerveceria, unaCerveza.Cerveceria),
                builder.Eq(envasadoCerveza => envasadoCerveza.Cerveza, unaCerveza.Nombre),
                builder.Eq(envasadoCerveza => envasadoCerveza.Envasado, envasado_nombre),
                builder.Eq(envasadoCerveza => envasadoCerveza.Unidad_Volumen, unidad_volumen),
                builder.Eq(envasadoCerveza => envasadoCerveza.Volumen, volumen)                );

            var unEnvasadoCerveza = await coleccionEnvasadosCerveza
                .Find(filtro)
                .FirstOrDefaultAsync();

            return unEnvasadoCerveza;
        }

        public async Task<string> GetIbuRangeNameAsync(double ibu)
        {
            string unRangoIbu = string.Empty;

            var conexion = contextoDB.CreateConnection();
            var coleccionRangosIbu = conexion.GetCollection<RangoIbu>("rangos_ibu");

            var builder = Builders<RangoIbu>.Filter;
            var filtro = builder.Gte("valor_final", ibu) & builder.Lte("valor_inicial", ibu);

            var resultado = await coleccionRangosIbu
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unRangoIbu = resultado.Nombre;

            return unRangoIbu;
        }

        public async Task<string> GetAbvRangeNameAsync(double abv)
        {
            string unRangoAbv = string.Empty;

            var conexion = contextoDB.CreateConnection();
            var coleccionRangosAbv = conexion.GetCollection<RangoAbv>("rangos_abv");

            var builder = Builders<RangoAbv>.Filter;
            var filtro = builder.Gte("valor_final", abv) & builder.Lte("valor_inicial", abv);

            var resultado = await coleccionRangosAbv
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unRangoAbv = resultado.Nombre;

            return unRangoAbv;
        }

        public async Task<bool> CreateAsync(Cerveza unaCerveza)
        {
            bool resultadoAccion = false;

            //Obtenemos los valores para los rangos Ibu y Abv
            unaCerveza.Rango_Ibu = await GetIbuRangeNameAsync(unaCerveza.Ibu);
            unaCerveza.Rango_Abv = await GetAbvRangeNameAsync(unaCerveza.Abv);

            var conexion = contextoDB.CreateConnection();
            var coleccionCervezas = conexion.GetCollection<Cerveza>(contextoDB.configuracionColecciones.ColeccionCervezas);

            await coleccionCervezas
                .InsertOneAsync(unaCerveza);

            var resultado = await GetByNameAndBreweryAsync(unaCerveza.Nombre, unaCerveza.Cerveceria);

            if (resultado is not null)
            {
                //Aqui registramos el envasado predeterminado: Botella de 330 ml
                EnvasadoCerveza unEnvasadoCerveza = new()
                {
                    Cerveceria = unaCerveza.Cerveceria,
                    Cerveza = unaCerveza.Nombre,
                    Envasado = "Botella",
                    Unidad_Volumen = "Mililitros",
                    Volumen = 330
                };

                var coleccionEnvasadosCervezas = conexion.GetCollection<EnvasadoCerveza>("envasados_cervezas");
                await coleccionEnvasadosCervezas
                    .InsertOneAsync(unEnvasadoCerveza);

                //Aqui registramos el ingrediente preeterminado: Agua - Agua de Manantial
                IngredienteCerveza unIngredienteCerveza = new()
                {
                    Cerveceria = unaCerveza.Cerveceria,
                    Cerveza = unaCerveza.Nombre,
                    Tipo_Ingrediente = "Agua",
                    Ingrediente = "Agua de Manantial"
                };

                var coleccionIngredientesCervezas = conexion.GetCollection<IngredienteCerveza>("ingredientes_cervezas");
                await coleccionIngredientesCervezas
                    .InsertOneAsync(unIngredienteCerveza);

                resultadoAccion = true;
            }
                

            return resultadoAccion;
        }

        public async Task<bool> CreateBeerPackagingAsync(EnvasadoCerveza unEnvasadoCerveza)
        {
            bool resultadoAccion = false;

            var unaCerveza = await GetByNameAndBreweryAsync(
                unEnvasadoCerveza.Cerveza,
                unEnvasadoCerveza.Cerveceria);

            var conexion = contextoDB.CreateConnection();
            var coleccionCervecerias = conexion.GetCollection<EnvasadoCerveza>("envasados_cervezas");

            await coleccionCervecerias
                .InsertOneAsync(unEnvasadoCerveza);

            var resultado = await GetPackagedBeerByNameAsync(unaCerveza.Id!,
                unEnvasadoCerveza.Envasado,
                unEnvasadoCerveza.Unidad_Volumen,
                unEnvasadoCerveza.Volumen);

            if (resultado is not null)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> CreateBeerIngredientAsync(string cerveza_id, Ingrediente unIngrediente)
        {
            bool resultadoAccion = false;

            var unaCerveza = await GetByIdAsync(cerveza_id);

            IngredienteCerveza unIngredienteCerveza = new()
            {
                Cerveza = unaCerveza.Nombre,
                Cerveceria = unaCerveza.Cerveceria,
                Ingrediente = unIngrediente.Nombre,
                Tipo_Ingrediente = unIngrediente.Tipo_Ingrediente
            };

            var conexion = contextoDB.CreateConnection();
            var coleccionIngredientesCervezas = conexion.GetCollection<IngredienteCerveza>("ingredientes_cervezas");

            await coleccionIngredientesCervezas
                .InsertOneAsync(unIngredienteCerveza);

            //Aqui validamos que la inserción quedó correcta
            var builder = Builders<IngredienteCerveza>.Filter;
            var filtroIngredienteCerveza = builder.And(
                builder.Eq(ingredienteCerveza => ingredienteCerveza.Cerveceria, unIngredienteCerveza.Cerveceria),
                builder.Eq(ingredienteCerveza => ingredienteCerveza.Cerveza, unIngredienteCerveza.Cerveza),
                builder.Eq(ingredienteCerveza => ingredienteCerveza.Ingrediente, unIngredienteCerveza.Ingrediente),
                builder.Eq(ingredienteCerveza => ingredienteCerveza.Tipo_Ingrediente, unIngredienteCerveza.Tipo_Ingrediente));

            var resultadoIngredienteCerveza = await coleccionIngredientesCervezas
                .Find(filtroIngredienteCerveza)
                .FirstOrDefaultAsync();

            if (resultadoIngredienteCerveza is not null)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Cerveza unaCerveza)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionEstilos = conexion.GetCollection<Cerveza>(contextoDB.configuracionColecciones.ColeccionCervezas);

            var resultado = await coleccionEstilos
                .ReplaceOneAsync(cerveza => cerveza.Id == unaCerveza.Id, unaCerveza);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> DeleteAsync(Cerveza unaCerveza)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();

            //Aqui borramos los ingredientes asociados a la cerveza
            var coleccionIngredientesCervezas = conexion.GetCollection<IngredienteCerveza>("ingredientes_cervezas");
            
            var builderIngredienteCerveza = Builders<IngredienteCerveza>.Filter;
            var filtroIngredienteCerveza = builderIngredienteCerveza.And(
                builderIngredienteCerveza.Eq(ingredienteCerveza => ingredienteCerveza.Cerveza, unaCerveza.Nombre),
                builderIngredienteCerveza.Eq(ingredienteCerveza => ingredienteCerveza.Cerveceria, unaCerveza.Cerveceria));

            await coleccionIngredientesCervezas
                .DeleteManyAsync(filtroIngredienteCerveza);

            //Aqui borramos los envasados asociados a la cerveza
            var coleccionEnvasadosCervezas = conexion.GetCollection<EnvasadoCerveza>("envasados_cervezas");

            var builderEnvasadoCerveza = Builders<EnvasadoCerveza>.Filter;
            var filtroEnvasadoCerveza = builderEnvasadoCerveza.And(
                builderEnvasadoCerveza.Eq(envasadoCerveza => envasadoCerveza.Cerveza, unaCerveza.Nombre),
                builderEnvasadoCerveza.Eq(envasadoCerveza => envasadoCerveza.Cerveceria, unaCerveza.Cerveceria));

            await coleccionEnvasadosCervezas
                .DeleteManyAsync(filtroEnvasadoCerveza);

            //Aqui Borramos la cerveza
            var coleccionCervezas = conexion.GetCollection<Cerveza>(contextoDB.configuracionColecciones.ColeccionCervezas);

            var resultado = await coleccionCervezas
                .DeleteOneAsync(cerveza => cerveza.Id == unaCerveza.Id);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> DeleteBeerPackagingAsync(string cerveza_id, EnvasadoCerveza unEnvasadoCerveza)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            //Aqui borramos los envasados asociados a la cerveza
            var coleccionEnvasadosCervezas = conexion.GetCollection<EnvasadoCerveza>("envasados_cervezas");

            var builderEnvasadoCerveza = Builders<EnvasadoCerveza>.Filter;
            var filtroEnvasadoCerveza = builderEnvasadoCerveza.And(
                builderEnvasadoCerveza.Eq(envasadoCerveza => envasadoCerveza.Cerveza, unEnvasadoCerveza.Cerveza),
                builderEnvasadoCerveza.Eq(envasadoCerveza => envasadoCerveza.Cerveceria, unEnvasadoCerveza.Cerveceria),
                builderEnvasadoCerveza.Eq(envasadoCerveza => envasadoCerveza.Envasado, unEnvasadoCerveza.Envasado),
                builderEnvasadoCerveza.Eq(envasadoCerveza => envasadoCerveza.Unidad_Volumen, unEnvasadoCerveza.Unidad_Volumen),
                builderEnvasadoCerveza.Eq(envasadoCerveza => envasadoCerveza.Volumen, unEnvasadoCerveza.Volumen));

            var resultado = await coleccionEnvasadosCervezas
                .DeleteOneAsync(filtroEnvasadoCerveza);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> DeleteBeerIngredientAsync(string cerveza_id, Ingrediente unIngrediente)
        {
            bool resultadoAccion = false;

            var unaCerveza = await GetByIdAsync(cerveza_id);

            var conexion = contextoDB.CreateConnection();
            var coleccionIngredientesCervezas = conexion.GetCollection<IngredienteCerveza>("ingredientes_cervezas");

            var builder = Builders<IngredienteCerveza>.Filter;
            var filtroIngredienteCerveza = builder.And(
                builder.Eq(ingredienteCerveza => ingredienteCerveza.Cerveceria, unaCerveza.Cerveceria),
                builder.Eq(ingredienteCerveza => ingredienteCerveza.Cerveza, unaCerveza.Nombre),
                builder.Eq(ingredienteCerveza => ingredienteCerveza.Ingrediente, unIngrediente.Nombre),
                builder.Eq(ingredienteCerveza => ingredienteCerveza.Tipo_Ingrediente, unIngrediente.Tipo_Ingrediente));

            var resultado = await coleccionIngredientesCervezas
                .DeleteOneAsync(filtroIngredienteCerveza);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }
    }
}
