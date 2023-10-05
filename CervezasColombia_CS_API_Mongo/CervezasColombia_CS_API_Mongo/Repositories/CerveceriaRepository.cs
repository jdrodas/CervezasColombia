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
                string sentenciaSQL = "SELECT v.cerveceria_id id, v.cerveceria nombre, v.sitio_web, v.instagram " +
                    "FROM v_info_cervecerias v " +
                    "ORDER BY v.cerveceria_id";

                var resultadoCervecerias = await conexion.QueryAsync<Cerveceria>(sentenciaSQL,
                                        new DynamicParameters());

                foreach (Cerveceria unaCerveceria in resultadoCervecerias)
                    unaCerveceria.Ubicacion = await GetAssociatedLocationAsync(unaCerveceria.Id);

                return resultadoCervecerias;
            }
        }

        public async Task<Cerveceria> GetByIdAsync(int cerveceria_id)
        {
            Cerveceria unaCerveceria = new();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new();
                parametrosSentencia.Add("@cerveceria_id", cerveceria_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT v.cerveceria_id id, v.cerveceria nombre, v.sitio_web, v.instagram " +
                    "FROM v_info_cervecerias v " +
                    "WHERE v.cerveceria_id = @cerveceria_id";

                var resultado = await conexion.QueryAsync<Cerveceria>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Any())
                {
                    unaCerveceria = resultado.First();
                    unaCerveceria.Ubicacion = await GetAssociatedLocationAsync(unaCerveceria.Id);
                }                    
            }

            return unaCerveceria;
        }

        public async Task<CerveceriaDetallada> GetDetailsByIdAsync(int cerveceria_id)
        {
            CerveceriaDetallada unaCerveceriaDetallada = new();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new();
                parametrosSentencia.Add("@cerveceria_id", cerveceria_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT v.cerveceria_id id, v.cerveceria nombre, v.sitio_web, v.instagram " +
                    "FROM v_info_cervecerias v " +
                    "WHERE v.cerveceria_id = @cerveceria_id";

                var resultado = await conexion.QueryAsync<CerveceriaDetallada>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Any())
                {
                    unaCerveceriaDetallada = resultado.First();
                    unaCerveceriaDetallada.Ubicacion = await GetAssociatedLocationAsync(unaCerveceriaDetallada.Id);

                    //Aqui buscamos las cervezas asociadas
                    var cervezasAsociadas = await GetAssociatedBeersAsync(unaCerveceriaDetallada.Id);
                    unaCerveceriaDetallada.Cervezas = cervezasAsociadas.ToList();
                }
            }

            return unaCerveceriaDetallada;
        }

        public async Task<Cerveceria> GetByNameAsync(string cerveceria_nombre)
        {
            Cerveceria unaCerveceria = new();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new();
                parametrosSentencia.Add("@cerveceria_nombre", cerveceria_nombre,
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "SELECT c.id, c.nombre, c.sitio_web, c.instagram " +
                                      "FROM cervecerias c " +
                                      "WHERE LOWER(nombre) = LOWER(@cerveceria_nombre)";

                var resultado = await conexion.QueryAsync<Cerveceria>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Any())
                {
                    unaCerveceria = resultado.First();
                    unaCerveceria.Ubicacion = await GetAssociatedLocationAsync(unaCerveceria.Id);
                }
            }

            return unaCerveceria;
        }

        public async Task<Cerveceria> GetByInstagramAsync(string cerveceria_instagram)
        {
            Cerveceria unaCerveceria = new();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new();
                parametrosSentencia.Add("@cerveceria_instagram", cerveceria_instagram,
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "SELECT c.id, c.nombre, c.sitio_web, c.instagram " +
                                      "FROM cervecerias c  " +
                                      "WHERE LOWER(instagram) = LOWER(@cerveceria_instagram) ";

                var resultado = await conexion.QueryAsync<Cerveceria>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Any())
                {
                    unaCerveceria = resultado.First();
                    unaCerveceria.Ubicacion = await GetAssociatedLocationAsync(unaCerveceria.Id);
                }
            }

            return unaCerveceria;
        }

        public async Task<Cerveceria> GetBySitioWebAsync(string cerveceria_sitio_web)
        {
            Cerveceria unaCerveceria = new();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new();
                parametrosSentencia.Add("@cerveceria_sitio_web", cerveceria_sitio_web,
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "SELECT c.id, c.nombre, c.sitio_web, c.instagram " +
                                      "FROM cervecerias c " +
                                      "WHERE LOWER(sitio_web) = LOWER(@cerveceria_sitio_web) ";

                var resultado = await conexion.QueryAsync<Cerveceria>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Any())
                {
                    unaCerveceria = resultado.First();
                    unaCerveceria.Ubicacion = await GetAssociatedLocationAsync(unaCerveceria.Id);
                }
            }

            return unaCerveceria;
        }

        public async Task<int> GetTotalAssociatedBeersAsync(int cerveceria_id)
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new();
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
                DynamicParameters parametrosSentencia = new();
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

        public async Task<Ubicacion> GetAssociatedLocationAsync(int cerveceria_id)
        {
            Ubicacion unaUbicacion = new();
            
            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new();
                parametrosSentencia.Add("@cerveceria_id", cerveceria_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT u.id, u.municipio, u.departamento, u.latitud, u.longitud " +
                                        "FROM ubicaciones u " +
                                        "WHERE u.id = (select c.ubicacion_id from cervecerias c where c.id = @cerveceria_id) ";

                var resultadoIdUbicacion = await conexion.QueryAsync<Ubicacion>(sentenciaSQL,
                                                parametrosSentencia);

                if (resultadoIdUbicacion.Any())
                    return resultadoIdUbicacion.First();
            }

            return unaUbicacion;
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
                        p_ubicacion_id = unaCerveceria.Ubicacion.Id,
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
                        p_ubicacion_id = unaCerveceria.Ubicacion.Id,
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
