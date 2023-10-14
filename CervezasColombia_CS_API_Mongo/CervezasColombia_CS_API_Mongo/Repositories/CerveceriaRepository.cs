using CervezasColombia_CS_API_Mongo.DbContexts;
using CervezasColombia_CS_API_Mongo.Helpers;
using CervezasColombia_CS_API_Mongo.Interfaces;
using CervezasColombia_CS_API_Mongo.Models;
using MongoDB.Driver;
using System.Data;

namespace CervezasColombia_CS_API_Mongo.Repositories
{
    public class CerveceriaRepository : ICerveceriaRepository
    {
        private readonly MongoDbContext contextoDB;

        public CerveceriaRepository(MongoDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Cerveceria>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();
            var coleccionCervecerias = conexion.GetCollection<Cerveceria>("cervecerias");

            var lasCervecerias = await coleccionCervecerias
                .Find(_ => true)
                .SortBy(cerveceria => cerveceria.Nombre)
                .ToListAsync();

            return lasCervecerias;
        }

        public async Task<Cerveceria> GetByIdAsync(string cerveceria_id)
        {
            Cerveceria unaCerveceria= new();

            var conexion = contextoDB.CreateConnection();
            var coleccionCervecerias = conexion.GetCollection<Cerveceria>("cervecerias");

            var resultado = await coleccionCervecerias
                .Find(cerveceria => cerveceria.Id == cerveceria_id)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaCerveceria = resultado;

            return unaCerveceria;
        }

        //public async Task<CerveceriaDetallada> GetDetailsByIdAsync(int cerveceria_id)
        //{
        //    CerveceriaDetallada unaCerveceriaDetallada = new();

        //    var conexion = contextoDB.CreateConnection();

        //    DynamicParameters parametrosSentencia = new();
        //    parametrosSentencia.Add("@cerveceria_id", cerveceria_id,
        //                            DbType.Int32, ParameterDirection.Input);

        //    string sentenciaSQL = "SELECT v.cerveceria_id id, v.cerveceria nombre, v.sitio_web, v.instagram " +
        //        "FROM v_info_cervecerias v " +
        //        "WHERE v.cerveceria_id = @cerveceria_id";

        //    var resultado = await conexion.QueryAsync<CerveceriaDetallada>(sentenciaSQL,
        //                        parametrosSentencia);

        //    if (resultado.Any())
        //    {
        //        unaCerveceriaDetallada = resultado.First();
        //        unaCerveceriaDetallada.Ubicacion = await GetAssociatedLocationAsync(unaCerveceriaDetallada.Id);

        //        //Aqui buscamos las cervezas asociadas
        //        var cervezasAsociadas = await GetAssociatedBeersAsync(unaCerveceriaDetallada.Id);
        //        unaCerveceriaDetallada.Cervezas = cervezasAsociadas.ToList();
        //    }

        //    return unaCerveceriaDetallada;
        //}

        public async Task<Cerveceria> GetByNameAsync(string cerveceria_nombre)
        {
            Cerveceria unaCerveceria = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionCervecerias = conexion.GetCollection<Cerveceria>("cervecerias");

            var resultado = await coleccionCervecerias
                .Find(cerveceria => cerveceria.Nombre == cerveceria_nombre)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaCerveceria = resultado;

            return unaCerveceria;
        }

        public async Task<Cerveceria> GetByInstagramAsync(string cerveceria_instagram)
        {
            Cerveceria unaCerveceria = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionCervecerias = conexion.GetCollection<Cerveceria>("cervecerias");

            var resultado = await coleccionCervecerias
                .Find(cerveceria => cerveceria.Instagram == cerveceria_instagram)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaCerveceria = resultado;

            return unaCerveceria;
        }

        public async Task<Cerveceria> GetBySitioWebAsync(string cerveceria_sitio_web)
        {
            Cerveceria unaCerveceria = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionCervecerias = conexion.GetCollection<Cerveceria>("cervecerias");

            var resultado = await coleccionCervecerias
                .Find(cerveceria => cerveceria.Sitio_Web == cerveceria_sitio_web)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaCerveceria = resultado;

            return unaCerveceria;
        }

        //public async Task<int> GetTotalAssociatedBeersAsync(int cerveceria_id)
        //{
        //    var conexion = contextoDB.CreateConnection();

        //    DynamicParameters parametrosSentencia = new();
        //    parametrosSentencia.Add("@cerveceria_id", cerveceria_id,
        //                            DbType.Int32, ParameterDirection.Input);

        //    string sentenciaSQL = "SELECT COUNT(id) totalCervezas " +
        //                          "FROM cervezas " +
        //                          "WHERE cerveceria_id = @cerveceria_id ";


        //    var totalCervezas = await conexion.QueryFirstAsync<int>(sentenciaSQL,
        //                            parametrosSentencia);

        //    return totalCervezas;
        //}

        //public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int cerveceria_id)
        //{
        //    var conexion = contextoDB.CreateConnection();

        //    DynamicParameters parametrosSentencia = new();
        //    parametrosSentencia.Add("@cerveceria_id", cerveceria_id,
        //                            DbType.Int32, ParameterDirection.Input);

        //    string sentenciaSQL = "SELECT cerveza_id id, cerveza nombre, cerveceria_id, cerveceria, estilo_id, " +
        //                            "estilo, ibu, abv, rango_ibu, rango_abv " +
        //                            "FROM v_info_cervezas " +
        //                            "WHERE cerveceria_id = @cerveceria_id " +
        //                            "ORDER BY id DESC";

        //    var resultadoCervezas = await conexion.QueryAsync<Cerveza>(sentenciaSQL, parametrosSentencia);

        //    return resultadoCervezas;
        //}

        //public async Task<Ubicacion> GetAssociatedLocationAsync(int cerveceria_id)
        //{
        //    Ubicacion unaUbicacion = new();

        //    var conexion = contextoDB.CreateConnection();

        //    DynamicParameters parametrosSentencia = new();
        //    parametrosSentencia.Add("@cerveceria_id", cerveceria_id,
        //                            DbType.Int32, ParameterDirection.Input);

        //    string sentenciaSQL = "SELECT u.id, u.municipio, u.departamento, u.latitud, u.longitud " +
        //                            "FROM ubicaciones u " +
        //                            "WHERE u.id = (select c.ubicacion_id from cervecerias c where c.id = @cerveceria_id) ";

        //    var resultadoIdUbicacion = await conexion.QueryAsync<Ubicacion>(sentenciaSQL,
        //                                    parametrosSentencia);

        //    if (resultadoIdUbicacion.Any())
        //        unaUbicacion = resultadoIdUbicacion.First();

        //    return unaUbicacion;
        //}

        public async Task<bool> CreateAsync(Cerveceria unaCerveceria)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionCervecerias = conexion.GetCollection<Cerveceria>("cervecerias");

            await coleccionCervecerias
                .InsertOneAsync(unaCerveceria);

            var resultado = await GetByNameAsync(unaCerveceria.Nombre);

            if (resultado is not null)
                resultadoAccion = true;

            return resultadoAccion;
        }

        //public async Task<bool> UpdateAsync(Cerveceria unaCerveceria)
        //{
        //    bool resultadoAccion = false;

        //    try
        //    {
        //        var conexion = contextoDB.CreateConnection();

        //        string procedimiento = "core.p_actualiza_cerveceria";
        //        var parametros = new
        //        {
        //            p_id = unaCerveceria.Id,
        //            p_nombre = unaCerveceria.Nombre,
        //            p_ubicacion_id = unaCerveceria.Ubicacion.Id,
        //            p_sitio_web = unaCerveceria.Sitio_Web,
        //            p_instagram = unaCerveceria.Instagram
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

        //public async Task<bool> DeleteAsync(Cerveceria unaCerveceria)
        //{
        //    bool resultadoAccion = false;

        //    try
        //    {
        //        var conexion = contextoDB.CreateConnection();

        //        string procedimiento = "core.p_elimina_cerveceria";
        //        var parametros = new
        //        {
        //            p_id = unaCerveceria.Id
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
