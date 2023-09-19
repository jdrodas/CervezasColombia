using CervezasColombia_CS_API_SQLite_Dapper.DbContexts;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;
using CervezasColombia_CS_API_SQLite_Dapper.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace CervezasColombia_CS_API_SQLite_Dapper.Repositories
{
    public class EstiloRepository : IEstiloRepository
    {
        private readonly SQLiteDbContext contextoDB;

        public EstiloRepository(SQLiteDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Estilo>> GetAllAsync()
        {
            using (contextoDB.Conexion)
            {
                string sentenciaSQL = "SELECT id, nombre " +
                                      "FROM estilos " +
                                      "ORDER BY id DESC";

                var resultadoEstilos = await contextoDB.Conexion.QueryAsync<Estilo>(sentenciaSQL,
                                            new DynamicParameters());

                return resultadoEstilos;
            }
        }

        public async Task<Estilo> GetByIdAsync(int estilo_id)
        {
            Estilo unEstilo = new Estilo();

            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@estilo_id", estilo_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT id, nombre " +
                                      "FROM estilos " +
                                      "WHERE id = @estilo_id ";

                var resultado = await contextoDB.Conexion.QueryAsync<Estilo>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unEstilo = resultado.First();
            }

            return unEstilo;
        }

        public async Task<Estilo> GetByNameAsync(string estilo_nombre)
        {
            Estilo unEstilo = new Estilo();

            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@estilo_nombre", estilo_nombre,
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "SELECT id, nombre " +
                                      "FROM estilos " +
                                      "WHERE LOWER(nombre) = LOWER(@estilo_nombre) ";

                var resultado = await contextoDB.Conexion.QueryAsync<Estilo>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unEstilo = resultado.First();
            }

            return unEstilo;
        }

        public async Task<int> GetTotalAssociatedBeersAsync(int estilo_id)
        {

            DynamicParameters parametrosSentencia = new DynamicParameters();
            parametrosSentencia.Add("@estilo_id", estilo_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT COUNT(id) totalCervezas " +
                                  "FROM cervezas " +
                                  "WHERE estilo_id = @estilo_id " +
                                  "ORDER BY nombre";


            var totalCervezas = await contextoDB.Conexion.QueryFirstAsync<int>(sentenciaSQL,
                                    parametrosSentencia);

            return totalCervezas;
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int estilo_id)
        {
            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@estilo_id", estilo_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT cerveza_id id, cerveza nombre, cerveceria, estilo, " +
                    "ibu, abv, rango_ibu, rango_abv " +
                    "FROM v_info_cervezas " +
                    "WHERE estilo_id = @estilo_id " +
                    "ORDER BY cerveza_id DESC";

                var resultadoCervezas = await contextoDB.Conexion.QueryAsync<Cerveza>(sentenciaSQL,
                                            parametrosSentencia);

                return resultadoCervezas;
            }
        }

        public async Task<bool> CreateAsync(Estilo unEstilo)
        {
            bool resultadoAccion = false;

            try
            {
                using (contextoDB.Conexion)
                {
                    string sentenciaSQL = "INSERT INTO estilos (nombre) " +
                                              "VALUES (@Nombre)";

                    int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL,
                                            unEstilo);

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

        public async Task<bool> UpdateAsync(Estilo unEstilo)
        {
            bool resultadoAccion = false;

            try
            {
                using (contextoDB.Conexion)
                {
                    string sentenciaSQL = "UPDATE estilos SET nombre = @Nombre " +
                                              "WHERE id = @Id";

                    int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL,
                                            unEstilo);

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


        public async Task<bool> DeleteAsync(Estilo unEstilo)
        {
            bool resultadoAccion = false;

            try
            {
                using (contextoDB.Conexion)
                {
                    string sentenciaSQL = "DELETE FROM estilos WHERE id = @Id";

                    int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL,
                                            unEstilo);

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