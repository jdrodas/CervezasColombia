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
            var coleccionIngredientes = conexion.GetCollection<Ingrediente>("ingredientes");

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
            var coleccionIngredientes = conexion.GetCollection<Ingrediente>("ingredientes");

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
            var coleccionIngredientes = conexion.GetCollection<Ingrediente>("ingredientes");

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

        //public async Task<int> GetTotalAssociatedBeersAsync(int ingrediente_id)
        //{
        //    var conexion = contextoDB.CreateConnection();

        //    DynamicParameters parametrosSentencia = new();
        //    parametrosSentencia.Add("@ingrediente_id", ingrediente_id,
        //                            DbType.Int32, ParameterDirection.Input);

        //    string sentenciaSQL = "SELECT COUNT(v.cerveza_id) totalCervezas " +
        //                            "FROM v_info_ingredientes_cervezas v " +
        //                            "WHERE v.ingrediente_id = @ingrediente_id ";

        //    var totalCervezas = await conexion.QueryFirstAsync<int>(sentenciaSQL,
        //                            parametrosSentencia);

        //    return totalCervezas;
        //}

        //public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int ingrediente_id)
        //{
        //    var conexion = contextoDB.CreateConnection();

        //    DynamicParameters parametrosSentencia = new();
        //    parametrosSentencia.Add("@ingrediente_id", ingrediente_id,
        //                            DbType.Int32, ParameterDirection.Input);

        //    string sentenciaSQL = "SELECT vc.cerveza_id id, vc.cerveza nombre, vc.cerveceria, vc.cerveceria_id, " +
        //                            "vc.estilo, vc.estilo_id, vc.ibu, vc.abv, vc.rango_ibu, vc.rango_abv " +
        //                            "FROM v_info_cervezas vc " +
        //                            "JOIN v_info_ingredientes_cervezas vi ON vc.cerveza_id = vi.cerveza_id " +
        //                            "WHERE vi.ingrediente_id = @ingrediente_id " +
        //                            "ORDER BY vc.cerveza_id DESC ";

        //    var resultadoCervezas = await conexion.QueryAsync<Cerveza>(sentenciaSQL, parametrosSentencia);

        //    return resultadoCervezas;
        //}

        public async Task<Cerveza> GetAssociatedBeerByIdAsync(string ingrediente_id, string cerveza_id)
        {
            Cerveza cervezaExistente = new();
            Ingrediente ingredienteExistente = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionCervezas = conexion.GetCollection<Cerveza>("cervezas");

            var resultadoCerveza = await coleccionCervezas
                .Find(cerveza => cerveza.Id == cerveza_id)
                .FirstOrDefaultAsync();

            if (resultadoCerveza is not null)
                cervezaExistente = resultadoCerveza;

            var coleccionIngredientes = conexion.GetCollection<Ingrediente>("ingredientes");

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


        //public async Task<int> GetAssociatedIngredientTypeIdAsync(string tipo_ingrediente_nombre)
        //{
        //    var conexion = contextoDB.CreateConnection();

        //    DynamicParameters parametrosSentencia = new();
        //    parametrosSentencia.Add("@tipo_ingrediente", tipo_ingrediente_nombre,
        //                            DbType.String, ParameterDirection.Input);

        //    string sentenciaSQL = "SELECT id FROM tipos_ingredientes ti " +
        //                            "WHERE LOWER(ti.nombre) = LOWER(@tipo_ingrediente) ";

        //    var resultadotipoIngrediente = await conexion.QueryAsync<int>(sentenciaSQL,
        //                                    parametrosSentencia);

        //    if (resultadotipoIngrediente.Any())
        //        return resultadotipoIngrediente.First();
        //    else
        //        return 0;
        //}


        //public async Task<bool> CreateAsync(Ingrediente unIngrediente)
        //{
        //    bool resultadoAccion = false;

        //    try
        //    {
        //        var conexion = contextoDB.CreateConnection();

        //        string procedimiento = "core.p_inserta_ingrediente";
        //        var parametros = new
        //        {
        //            p_nombre = unIngrediente.Nombre,
        //            p_tipo_ingrediente_id = unIngrediente.Tipo_Ingrediente_Id
        //        };

        //        var cantidad_filas = await conexion.ExecuteAsync(
        //            procedimiento,
        //            parametros,
        //            commandType: CommandType.StoredProcedure);

        //        if (cantidad_filas != 0)
        //            resultadoAccion = true;
        //    }
        //    catch (NpgsqlException error)
        //    {
        //        throw new DbOperationException(error.Message);
        //    }

        //    return resultadoAccion;
        //}

        //public async Task<bool> UpdateAsync(Ingrediente unIngrediente)
        //{
        //    bool resultadoAccion = false;

        //    try
        //    {
        //        var conexion = contextoDB.CreateConnection();

        //        string procedimiento = "core.p_actualiza_ingrediente";
        //        var parametros = new
        //        {
        //            p_id = unIngrediente.Id,
        //            p_nombre = unIngrediente.Nombre,
        //            p_tipo_ingrediente_id = unIngrediente.Tipo_Ingrediente_Id
        //        };

        //        var cantidad_filas = await conexion.ExecuteAsync(
        //            procedimiento,
        //            parametros,
        //            commandType: CommandType.StoredProcedure);

        //        if (cantidad_filas != 0)
        //            resultadoAccion = true;


        //    }
        //    catch (NpgsqlException error)
        //    {
        //        throw new DbOperationException(error.Message);
        //    }

        //    return resultadoAccion;
        //}

        //public async Task<bool> DeleteAsync(Ingrediente unIngrediente)
        //{
        //    bool resultadoAccion = false;

        //    try
        //    {
        //        var conexion = contextoDB.CreateConnection();

        //        string procedimiento = "core.p_elimina_ingrediente";
        //        var parametros = new
        //        {
        //            p_id = unIngrediente.Id
        //        };

        //        var cantidad_filas = await conexion.ExecuteAsync(
        //            procedimiento,
        //            parametros,
        //            commandType: CommandType.StoredProcedure);

        //        if (cantidad_filas != 0)
        //            resultadoAccion = true;
        //    }
        //    catch (NpgsqlException error)
        //    {
        //        throw new DbOperationException(error.Message);
        //    }

        //    return resultadoAccion;
        //}
    }
}
