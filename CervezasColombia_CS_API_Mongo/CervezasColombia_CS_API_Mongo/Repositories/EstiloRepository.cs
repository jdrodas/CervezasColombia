using CervezasColombia_CS_API_Mongo.DbContexts;
using CervezasColombia_CS_API_Mongo.Helpers;
using CervezasColombia_CS_API_Mongo.Interfaces;
using CervezasColombia_CS_API_Mongo.Models;
using MongoDB.Driver;
using System.Data;

namespace CervezasColombia_CS_API_Mongo.Repositories
{
    public class EstiloRepository : IEstiloRepository
    {
        private readonly MongoDbContext contextoDB;

        public EstiloRepository(MongoDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Estilo>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();
            var coleccionEstilos = conexion.GetCollection<Estilo>("estilos");

            var losEstilos = await coleccionEstilos
                .Find(_ => true)
                .SortBy(estilo => estilo.Nombre)
                .ToListAsync();

            return losEstilos;
        }

        public async Task<Estilo> GetByIdAsync(string estilo_id)
        {
            Estilo unEstilo = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionEstilos = conexion.GetCollection<Estilo>("estilos");

            var resultado = await coleccionEstilos
                .Find(estilo => estilo.Id == estilo_id)
                .SortBy(estilo => estilo.Nombre)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(resultado.Id))
                unEstilo = resultado;

            return unEstilo;
        }

        //public async Task<EstiloDetallado> GetDetailsByIdAsync(int estilo_id)
        //{
        //    EstiloDetallado unEstilo = new();

        //    var conexion = contextoDB.CreateConnection();

        //    DynamicParameters parametrosSentencia = new();
        //    parametrosSentencia.Add("@estilo_id", estilo_id,
        //                            DbType.Int32, ParameterDirection.Input);

        //    string sentenciaSQL = "SELECT id, nombre " +
        //                          "FROM estilos " +
        //                          "WHERE id = @estilo_id ";

        //    var resultado = await conexion.QueryAsync<EstiloDetallado>(sentenciaSQL,
        //                        parametrosSentencia);

        //    if (resultado.Any())
        //    {
        //        unEstilo = resultado.First();

        //        var lasCervezas = await GetAssociatedBeersAsync(unEstilo.Id);
        //        unEstilo.Cervezas = lasCervezas.ToList();
        //    }

        //    return unEstilo;
        //}

        //public async Task<Estilo> GetByNameAsync(string estilo_nombre)
        //{
        //    Estilo unEstilo = new();

        //    var conexion = contextoDB.CreateConnection();

        //    DynamicParameters parametrosSentencia = new();
        //    parametrosSentencia.Add("@estilo_nombre", estilo_nombre,
        //                            DbType.String, ParameterDirection.Input);

        //    string sentenciaSQL = "SELECT id, nombre " +
        //                          "FROM estilos " +
        //                          "WHERE LOWER(nombre) = LOWER(@estilo_nombre) ";

        //    var resultado = await conexion.QueryAsync<Estilo>(sentenciaSQL,
        //                        parametrosSentencia);

        //    if (resultado.Any())
        //        unEstilo = resultado.First();

        //    return unEstilo;
        //}

        //public async Task<int> GetTotalAssociatedBeersAsync(int estilo_id)
        //{
        //    var conexion = contextoDB.CreateConnection();

        //    DynamicParameters parametrosSentencia = new();
        //    parametrosSentencia.Add("@estilo_id", estilo_id,
        //                            DbType.Int32, ParameterDirection.Input);

        //    string sentenciaSQL = "SELECT COUNT(id) totalCervezas " +
        //                          "FROM cervezas " +
        //                          "WHERE estilo_id = @estilo_id";


        //    var totalCervezas = await conexion.QueryFirstAsync<int>(sentenciaSQL,
        //                            parametrosSentencia);

        //    return totalCervezas;
        //}

        //public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int estilo_id)
        //{
        //    var conexion = contextoDB.CreateConnection();

        //    DynamicParameters parametrosSentencia = new();
        //    parametrosSentencia.Add("@estilo_id", estilo_id,
        //                            DbType.Int32, ParameterDirection.Input);

        //    string sentenciaSQL = "SELECT cerveza_id id, cerveza nombre, cerveceria, cerveceria_id, estilo, estilo_id, " +
        //                            "ibu, abv, rango_ibu, rango_abv " +
        //                            "FROM v_info_cervezas " +
        //                            "WHERE estilo_id = @estilo_id " +
        //                            "ORDER BY cerveza_id DESC";

        //    var resultadoCervezas = await conexion.QueryAsync<Cerveza>(sentenciaSQL,
        //                                parametrosSentencia);

        //    return resultadoCervezas;
        //}

        //public async Task<bool> CreateAsync(Estilo unEstilo)
        //{
        //    bool resultadoAccion = false;

        //    try
        //    {
        //        var conexion = contextoDB.CreateConnection();

        //        string procedimiento = "core.p_inserta_estilo";
        //        var parametros = new
        //        {
        //            p_nombre = unEstilo.Nombre
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

        //public async Task<bool> UpdateAsync(Estilo unEstilo)
        //{
        //    bool resultadoAccion = false;

        //    try
        //    {
        //        var conexion = contextoDB.CreateConnection();

        //        string procedimiento = "core.p_actualiza_estilo";
        //        var parametros = new
        //        {
        //            p_id = unEstilo.Id,
        //            p_nombre = unEstilo.Nombre
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


        //public async Task<bool> DeleteAsync(Estilo unEstilo)
        //{
        //    bool resultadoAccion = false;

        //    try
        //    {
        //        var conexion = contextoDB.CreateConnection();

        //        string procedimiento = "core.p_elimina_estilo";
        //        var parametros = new
        //        {
        //            p_id = unEstilo.Id
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