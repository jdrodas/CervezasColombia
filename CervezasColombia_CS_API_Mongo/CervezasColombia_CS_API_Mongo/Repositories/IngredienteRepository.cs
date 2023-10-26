using CervezasColombia_CS_API_Mongo.DbContexts;
using CervezasColombia_CS_API_Mongo.Helpers;
using CervezasColombia_CS_API_Mongo.Interfaces;
using CervezasColombia_CS_API_Mongo.Models;
using MongoDB.Driver;
using System.Data;

namespace CervezasColombia_CS_API_Mongo.Repositories
{
    public class IngredienteRepository : IIngredienteRepository
    {
        private readonly MongoDbContext contextoDB;

        public IngredienteRepository(MongoDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Ingrediente>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();
            var coleccionIngredientes = conexion.GetCollection<Ingrediente>(contextoDB.configuracionColecciones.ColeccionIngredientes);

            var losIngredientes = await coleccionIngredientes
                .Find(_ => true)
                .SortBy(ingrediente => ingrediente.Nombre)
                .ToListAsync();

            return losIngredientes;
        }

        public async Task<Ingrediente> GetByIdAsync(string ingrediente_id)
        {
            Ingrediente unIngrediente = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionIngredientes = conexion.GetCollection<Ingrediente>(contextoDB.configuracionColecciones.ColeccionIngredientes);

            var resultado = await coleccionIngredientes
                .Find(ingrediente => ingrediente.Id == ingrediente_id)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unIngrediente = resultado;

            return unIngrediente;
        }

        public async Task<Ingrediente> GetByNameAndTypeAsync(string ingrediente_nombre, string ingrediente_tipo)
        {
            Ingrediente unIngrediente = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionIngredientes = conexion.GetCollection<Ingrediente>(contextoDB.configuracionColecciones.ColeccionIngredientes);

            var builder = Builders<Ingrediente>.Filter;
            var filtro = builder.And(
                builder.Eq(ingrediente => ingrediente.Nombre, ingrediente_nombre),
                builder.Eq(ingrediente => ingrediente.Tipo_Ingrediente, ingrediente_tipo));

            var resultado = await coleccionIngredientes
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unIngrediente = resultado;

            return unIngrediente;
        }

        public async Task<int> GetTotalAssociatedBeersAsync(string ingrediente_id)
        {
            var lasCervezasAsociadas = await GetAssociatedBeersAsync(ingrediente_id);

            return lasCervezasAsociadas.Count();
        }

        public async Task<IEnumerable<IngredienteCerveza>> GetAssociatedBeersAsync(string ingrediente_id)
        {
            Ingrediente unIngrediente = await GetByIdAsync(ingrediente_id);

            var conexion = contextoDB.CreateConnection();
            var coleccionIngredientesCervezas = conexion.GetCollection<IngredienteCerveza>(contextoDB.configuracionColecciones.ColeccionIngredientesCervezas);

            var losIngredientesCervezas = await coleccionIngredientesCervezas
                .Find(ingrediente_cerveza => ingrediente_cerveza.Ingrediente == unIngrediente.Nombre)
                .SortBy(ingrediente_cerveza => ingrediente_cerveza.Cerveza)
                .ToListAsync();

            return losIngredientesCervezas;
        }

        public async Task<Cerveza> GetAssociatedBeerByIdAsync(string ingrediente_id, string cerveza_id)
        {
            Cerveza cervezaExistente = new();
            Ingrediente ingredienteExistente = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionCervezas = conexion.GetCollection<Cerveza>(contextoDB.configuracionColecciones.ColeccionCervezas);

            var resultadoCerveza = await coleccionCervezas
                .Find(cerveza => cerveza.Id == cerveza_id)
                .FirstOrDefaultAsync();

            if (resultadoCerveza is not null)
                cervezaExistente = resultadoCerveza;

            var coleccionIngredientes = conexion.GetCollection<Ingrediente>(contextoDB.configuracionColecciones.ColeccionIngredientes);

            var resultadoIngredientes= await coleccionIngredientes
                .Find(ingrediente => ingrediente.Id == ingrediente_id)
                .FirstOrDefaultAsync();

            if (resultadoIngredientes is not null)
                ingredienteExistente = resultadoIngredientes;

            var coleccionIngredientesCervezas = conexion.GetCollection<IngredienteCerveza>("ingredientes_cervezas");

            var builder = Builders<IngredienteCerveza>.Filter;
            var filtroIngredienteCerveza = builder.And(
                builder.Eq(ingredienteCerveza => ingredienteCerveza.Cerveceria, cervezaExistente.Cerveceria),
                builder.Eq(ingredienteCerveza => ingredienteCerveza.Cerveza, cervezaExistente.Nombre),
                builder.Eq(ingredienteCerveza => ingredienteCerveza.Ingrediente, ingredienteExistente.Nombre),
                builder.Eq(ingredienteCerveza => ingredienteCerveza.Tipo_Ingrediente, ingredienteExistente.Tipo_Ingrediente));

            var resultadoIngredienteCerveza = await coleccionIngredientesCervezas
                .Find(filtroIngredienteCerveza)
                .FirstOrDefaultAsync();

            if (resultadoIngredienteCerveza is not null)
                return cervezaExistente;
            else
                return new Cerveza();

        }


        public async Task<string> GetAssociatedIngredientTypeIdAsync(string tipo_ingrediente_nombre)
        {
            TipoIngrediente unTipoIngrediente = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionTiposIngredientes = conexion.GetCollection<TipoIngrediente>(contextoDB.configuracionColecciones.ColeccionTiposIngredientes);

            var resultado = await coleccionTiposIngredientes
                .Find(tipoIngrediente => tipoIngrediente.Nombre == tipo_ingrediente_nombre)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unTipoIngrediente = resultado;

            return unTipoIngrediente.Id!;
        }


        public async Task<bool> CreateAsync(Ingrediente unIngrediente)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionIngredientes = conexion.GetCollection<Ingrediente>(contextoDB.configuracionColecciones.ColeccionIngredientes);

            await coleccionIngredientes
                .InsertOneAsync(unIngrediente);

            var builder = Builders<Ingrediente>.Filter;
            var filtro = builder.And(
                builder.Eq(ingrediente => ingrediente.Nombre, unIngrediente.Nombre),
                builder.Eq(ingrediente => ingrediente.Tipo_Ingrediente, unIngrediente.Tipo_Ingrediente));

            var resultado = await coleccionIngredientes
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Ingrediente unIngrediente)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionIngredientes = conexion.GetCollection<Ingrediente>(contextoDB.configuracionColecciones.ColeccionIngredientes);

            var resultado = await coleccionIngredientes
                .ReplaceOneAsync(ingrediente => ingrediente.Id == unIngrediente.Id, unIngrediente);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> DeleteAsync(Ingrediente unIngrediente)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionIngredientes = conexion.GetCollection<Ingrediente>(contextoDB.configuracionColecciones.ColeccionIngredientes);

            var resultado = await coleccionIngredientes
                .DeleteOneAsync(ingrediente => ingrediente.Id == unIngrediente.Id);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }
    }
}
