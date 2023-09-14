using Dapper;
using System.Data;
using Microsoft.Data.Sqlite;
using CervezasColombia_CS_API_SQLite_Dapper.DbContexts;
using CervezasColombia_CS_API_SQLite_Dapper.Models;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;

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
                      "ORDER BY departamento, municipio";

                var resultadoUbicaciones = await contextoDB.Conexion.QueryAsync<Ubicacion>(sentenciaSQL,
                                            new DynamicParameters());

                return resultadoUbicaciones;
            }
        }

        public async Task<Ubicacion> GetByIdAsync(int id)
        {
            Ubicacion unaUbicacion = new Ubicacion();

            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@ubicacion_id", id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT id, municipio, departamento " +
                      "FROM ubicaciones " +
                      "WHERE id = @ubicacion_id ";

                var resultado = await contextoDB.Conexion.QueryAsync<Ubicacion>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.ToArray().Length > 0)
                    unaUbicacion = resultado.First();
            }

            return unaUbicacion;
        }

        public async Task<int> GetTotalAssociatedBreweriesAsync(int id)
        {

            DynamicParameters parametrosSentencia = new DynamicParameters();
            parametrosSentencia.Add("@ubicacion_id", id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT COUNT(id) total_cervecerias " +
                                  "FROM cervecerias " +
                                  "WHERE ubicacion_id = @ubicacion_id";


            var totalCervecerias = await contextoDB.Conexion.QueryAsync<int>(sentenciaSQL,
                                    parametrosSentencia);

            return totalCervecerias.First();
        }

        public async Task<IEnumerable<Cerveceria>> GetAssociatedBreweriesAsync(int id)
        {
            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@ubicacion_id", id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT c.id, c.nombre, c.sitio_web, c.instagram, " +
                      "(u.municipio || ', ' || u.departamento) ubicacion, c.ubicacion_id " +
                      "FROM cervecerias c JOIN ubicaciones u ON c.ubicacion_id = u.id " +
                      "WHERE c.ubicacion_id = @ubicacion_id ";

                var resultadoCervecerias = await contextoDB.Conexion.QueryAsync<Cerveceria>(sentenciaSQL, parametrosSentencia);

                return resultadoCervecerias;
            }
        }
    }
}