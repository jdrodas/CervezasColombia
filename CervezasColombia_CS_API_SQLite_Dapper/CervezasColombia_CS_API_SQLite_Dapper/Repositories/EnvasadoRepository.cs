using CervezasColombia_CS_API_SQLite_Dapper.DbContexts;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;
using CervezasColombia_CS_API_SQLite_Dapper.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace CervezasColombia_CS_API_SQLite_Dapper.Repositories
{
    public class EnvasadoRepository : IEnvasadoRepository
    {
        private readonly SQLiteDbContext contextoDB;

        public EnvasadoRepository(SQLiteDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Envasado>> GetAllAsync()
        {
            using (contextoDB.Conexion)
            {
                string sentenciaSQL =
                    "SELECT DISTINCT  e.id, e.nombre FROM envasados e " +
                    "order by e.id DESC ";

                var resultadoEnvasados = await contextoDB.Conexion.QueryAsync<Envasado>(sentenciaSQL,
                                        new DynamicParameters());

                return resultadoEnvasados;
            }
        }

        public async Task<Envasado> GetByIdAsync(int envasado_id)
        {
            Envasado unEvasado = new Envasado();

            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@envasado_id", envasado_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL =
                    "SELECT DISTINCT  e.id, e.nombre " +
                    "FROM envasados e " +
                    "WHERE e.id = @envasado_id ";

                var resultado = await contextoDB.Conexion.QueryAsync<Envasado>(sentenciaSQL,
                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unEvasado = resultado.First();
            }

            return unEvasado;
        }

        public async Task<Envasado> GetByNameAsync(string envasado_nombre)
        {
            Envasado unEnvasado = new Envasado();

            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@envasado_nombre", envasado_nombre,
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "SELECT id, nombre " +
                                      "FROM envasados " +
                                      "WHERE LOWER(nombre) = LOWER(@envasado_nombre) ";

                var resultado = await contextoDB.Conexion.QueryAsync<Envasado>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unEnvasado = resultado.First();
            }

            return unEnvasado;
        }

        public async Task<int> GetTotalAssociatedBeersAsync(int envasado_id)
        {

            DynamicParameters parametrosSentencia = new DynamicParameters();
            parametrosSentencia.Add("@envasado_id", envasado_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT COUNT(cerveza_id) totalCervezas " +
                "FROM v_info_envasados_cervezas v " +
                "WHERE envasado_id = @envasado_id ";

            var totalCervezas = await contextoDB.Conexion.QueryFirstAsync<int>(sentenciaSQL,
                                    parametrosSentencia);

            return totalCervezas;
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int envasado_id)
        {
            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@envasado_id", envasado_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT vc.cerveza_id id, vc.cerveza nombre, vc.cerveceria, " +
                    "vc.estilo, vc.ibu, vc.abv, vc.rango_ibu, vc.rango_abv " +
                    "FROM v_info_cervezas vc " +
                    "JOIN v_info_envasados_cervezas ve ON vc.cerveza_id = ve.cerveza_id " +
                    "WHERE ve.envasado_id = @envasado_id " +
                    "ORDER BY vc.cerveza_id DESC";

                var resultadoCervezas = await contextoDB.Conexion.QueryAsync<Cerveza>(sentenciaSQL, parametrosSentencia);

                return resultadoCervezas;
            }
        }

        public async Task<bool> CreateAsync(Envasado unEnvasado)
        {
            bool resultadoAccion = false;

            try
            {
                using (contextoDB.Conexion)
                {
                    string sentenciaSQL = "INSERT INTO envasados (nombre) " +
                                              "VALUES (@Nombre)";

                    int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL,
                                            unEnvasado);

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

        public async Task<bool> UpdateAsync(Envasado unEnvasado)
        {
            bool resultadoAccion = false;

            try
            {
                using (contextoDB.Conexion)
                {
                    string sentenciaSQL = "UPDATE envasados SET nombre = @Nombre " +
                                              "WHERE id = @Id";

                    int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL,
                                            unEnvasado);

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


        public async Task<bool> DeleteAsync(Envasado unEnvasado)
        {
            bool resultadoAccion = false;

            try
            {
                using (contextoDB.Conexion)
                {
                    string sentenciaSQL = "DELETE FROM envasados WHERE id = @Id";

                    int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL,
                                            unEnvasado);

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
