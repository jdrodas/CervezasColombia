using CervezasColombia_CS_API_SQLite_Dapper.Cervezas;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Ubicaciones;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace CervezasColombia_CS_API_SQLite_Dapper.Cervecerias
{
    public class CerveceriaRepository(SQLiteDbContext unContexto) : ICerveceriaRepository
    {
        private readonly SQLiteDbContext contextoDB = unContexto;

        public async Task<IEnumerable<Cerveceria>> GetAllAsync()
        {
            string sentenciaSQL = "SELECT v.cerveceria_id id, v.cerveceria nombre, v.instagram " +
                "FROM v_info_cervecerias v ";

            var resultadoCervecerias = await contextoDB.Conexion
                .QueryAsync<Cerveceria>(sentenciaSQL, new DynamicParameters());

            foreach (Cerveceria unaCerveceria in resultadoCervecerias)
                unaCerveceria.Ubicacion = await GetBreweryLocation(unaCerveceria.Id);

            return resultadoCervecerias;
        }

        public async Task<Cerveceria> GetByAttributeAsync<T>(T atributo_valor, string atributo_nombre)
        {
            Cerveceria unaCerveceria = new();
            DynamicParameters parametrosSentencia = new();

            string sentenciaSQL = "SELECT v.cerveceria_id id, v.cerveceria nombre, v.instagram " +
                "FROM v_info_cervecerias v ";

            switch (atributo_nombre.ToLower())
            {
                case "id":
                    sentenciaSQL += "WHERE v.cerveceria_id = @cerveceria_id ";
                    parametrosSentencia.Add("@cerveceria_id", atributo_valor,
                        DbType.Int32, ParameterDirection.Input);
                    break;

                case "nombre":
                    sentenciaSQL += "WHERE LOWER(nombre) = LOWER(@cerveceria_nombre) ";
                    parametrosSentencia.Add("@cerveceria_nombre", atributo_valor,
                        DbType.String, ParameterDirection.Input);
                    break;

                case "instagram":
                    sentenciaSQL += "WHERE LOWER(instagram) = LOWER(@cerveceria_instagram) ";
                    parametrosSentencia.Add("@cerveceria_instagram", atributo_valor,
                        DbType.String, ParameterDirection.Input);
                    break;
            }

            var resultado = await contextoDB.Conexion
                .QueryAsync<Cerveceria>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
            {
                unaCerveceria = resultado.First();
                unaCerveceria.Ubicacion = await GetBreweryLocation(unaCerveceria.Id);
            }

            return unaCerveceria;
        }

        public async Task<int> GetTotalAssociatedBeersAsync(int cerveceria_id)
        {
            var lasCervezas = await GetAssociatedBeersAsync(cerveceria_id);
            return lasCervezas.ToList().Count;
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int cerveceria_id)
        {
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@cerveceria_id", cerveceria_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT cerveza_id id, cerveza nombre, cerveceria, estilo, abv, rango_abv " +
                                  "FROM v_info_cervezas " +
                                  "WHERE cerveceria_id = @cerveceria_id " +
                                  "ORDER BY id DESC";

            var resultadoCervezas = await contextoDB.Conexion
                .QueryAsync<Cerveza>(sentenciaSQL, parametrosSentencia);

            return resultadoCervezas;
        }

        public async Task<bool> CreateAsync(Cerveceria cerveceria)
        {
            bool resultadoAccion = false;

            try
            {
                DynamicParameters parametrosSentencia = new();
                parametrosSentencia.Add("@cerveceria_nombre", cerveceria.Nombre,
                                        DbType.String, ParameterDirection.Input);
                parametrosSentencia.Add("@cerveceria_instagram", cerveceria.Instagram,
                                        DbType.String, ParameterDirection.Input);
                parametrosSentencia.Add("@ubicacion_id", cerveceria.Ubicacion.Id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "INSERT INTO cervecerias (nombre, instagram, ubicacion_id) " +
                                      "VALUES (@cerveceria_nombre, @cerveceria_instagram, @ubicacion_id)";

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

        public async Task<bool> UpdateAsync(Cerveceria cerveceria)
        {
            bool resultadoAccion = false;

            try
            {
                DynamicParameters parametrosSentencia = new();
                parametrosSentencia.Add("@cerveceria_nombre", cerveceria.Nombre,
                                        DbType.String, ParameterDirection.Input);
                parametrosSentencia.Add("@cerveceria_instagram", cerveceria.Instagram,
                                        DbType.String, ParameterDirection.Input);
                parametrosSentencia.Add("@ubicacion_id", cerveceria.Ubicacion.Id,
                                        DbType.Int32, ParameterDirection.Input);
                parametrosSentencia.Add("@cerveceria_id", cerveceria.Id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "UPDATE cervecerias " +
                                      "SET nombre = @cerveceria_nombre, " +
                                      "instagram = @cerveceria_instagram, " +
                                      "ubicacion_id = @ubicacion_id " +
                                      "WHERE id = @cerveceria_id";

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

        public async Task<bool> DeleteAsync(Cerveceria cerveceria)
        {
            bool resultadoAccion = false;

            try
            {
                string sentenciaSQL = "DELETE FROM cervecerias WHERE id = @Id";

                int filasAfectadas = await contextoDB.Conexion
                    .ExecuteAsync(sentenciaSQL, cerveceria);

                if (filasAfectadas > 0)
                    resultadoAccion = true;
            }
            catch (SqliteException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<Ubicacion> GetBreweryLocation(int cerveceria_id)
        {
            Ubicacion unaUbicacion = new();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@cerveceria_id", cerveceria_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT u.id, u.municipio, u.departamento, u.latitud, u.longitud " +
                "FROM ubicaciones u JOIN cervecerias c ON c.ubicacion_id = u.id " +
                "WHERE c.id = @cerveceria_id";

            var resultadoIdUbicacion = await contextoDB.Conexion
                .QueryAsync<Ubicacion>(sentenciaSQL, parametrosSentencia);

            if (resultadoIdUbicacion.Any())
                unaUbicacion = resultadoIdUbicacion.First();

            return unaUbicacion;
        }

        public async Task<bool> DeleteAssociatedBeersAsync(int cerveceria_id)
        {
            bool resultadoAccion = false;

            try
            {
                DynamicParameters parametrosSentencia = new();
                parametrosSentencia.Add("@cerveceria_id", cerveceria_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "DELETE FROM cervezas WHERE cerveceria_id = @cerveceria_id";

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
    }
}
