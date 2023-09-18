using CervezasColombia_CS_API_SQLite_Dapper.DbContexts;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;
using CervezasColombia_CS_API_SQLite_Dapper.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace CervezasColombia_CS_API_SQLite_Dapper.Repositories
{
    public class UbicacionRepository : IUbicacionRepository
    {
        private readonly SQLiteDbContext contextoDB;

        public UbicacionRepository(SQLiteDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Ubicacion>> GetAllAsync()
        {
            using (contextoDB.Conexion)
            {
                string sentenciaSQL = "SELECT id, municipio, departamento " +
                      "FROM ubicaciones " +
                      "ORDER BY id DESC";

                var resultadoUbicaciones = await contextoDB.Conexion.QueryAsync<Ubicacion>(sentenciaSQL,
                                            new DynamicParameters());

                return resultadoUbicaciones;
            }
        }

        public async Task<Ubicacion> GetByIdAsync(int ubicacion_id)
        {
            Ubicacion unaUbicacion = new Ubicacion();

            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@ubicacion_id", ubicacion_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT id, municipio, departamento " +
                      "FROM ubicaciones " +
                      "WHERE id = @ubicacion_id ";

                var resultado = await contextoDB.Conexion.QueryFirstAsync<Ubicacion>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado is not null)
                    unaUbicacion = resultado;
            }

            return unaUbicacion;
        }

        public async Task<Ubicacion> GetByNameAsync(string ubicacion_municipio, string ubicacion_departamento)
        {
            Ubicacion unaUbicacion = new Ubicacion();

            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@ubicacion_municipio", ubicacion_municipio,
                                        DbType.String, ParameterDirection.Input);
                parametrosSentencia.Add("@ubicacion_departamento", ubicacion_departamento,
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "SELECT id, municipio, departamento " +
                      "FROM ubicaciones " +
                      "WHERE municipio = @ubicacion_municipio " +
                      "AND departamento = @ubicacion_departamento";

                var resultado = await contextoDB.Conexion.QueryFirstAsync<Ubicacion>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado is not null)
                    unaUbicacion = resultado;
            }

            return unaUbicacion;
        }

        public async Task<int> GetTotalAssociatedBreweriesAsync(int ubicacion_id)
        {

            DynamicParameters parametrosSentencia = new DynamicParameters();
            parametrosSentencia.Add("@ubicacion_id", ubicacion_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT COUNT(id) total_cervecerias " +
                                  "FROM cervecerias " +
                                  "WHERE ubicacion_id = @ubicacion_id";


            var totalCervecerias = await contextoDB.Conexion.QueryFirstAsync<int>(sentenciaSQL,
                                    parametrosSentencia);

            return totalCervecerias;
        }

        public async Task<IEnumerable<Cerveceria>> GetAssociatedBreweriesAsync(int ubicacion_id)
        {
            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@ubicacion_id", ubicacion_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT c.id, c.nombre, c.sitio_web, c.instagram, " +
                      "(u.municipio || ', ' || u.departamento) ubicacion, c.ubicacion_id " +
                      "FROM cervecerias c JOIN ubicaciones u ON c.ubicacion_id = u.id " +
                      "WHERE c.ubicacion_id = @ubicacion_id ";

                var resultadoCervecerias = await contextoDB.Conexion.QueryAsync<Cerveceria>(sentenciaSQL, parametrosSentencia);

                return resultadoCervecerias;
            }
        }

        public async Task<bool> CreateAsync(Ubicacion unaUbicacion)
        {
            bool resultadoAccion = false;

            try
            {
                using (contextoDB.Conexion)
                {
                    string sentenciaSQL = "INSERT INTO ubicaciones (municipio, departamento) " +
                                              "VALUES (@Municipio, @Departamento)";

                    int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL,
                                            unaUbicacion);

                    if (filasAfectadas > 0)
                        resultadoAccion = true;
                }
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
                using (contextoDB.Conexion)
                {
                    string sentenciaSQL = "UPDATE ubicaciones SET municipio = @Municipio, " +
                                          "departamento = @Departamento " +
                                          "WHERE id = @Id";

                    int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL,
                                            unaUbicacion);

                    if (filasAfectadas > 0)
                        resultadoAccion = true;
                }
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
                using (contextoDB.Conexion)
                {
                    string sentenciaSQL = "DELETE FROM ubicaciones WHERE id = @Id";

                    int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL,
                                            unaUbicacion);

                    if (filasAfectadas > 0)
                        resultadoAccion = true;
                }
            }
            catch (SqliteException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }
    }
}