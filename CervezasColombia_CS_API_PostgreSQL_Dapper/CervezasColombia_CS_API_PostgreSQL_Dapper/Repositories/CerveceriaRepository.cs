using CervezasColombia_CS_API_PostgreSQL_Dapper.DbContexts;
using CervezasColombia_CS_API_PostgreSQL_Dapper.Helpers;
using CervezasColombia_CS_API_PostgreSQL_Dapper.Interfaces;
using CervezasColombia_CS_API_PostgreSQL_Dapper.Models;
using Dapper;
using Npgsql;
using System.Data;

namespace CervezasColombia_CS_API_PostgreSQL_Dapper.Repositories
{
    public class CerveceriaRepository : ICerveceriaRepository
    {
        private readonly PgsqlDbContext contextoDB;

        public CerveceriaRepository(PgsqlDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Cerveceria>> GetAllAsync()
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                string sentenciaSQL = "SELECT v.cerveceria_id id, v.cerveceria nombre, v.sitio_web, v.instagram, v.ubicacion, v.ubicacion_id " +
                    "FROM v_info_cervecerias v " +
                    "ORDER BY v.cerveceria_id";

                var resultadoCervecerias = await conexion.QueryAsync<Cerveceria>(sentenciaSQL,
                                        new DynamicParameters());

                return resultadoCervecerias;
            }
        }

        public async Task<Cerveceria> GetByIdAsync(int cerveceria_id)
        {
            Cerveceria unaCerveceria = new Cerveceria();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveceria_id", cerveceria_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT v.cerveceria_id id, v.cerveceria nombre, v.sitio_web, v.instagram, v.ubicacion, v.ubicacion_id " +
                    "FROM v_info_cervecerias v " +
                    "WHERE v.cerveceria_id = @cerveceria_id";

                var resultado = await conexion.QueryAsync<Cerveceria>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unaCerveceria = resultado.First();
            }

            return unaCerveceria;
        }

        public async Task<Cerveceria> GetByNameAsync(string cerveceria_nombre)
        {
            Cerveceria unaCerveceria = new Cerveceria();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveceria_nombre", cerveceria_nombre,
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "SELECT c.id, c.nombre, c.sitio_web, c.instagram, " +
                                      "(u.municipio || ', ' || u.departamento) ubicacion, c.ubicacion_id " +
                                      "FROM cervecerias c JOIN ubicaciones u ON c.ubicacion_id = u.id " +
                                      "WHERE LOWER(nombre) = LOWER(@cerveceria_nombre)";

                var resultado = await conexion.QueryAsync<Cerveceria>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unaCerveceria = resultado.First();
            }

            return unaCerveceria;
        }

        public async Task<Cerveceria> GetByInstagramAsync(string cerveceria_instagram)
        {
            Cerveceria unaCerveceria = new Cerveceria();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveceria_instagram", cerveceria_instagram,
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "SELECT c.id, c.nombre, c.sitio_web, c.instagram, " +
                                      "(u.municipio || ', ' || u.departamento) ubicacion, c.ubicacion_id " +
                                      "FROM cervecerias c JOIN ubicaciones u ON c.ubicacion_id = u.id " +
                                      "WHERE LOWER(instagram) = LOWER(@cerveceria_instagram) ";

                var resultado = await conexion.QueryAsync<Cerveceria>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unaCerveceria = resultado.First();
            }

            return unaCerveceria;
        }

        public async Task<Cerveceria> GetBySitioWebAsync(string cerveceria_sitio_web)
        {
            Cerveceria unaCerveceria = new Cerveceria();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveceria_sitio_web", cerveceria_sitio_web,
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "SELECT c.id, c.nombre, c.sitio_web, c.instagram, " +
                                      "(u.municipio || ', ' || u.departamento) ubicacion, c.ubicacion_id " +
                                      "FROM cervecerias c JOIN ubicaciones u ON c.ubicacion_id = u.id " +
                                      "WHERE LOWER(sitio_web) = LOWER(@cerveceria_sitio_web) ";

                var resultado = await conexion.QueryAsync<Cerveceria>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unaCerveceria = resultado.First();
            }

            return unaCerveceria;
        }

        public async Task<int> GetTotalAssociatedBeersAsync(int cerveceria_id)
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveceria_id", cerveceria_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT COUNT(id) totalCervezas " +
                                      "FROM cervezas " +
                                      "WHERE cerveceria_id = @cerveceria_id ";


                var totalCervezas = await conexion.QueryFirstAsync<int>(sentenciaSQL,
                                        parametrosSentencia);

                return totalCervezas;
            }
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int cerveceria_id)
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveceria_id", cerveceria_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT cerveza_id id, cerveza nombre, cerveceria_id, cerveceria, estilo_id, " +
                                        "estilo, ibu, abv, rango_ibu, rango_abv " +
                                        "FROM v_info_cervezas " +
                                        "WHERE cerveceria_id = @cerveceria_id " +
                                        "ORDER BY id DESC";

                var resultadoCervezas = await conexion.QueryAsync<Cerveza>(sentenciaSQL, parametrosSentencia);

                return resultadoCervezas;
            }
        }

        public async Task<int> GetAssociatedLocationIdAsync(string ubicacion_nombre)
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@ubicacion", ubicacion_nombre,
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "SELECT id " +
                                        "FROM ubicaciones u " +
                                        "WHERE (LOWER(u.municipio) || ', ' || LOWER(u.departamento)) = LOWER(@ubicacion) ";

                var resultadoIdUbicacion = await conexion.QueryAsync<int>(sentenciaSQL,
                                                parametrosSentencia);

                if (resultadoIdUbicacion.Count() > 0)
                    return resultadoIdUbicacion.First();
                else
                    return 0;
            }
        }

        public async Task<bool> CreateAsync(Cerveceria unaCerveceria)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "core.p_inserta_cerveceria";
                    var parametros = new
                    {
                        p_nombre = unaCerveceria.Nombre,
                        p_ubicacion_id = unaCerveceria.Ubicacion_Id,
                        p_sitio_web = unaCerveceria.Sitio_Web,
                        p_instagram = unaCerveceria.Instagram
                    };

                    var cantidad_filas = await conexion.ExecuteAsync(
                        procedimiento,
                        parametros,
                        commandType: CommandType.StoredProcedure);

                    if (cantidad_filas != 0)
                        resultadoAccion = true;
                }
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Cerveceria unaCerveceria)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "core.p_actualiza_cerveceria";
                    var parametros = new
                    {
                        p_id = unaCerveceria.Id,
                        p_nombre = unaCerveceria.Nombre,
                        p_ubicacion_id = unaCerveceria.Ubicacion_Id,
                        p_sitio_web = unaCerveceria.Sitio_Web,
                        p_instagram = unaCerveceria.Instagram
                    };

                    var cantidad_filas = await conexion.ExecuteAsync(
                        procedimiento,
                        parametros,
                        commandType: CommandType.StoredProcedure);

                    if (cantidad_filas != 0)
                        resultadoAccion = true;
                }
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> DeleteAsync(Cerveceria unaCerveceria)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "core.p_elimina_cerveceria";
                    var parametros = new
                    {
                        p_id = unaCerveceria.Id
                    };

                    var cantidad_filas = await conexion.ExecuteAsync(
                        procedimiento,
                        parametros,
                        commandType: CommandType.StoredProcedure);

                    if (cantidad_filas != 0)
                        resultadoAccion = true;
                }
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }
    }
}
