using Dapper;
using System.Data;
using Microsoft.Data.Sqlite;
using CervezasColombia_CS_API_SQLite_Dapper.DbContexts;
using CervezasColombia_CS_API_SQLite_Dapper.Models;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;

namespace CervezasColombia_CS_API_SQLite_Dapper.Repositories
{
    public class CervezaRepository : ICervezaRepository
    {
        private readonly SQLiteDbContext contextoDB;

        public CervezaRepository(SQLiteDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Cerveza>> GetAllAsync()
        {
            using (contextoDB.Conexion)
            {
                string sentenciaSQL = "SELECT cerveza_id id, cerveza nombre, cerveceria_id, " +
                                    "cerveceria, estilo_id, estilo, ibu, abv, rango_ibu, rango_abv " +
                                    "FROM v_info_cervezas " +
                                    "ORDER BY id DESC";

                var resultadoCervezas = await contextoDB.Conexion.QueryAsync<Cerveza>(sentenciaSQL,
                                            new DynamicParameters());

                return resultadoCervezas;
            }
        }

        public async Task<Cerveza> GetByIdAsync(int id)
        {
            Cerveza unaCerveza = new Cerveza();

            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveza_id", id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT cerveza_id id, cerveza nombre, cerveceria, cerveceria_id, " +
                    "estilo, estilo_id, ibu, abv, rango_ibu, rango_abv " +
                    "FROM v_info_cervezas " +
                    "WHERE cerveza_id = @cerveza_id ";

                var resultado = await contextoDB.Conexion.QueryAsync<Cerveza>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.ToArray().Length > 0)
                    unaCerveza = resultado.First();
            }

            return unaCerveza;
        }

        public async Task<Cerveza> GetByNameAndBreweryAsync(string cerveza_nombre, string cerveceria)
        {
            Cerveza unaCerveza = new Cerveza();

            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveza", cerveza_nombre,
                                        DbType.String, ParameterDirection.Input);
                parametrosSentencia.Add("@cerveceria", cerveceria,
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "SELECT cerveza_id id, cerveza nombre, cerveceria, cerveceria_id, " +
                    "estilo, estilo_id, ibu, abv, rango_ibu, rango_abv " +
                    "FROM v_info_cervezas " +
                    "WHERE cerveza = @cerveza " +
                    "AND cerveceria = @cerveceria ";

                var resultado = await contextoDB.Conexion.QueryAsync<Cerveza>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.ToArray().Length > 0)
                    unaCerveza = resultado.First();
            }

            return unaCerveza;
        }

        public async Task<int> GetTotalAssociatedIngredientsAsync(int cerveza_id)
        {

            DynamicParameters parametrosSentencia = new DynamicParameters();
            parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT COUNT(ingrediente_id) totalIngredientes " +
                                  "FROM ingredientes_cervezas " +
                                  "WHERE cerveza_id = @cerveza_id";


            var totalIngredientes = await contextoDB.Conexion.QueryAsync<int>(sentenciaSQL,
                                    parametrosSentencia);

            return totalIngredientes.First();
        }

        public async Task<IEnumerable<Ingrediente>> GetAssociatedIngredientsAsync(int cerveza_id)
        {
            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT DISTINCT v.ingrediente_id id, v.ingrediente nombre, v.tipo_ingrediente " +
                      "FROM v_info_ingredientes_cervezas v " +
                      "WHERE cerveza_id = @cerveza_id " +
                      "ORDER BY tipo_ingrediente, nombre ";

                var resultadoIngredientes = await contextoDB.Conexion.QueryAsync<Ingrediente>(sentenciaSQL, parametrosSentencia);

                return resultadoIngredientes;
            }
        }

        public async Task<int> GetTotalAssociatedPackagingsAsync(int cerveza_id)
        {
            DynamicParameters parametrosSentencia = new DynamicParameters();
            parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT COUNT(envasado_id) totalEnvasados " +
                                  "FROM envasados_cervezas " +
                                  "WHERE cerveza_id = @cerveza_id";


            var totalIngredientes = await contextoDB.Conexion.QueryAsync<int>(sentenciaSQL,
                                    parametrosSentencia);

            return totalIngredientes.First();
        }

        public async Task<IEnumerable<Envasado>> GetAssociatedPackagingsAsync(int cerveza_id)
        {
            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT DISTINCT v.envasado_id id, v.envasado nombre, v.unidad_volumen, v.volumen " +
                      "FROM v_info_envasados_cervezas v " +
                      "WHERE cerveza_id = @cerveza_id " +
                      "ORDER BY envasado, unidad_volumen, volumen ";

                var resultadoEnvasados = await contextoDB.Conexion.QueryAsync<Envasado>(sentenciaSQL, parametrosSentencia);

                return resultadoEnvasados;
            }
        }

        public async Task<bool> CreateAsync(Cerveza unaCerveza)
        {
            bool resultadoAccion = false;

            try
            {
                using (contextoDB.Conexion)
                {
                    string sentenciaSQL = "INSERT INTO cervezas (nombre, cerveceria_id, estilo_id, ibu, abv) " +
                                              "VALUES (@Nombre, @Cerveceria_id, @Estilo_id, @Ibu, @Abv )";

                    int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL, unaCerveza);

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

        public async Task<bool> UpdateAsync(Cerveza unaCerveza)
        {
            bool resultadoAccion = false;

            try
            {
                using (contextoDB.Conexion)
                {
                    string sentenciaSQL = "UPDATE cervezas " +
                                                    "SET nombre = @Nombre, " +
                                                    "estilo_id = @Estilo_id, " +
                                                    "cerveceria_id = @Cerveceria_id, " +
                                                    "abv = @Abv, " +
                                                    "ibu = @Ibu " +
                                                    "WHERE id = @Id";

                    int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL, unaCerveza);

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

        public async Task<bool> DeleteAsync(Cerveza unaCerveza)
        {
            bool resultadoAccion = false;

            try
            {
                using (contextoDB.Conexion)
                {
                    string sentenciaSQL = "DELETE FROM cervezas WHERE id = @Id";

                    int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL, unaCerveza);

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
