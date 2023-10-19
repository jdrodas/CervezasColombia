using CervezasColombia_CS_API_Mongo.DbContexts;
using CervezasColombia_CS_API_Mongo.Interfaces;
using CervezasColombia_CS_API_Mongo.Models;
using MongoDB.Driver;

namespace CervezasColombia_CS_API_Mongo.Repositories
{
    public class UbicacionRepository : IUbicacionRepository
    {
        private readonly MongoDbContext contextoDB;

        public UbicacionRepository(MongoDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Ubicacion>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();
            var coleccionUbicaciones = conexion.GetCollection<Ubicacion>("ubicaciones");

            var lasUbicaciones = await coleccionUbicaciones
                .Find(_ => true)
                .SortBy(ubicacion => ubicacion.Departamento)
                .ToListAsync();

            return lasUbicaciones;
        }


        public async Task<Ubicacion> GetByIdAsync(string ubicacion_id)
        {
            Ubicacion unaUbicacion = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionUbicaciones = conexion.GetCollection<Ubicacion>("ubicaciones");

            var resultado = await coleccionUbicaciones
                .Find(ubicacion => ubicacion.Id == ubicacion_id)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaUbicacion = resultado;

            return unaUbicacion;
        }

        public async Task<Ubicacion> GetByNameAsync(string ubicacion_municipio, string ubicacion_departamento)
        {
            Ubicacion unaUbicacion = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionUbicaciones = conexion.GetCollection<Ubicacion>("ubicaciones");

            var builder = Builders<Ubicacion>.Filter;
            var filtro = builder.And(
                builder.Eq(ubicacion => ubicacion.Municipio, ubicacion_municipio),
                builder.Eq(ubicacion => ubicacion.Departamento, ubicacion_departamento));

            var resultado = await coleccionUbicaciones
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaUbicacion = resultado;

            return unaUbicacion;
        }

        public async Task<Ubicacion> GetByNameAsync(string ubicacion_nombre)
        {
            Ubicacion unaUbicacion = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionUbicaciones = conexion.GetCollection<Ubicacion>("ubicaciones");

            string[] partesUbicacion = ubicacion_nombre.Split(',');

            var builder = Builders<Ubicacion>.Filter;
            var filtro = builder.And(
                builder.Eq(ubicacion => ubicacion.Municipio, partesUbicacion[0].Trim()),
                builder.Eq(ubicacion => ubicacion.Departamento, partesUbicacion[1].Trim()));

            var resultado = await coleccionUbicaciones
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaUbicacion = resultado;

            return unaUbicacion;
        }

        //TODO: UbicacionRepository: Obtener total cervecerias asociadas

        //public async Task<int> GetTotalAssociatedBreweriesAsync(int ubicacion_id)
        //{
        //    using (var conexion = contextoDB.CreateConnection())
        //    {
        //        DynamicParameters parametrosSentencia = new DynamicParameters();
        //        parametrosSentencia.Add("@ubicacion_id", ubicacion_id,
        //                                DbType.Int32, ParameterDirection.Input);

        //        string sentenciaSQL = "SELECT COUNT(id) total_cervecerias " +
        //                              "FROM cervecerias " +
        //                              "WHERE ubicacion_id = @ubicacion_id";


        //        var totalCervecerias = await conexion.QueryFirstAsync<int>(sentenciaSQL,
        //                                parametrosSentencia);

        //        return totalCervecerias;
        //    }
        //}

        //TODO: UbicacionRepository: Obtener cervecerias asociadas

        //public async Task<IEnumerable<Cerveceria>> GetAssociatedBreweriesAsync(int ubicacion_id)
        //{
        //    using (var conexion = contextoDB.CreateConnection())
        //    {
        //        DynamicParameters parametrosSentencia = new DynamicParameters();
        //        parametrosSentencia.Add("@ubicacion_id", ubicacion_id,
        //                                DbType.Int32, ParameterDirection.Input);

        //        string sentenciaSQL =   "SELECT v.cervceria_id id, v.cerveceria nombre, v.sitio_web, c.instagram, " +
        //                                "v.ubicacion, v.ubicacion_id " +
        //                                "FROM v_info_cervecerias v " +
        //                                "where v.ubicacion_id = @ubicacion_id";

        //        var resultadoCervecerias = await conexion.QueryAsync<Cerveceria>(sentenciaSQL, parametrosSentencia);

        //        return resultadoCervecerias;
        //    }
        //}

        public async Task<bool> CreateAsync(Ubicacion unaUbicacion)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionUbicaciones = conexion.GetCollection<Ubicacion>("ubicaciones");

            await coleccionUbicaciones
                .InsertOneAsync(unaUbicacion);

            var resultado = await GetByNameAsync(unaUbicacion.Municipio, unaUbicacion.Departamento);

            if (resultado is not null)
                resultadoAccion = true;

            return resultadoAccion;
        }

        //TODO: UbicacionRepository: Actualizar Ubicación

        //public async Task<bool> UpdateAsync(Ubicacion unaUbicacion)
        //{
        //    bool resultadoAccion = false;

        //    try
        //    {
        //        using (var conexion = contextoDB.CreateConnection())
        //        {
        //            string procedimiento = "core.p_actualiza_ubicacion";
        //            var parametros = new
        //            {
        //                p_id            = unaUbicacion.Id,
        //                p_municipio     = unaUbicacion.Municipio,
        //                p_departamento  = unaUbicacion.Departamento,
        //                p_latitud       = unaUbicacion.Latitud,
        //                p_longitud      = unaUbicacion.Longitud
        //            };

        //            var cantidad_filas = await conexion.ExecuteAsync(
        //                procedimiento,
        //                parametros,
        //                commandType: CommandType.StoredProcedure);

        //            if (cantidad_filas != 0)
        //                resultadoAccion = true;
        //        }
        //    }
        //    catch (NpgsqlException error)
        //    {
        //        throw new DbOperationException(error.Message);
        //    }

        //    return resultadoAccion;
        //}

        //TODO: UbicacionRepository: Borrar Urbanización

        //public async Task<bool> DeleteAsync(Ubicacion unaUbicacion)
        //{
        //    bool resultadoAccion = false;

        //    try
        //    {
        //        using (var conexion = contextoDB.CreateConnection())
        //        {
        //            string procedimiento = "core.p_elimina_ubicacion";
        //            var parametros = new
        //            {
        //                p_id = unaUbicacion.Id
        //            };

        //            var cantidad_filas = await conexion.ExecuteAsync(
        //                procedimiento,
        //                parametros,
        //                commandType: CommandType.StoredProcedure);

        //            if (cantidad_filas != 0)
        //                resultadoAccion = true;
        //        }
        //    }
        //    catch (NpgsqlException error)
        //    {
        //        throw new DbOperationException(error.Message);
        //    }

        //    return resultadoAccion;
        //}
    }
}