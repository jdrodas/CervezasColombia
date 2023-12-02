using CervezasColombia_CS_API_SQLite_Dapper.DbContexts;
using CervezasColombia_CS_API_SQLite_Dapper.Exceptions;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;
using CervezasColombia_CS_API_SQLite_Dapper.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace CervezasColombia_CS_API_SQLite_Dapper.Repositories
{
    public class UbicacionRepository(SQLiteDbContext unContexto) : IUbicacionRepository
    {
        private readonly SQLiteDbContext contextoDB = unContexto;

        public async Task<IEnumerable<Ubicacion>> GetAllAsync()
        {
            string sentenciaSQL = "SELECT id, municipio, departamento, latitud, longitud " +
                                  "FROM ubicaciones " +
                                  "ORDER BY id DESC";

            var resultadoUbicaciones = await contextoDB.Conexion
                .QueryAsync<Ubicacion>(sentenciaSQL, new DynamicParameters());

            return resultadoUbicaciones;
        }

        public async Task<Ubicacion> GetByIdAsync(int ubicacion_id)
        {
            Ubicacion unaUbicacion = new();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ubicacion_id", ubicacion_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, municipio, departamento, latitud, longitud " +
                                  "FROM ubicaciones " +
                                  "WHERE id = @ubicacion_id ";

            var resultado = await contextoDB.Conexion
                .QueryAsync<Ubicacion>(sentenciaSQL,parametrosSentencia);

            if (resultado.Any())
                unaUbicacion = resultado.First();

            return unaUbicacion;
        }

        public async Task<Ubicacion> GetByNameAsync(string ubicacion_municipio, string ubicacion_departamento)
        {
            Ubicacion unaUbicacion = new();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ubicacion_municipio", ubicacion_municipio,
                                    DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@ubicacion_departamento", ubicacion_departamento,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, municipio, departamento, latitud, longitud " +
                                  "FROM ubicaciones " +
                                  "WHERE municipio = @ubicacion_municipio " +
                                  "AND departamento = @ubicacion_departamento";

            var resultado = await contextoDB.Conexion
                .QueryAsync<Ubicacion>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unaUbicacion = resultado.First();

            return unaUbicacion;
        }

        public async Task<Ubicacion> GetByNameAsync(string ubicacion_nombre)
        {
            Ubicacion unaUbicacion = new();

            string[] partesUbicacion = ubicacion_nombre.Split(',');

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ubicacion_municipio", partesUbicacion[0].Trim(),
                                    DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@ubicacion_departamento", partesUbicacion[1].Trim(),
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, municipio, departamento, latitud, longitud " +
                                  "FROM ubicaciones " +
                                  "WHERE municipio = @ubicacion_municipio " +
                                  "AND departamento = @ubicacion_departamento";

            var resultado = await contextoDB.Conexion
                .QueryAsync<Ubicacion>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unaUbicacion = resultado.First();

            return unaUbicacion;
        }


        public async Task<int> GetTotalAssociatedBreweriesAsync(int ubicacion_id)
        {

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ubicacion_id", ubicacion_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT COUNT(id) total_cervecerias " +
                                  "FROM cervecerias " +
                                  "WHERE ubicacion_id = @ubicacion_id";


            var totalCervecerias = await contextoDB.Conexion
                .QueryFirstAsync<int>(sentenciaSQL, parametrosSentencia);

            return totalCervecerias;
        }

        public async Task<IEnumerable<Cerveceria>> GetAssociatedBreweriesAsync(int ubicacion_id)
        {
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ubicacion_id", ubicacion_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT c.id, c.nombre, c.instagram " +
                                  "FROM cervecerias c JOIN ubicaciones u ON c.ubicacion_id = u.id " +
                                  "WHERE c.ubicacion_id = @ubicacion_id ";

            var resultadoCervecerias = await contextoDB.Conexion
                .QueryAsync<Cerveceria>(sentenciaSQL, parametrosSentencia);

            foreach(Cerveceria unaCerveceria in  resultadoCervecerias)
            {
                unaCerveceria.Ubicacion = await GetLocationForBrewery(unaCerveceria.Id);
            }

            return resultadoCervecerias;
        }

        public async Task<Ubicacion> GetLocationForBrewery(int cerveceria_id)
        {
            Ubicacion unaUbicacion = new();
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@cerveceria_id", cerveceria_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT u.id, u.municipio, u.departamento, u.latitud, u.longitud " +
                "FROM ubicaciones u JOIN cervecerias c on c.ubicacion_id = u.id " +
                "where c.id = @cerveceria_id";

            var resultado = await contextoDB.Conexion
                .QueryAsync<Ubicacion>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unaUbicacion = resultado.First();

            return unaUbicacion;
        }

        public async Task<bool> CreateAsync(Ubicacion unaUbicacion)
        {
            bool resultadoAccion = false;

            try
            {
                string sentenciaSQL = "INSERT INTO ubicaciones (municipio, departamento, latitud, longitud) " +
                                        "VALUES (@Municipio, @Departamento, @Latitud, @Longitud)";

                int filasAfectadas = await contextoDB.Conexion
                    .ExecuteAsync(sentenciaSQL, unaUbicacion);

                if (filasAfectadas > 0)
                    resultadoAccion = true;
            }
            catch (SqliteException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Ubicacion unaUbicacion)
        {
            bool resultadoAccion = false;

            try
            {
                string sentenciaSQL = "UPDATE ubicaciones SET municipio = @Municipio, " +
                                      "departamento = @Departamento, " +
                                      "latitud = @Latitud, " +
                                      "longitud = @Longitud " +
                                      "WHERE id = @Id";

                int filasAfectadas = await contextoDB.Conexion
                    .ExecuteAsync(sentenciaSQL, unaUbicacion);

                if (filasAfectadas > 0)
                    resultadoAccion = true;
            }
            catch (SqliteException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> DeleteAsync(Ubicacion unaUbicacion)
        {
            bool resultadoAccion = false;

            try
            {
                string sentenciaSQL = "DELETE FROM ubicaciones WHERE id = @Id";

                int filasAfectadas = await contextoDB.Conexion
                    .ExecuteAsync(sentenciaSQL, unaUbicacion);

                if (filasAfectadas > 0)
                    resultadoAccion = true;
            }
            catch (SqliteException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }
    }
}