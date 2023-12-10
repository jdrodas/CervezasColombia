using CervezasColombia_CS_API_SQLite_Dapper.Cervezas;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace CervezasColombia_CS_API_SQLite_Dapper.Estilos
{
    public class EstiloRepository(SQLiteDbContext unContexto) : IEstiloRepository
    {
        private readonly SQLiteDbContext contextoDB = unContexto;

        public async Task<IEnumerable<Estilo>> GetAllAsync()
        {
            string sentenciaSQL = "SELECT id, nombre " +
                                  "FROM estilos ";

            var resultadoEstilos = await contextoDB.Conexion
                .QueryAsync<Estilo>(sentenciaSQL, new DynamicParameters());

            return resultadoEstilos;
        }

        public async Task<Estilo> GetByAttributeAsync<T>(T atributo_valor, string atributo_nombre)
        {
            Estilo unEstilo = new();
            DynamicParameters parametrosSentencia = new();

            string sentenciaSQL = "SELECT id, nombre " +
                                  "FROM estilos ";

            switch (atributo_nombre.ToLower())
            {
                case "id":
                    sentenciaSQL += "WHERE id = @estilo_id ";
                    parametrosSentencia.Add("@estilo_id", atributo_valor,
                        DbType.Int32, ParameterDirection.Input);
                    break;

                case "nombre":
                    sentenciaSQL += "WHERE LOWER(nombre) = LOWER(@estilo_nombre) ";
                    parametrosSentencia.Add("@estilo_nombre", atributo_valor,
                        DbType.String, ParameterDirection.Input);
                    break;
            }

            var resultado = await contextoDB.Conexion
                .QueryAsync<Estilo>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unEstilo = resultado.First();

            return unEstilo;
        }

        public async Task<int> GetTotalAssociatedBeersAsync(int estilo_id)
        {
            var lasCervezas = await GetAssociatedBeersAsync(estilo_id);
            return lasCervezas.ToList().Count;
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int estilo_id)
        {
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@estilo_id", estilo_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT cerveza_id id, cerveza nombre, cerveceria, cerveceria_id, estilo, estilo_id, " +
                                    "abv, rango_abv " +
                                    "FROM v_info_cervezas " +
                                    "WHERE estilo_id = @estilo_id " +
                                    "ORDER BY cerveza_id DESC";

            var resultadoCervezas = await contextoDB.Conexion
                .QueryAsync<Cerveza>(sentenciaSQL, parametrosSentencia);

            return resultadoCervezas;
        }

        public async Task<bool> CreateAsync(Estilo estilo)
        {
            bool resultadoAccion = false;

            try
            {
                DynamicParameters parametrosSentencia = new();
                parametrosSentencia.Add("@estilo_nombre", estilo.Nombre,
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "INSERT INTO estilos (nombre) " +
                                        "VALUES (@estilo_nombre)";

                int filasAfectadas = await contextoDB.Conexion
                    .ExecuteAsync(sentenciaSQL, parametrosSentencia);

                if (filasAfectadas > 0)
                    resultadoAccion = true;
            }
            catch (SqliteException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Estilo estilo)
        {
            bool resultadoAccion = false;

            try
            {
                DynamicParameters parametrosSentencia = new();
                parametrosSentencia.Add("@estilo_id", estilo.Id,
                                        DbType.Int32, ParameterDirection.Input);
                parametrosSentencia.Add("@estilo_nombre", estilo.Nombre,
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "UPDATE estilos SET nombre = @estilo_nombre " +
                                        "WHERE id = @estilo_id";

                int filasAfectadas = await contextoDB.Conexion
                    .ExecuteAsync(sentenciaSQL, parametrosSentencia);

                if (filasAfectadas > 0)
                    resultadoAccion = true;
            }
            catch (SqliteException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> DeleteAsync(Estilo estilo)
        {
            bool resultadoAccion = false;

            try
            {
                string sentenciaSQL = "DELETE FROM estilos WHERE id = @Id";

                int filasAfectadas = await contextoDB.Conexion
                    .ExecuteAsync(sentenciaSQL, estilo);

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