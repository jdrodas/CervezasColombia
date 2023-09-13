using Dapper;
using System.Data;
using Microsoft.Data.Sqlite;
using CervezasColombia_CS_API_SQLite_Dapper.DbContexts;
using CervezasColombia_CS_API_SQLite_Dapper.Models;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;

namespace CervezasColombia_CS_API_SQLite_Dapper.Repositories
{
    public class CerveceriaRepository : ICerveceriaRepository
    {
        private readonly SQLiteDbContext contextoDB;

        public CerveceriaRepository(SQLiteDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Cerveceria>> GetAllAsync()
        {
            using (contextoDB.Conexion)
            {
                string sentenciaSQL = "SELECT c.id, c.nombre, c.sitio_web, c.instagram, " +
                                      "(u.municipio || ', ' || u.departamento) ubicacion, c.ubicacion_id " +
                                      "FROM cervecerias c JOIN ubicaciones u " +
                                      "ON c.ubicacion_id = u.id " +
                                      "ORDER BY ubicacion, nombre";

                var resultadoEstilos = await contextoDB.Conexion.QueryAsync<Cerveceria>(sentenciaSQL,
                                        new DynamicParameters());

                return resultadoEstilos;
            }
        }

        public async Task<Cerveceria> GetByIdAsync(int id)
        {
            Cerveceria unaCerveceria = new Cerveceria();

            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveceria_id", id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT c.id, c.nombre, c.sitio_web, c.instagram, " +
                                      "(u.municipio || ', ' || u.departamento) ubicacion, c.ubicacion_id " +
                                      "FROM cervecerias c JOIN ubicaciones u ON c.ubicacion_id = u.id " +
                                      "WHERE c.id = @cerveceria_id ";                                      

                var resultado = await contextoDB.Conexion.QueryAsync<Cerveceria>(sentenciaSQL, 
                                    parametrosSentencia);

                if(resultado.ToArray().Length>0)
                    unaCerveceria = resultado.First();
            }

            return unaCerveceria;
        }

        public async Task<Cerveceria> GetByNameAsync(string nombre)
        {
            Cerveceria unaCerveceria = new Cerveceria();

            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveceria_nombre", nombre,
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "SELECT c.id, c.nombre, c.sitio_web, c.instagram, " +
                                      "(u.municipio || ', ' || u.departamento) ubicacion " +
                                      "FROM cervecerias c JOIN ubicaciones u ON c.ubicacion_id = u.id " +
                                      "WHERE LOWER(nombre) = LOWER(@cerveceria_nombre)";                                      

                var resultado = await contextoDB.Conexion.QueryAsync<Cerveceria>(sentenciaSQL, 
                                    parametrosSentencia);

                if(resultado.ToArray().Length>0)
                    unaCerveceria = resultado.First();
            }

            return unaCerveceria;
        }

        public async Task<Cerveceria> GetByInstagramAsync(string instagram)
        {
            Cerveceria unaCerveceria = new Cerveceria();

            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveceria_instagram", instagram,
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "SELECT c.id, c.nombre, c.sitio_web, c.instagram, " +
                                      "(u.municipio || ', ' || u.departamento) ubicacion " +
                                      "FROM cervecerias c JOIN ubicaciones u ON c.ubicacion_id = u.id " +
                                      "WHERE LOWER(instagram) = LOWER(@cerveceria_instagram) ";

                var resultado = await contextoDB.Conexion.QueryAsync<Cerveceria>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.ToArray().Length > 0)
                    unaCerveceria = resultado.First();
            }

            return unaCerveceria;
        }

        public async Task<Cerveceria> GetBySitioWebAsync(string sitio_web)
        {
            Cerveceria unaCerveceria = new Cerveceria();

            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveceria_sitio_web", sitio_web,
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "SELECT c.id, c.nombre, c.sitio_web, c.instagram, " +
                                      "(u.municipio || ', ' || u.departamento) ubicacion " +
                                      "FROM cervecerias c JOIN ubicaciones u ON c.ubicacion_id = u.id " +
                                      "WHERE LOWER(sitio_web) = LOWER(@cerveceria_sitio_web) ";

                var resultado = await contextoDB.Conexion.QueryAsync<Cerveceria>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.ToArray().Length > 0)
                    unaCerveceria = resultado.First();
            }

            return unaCerveceria;
        }

        public async Task<int> GetTotalAssociatedBeersAsync(int id)
        {

                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveceria_id", id,
                                        DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT COUNT(id) totalCervezas " +
                                  "FROM cervezas " +
                                  "WHERE cerveceria_id = @cerveceria_id " +
                                  "ORDER BY nombre";


            var totalCervezas = await contextoDB.Conexion.QueryAsync<int>(sentenciaSQL,
                                    parametrosSentencia);

            return totalCervezas.First();
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int id)
        {
            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveceria_id", id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT cerveza_id id, cerveza nombre, cerveceria, estilo, ibu, abv, rango_ibu, rango_abv " +
                      "FROM v_info_cervezas " +
                      "WHERE cerveceria_id = @cerveceria_id " +
                      "ORDER BY estilo, nombre";

                var resultadoCervezas = await contextoDB.Conexion.QueryAsync<Cerveza>(sentenciaSQL, parametrosSentencia);

                return resultadoCervezas;
            }
        }

        public async Task<int> GetAssociatedLocationIdAsync(string unaUbicacion)
        {
            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@ubicacion", unaUbicacion,
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "SELECT id FROM ubicaciones u " +
                      "WHERE (u.municipio || ', ' || u.departamento) = @ubicacion ";

                var resultadoIdUbicacion = await contextoDB.Conexion.QueryAsync<int>(sentenciaSQL, 
                                                parametrosSentencia);
                
                if (resultadoIdUbicacion.ToArray().Length > 0)
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
                using (contextoDB.Conexion) 
                {
                    string sentenciaSQL = "INSERT INTO cervecerias (nombre, sitio_web, instagram, ubicacion_id) " +
                                              "VALUES (@Nombre, @Sitio_Web, @Instagram, @Ubicacion_Id )";

                    int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL, unaCerveceria);

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

        public async Task<bool> UpdateAsync(Cerveceria unaCerveceria)
        {
            bool resultadoAccion = false;

            try
            {
                using (contextoDB.Conexion)
                {
                    string sentenciaSQL = "UPDATE cervecerias " +
                                                    "SET nombre = @Nombre, " +
                                                    "sitio_web = @Sitio_Web, " +
                                                    "instagram = @Instagram, " +
                                                    "ubicacion_id = @Ubicacion_Id " +
                                                    "WHERE id = @Id";

                    int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL, unaCerveceria);
                    
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

        public async Task<bool> DeleteAsync(Cerveceria unaCerveceria)
        {
            bool resultadoAccion = false;

            try
            {
                using (contextoDB.Conexion)
                {
                    string sentenciaSQL = "DELETE FROM cervecerias WHERE id = @Id";

                    int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL, unaCerveceria);

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
