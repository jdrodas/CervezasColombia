using CervezasColombia_CS_API_SQLite_Dapper.DbContexts;
using CervezasColombia_CS_API_SQLite_Dapper.Exceptions;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;
using CervezasColombia_CS_API_SQLite_Dapper.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace CervezasColombia_CS_API_SQLite_Dapper.Repositories
{
    public class EnvasadoRepository(SQLiteDbContext unContexto) : IEnvasadoRepository
    {
        private readonly SQLiteDbContext contextoDB = unContexto;

        public async Task<IEnumerable<Envasado>> GetAllAsync()
        {
            string sentenciaSQL = "SELECT DISTINCT  e.id, e.nombre FROM envasados e " +
                                    "ORDER BY e.id DESC ";

            var resultadoEnvasados = await contextoDB.Conexion.QueryAsync<Envasado>(sentenciaSQL,
                                    new DynamicParameters());

            return resultadoEnvasados;
        }

        public async Task<Envasado> GetByIdAsync(int envasado_id)
        {
            Envasado unEnvasado = new();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@envasado_id", envasado_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT  e.id, e.nombre " +
                                    "FROM envasados e " +
                                    "WHERE e.id = @envasado_id ";

            var resultado = await contextoDB.Conexion.QueryAsync<Envasado>(sentenciaSQL,
                parametrosSentencia);

            if (resultado.Any())
                unEnvasado = resultado.First();

            return unEnvasado;
        }

        public async Task<Envasado> GetByNameAsync(string envasado_nombre)
        {
            Envasado unEnvasado = new();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@envasado_nombre", envasado_nombre,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre " +
                                  "FROM envasados " +
                                  "WHERE LOWER(nombre) = LOWER(@envasado_nombre) ";

            var resultado = await contextoDB.Conexion.QueryAsync<Envasado>(sentenciaSQL,
                                parametrosSentencia);

            if (resultado.Any())
                unEnvasado = resultado.First();

            return unEnvasado;
        }

        public async Task<int> GetTotalAssociatedBeersAsync(int envasado_id)
        {

            DynamicParameters parametrosSentencia = new();
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
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@envasado_id", envasado_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT vc.cerveza_id id, vc.cerveza nombre, vc.cerveceria, " +
                                    "vc.estilo, vc.abv, vc.rango_abv " +
                                    "FROM v_info_cervezas vc " +
                                    "JOIN v_info_envasados_cervezas ve ON vc.cerveza_id = ve.cerveza_id " +
                                    "WHERE ve.envasado_id = @envasado_id " +
                                    "ORDER BY vc.cerveza_id DESC";

            var resultadoCervezas = await contextoDB.Conexion.QueryAsync<Cerveza>(sentenciaSQL, parametrosSentencia);

            return resultadoCervezas;
        }

        public async Task<EnvasadoCerveza> GetAssociatedBeerPackagingAsync(int cerveza_id, int envasado_id, int unidad_volumen_id, float volumen)
        {
            EnvasadoCerveza unEnvasadoCerveza = new();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                    DbType.Int32, ParameterDirection.Input);
            parametrosSentencia.Add("@envasado_id", envasado_id,
                                    DbType.Int32, ParameterDirection.Input);
            parametrosSentencia.Add("@unidad_volumen_id", unidad_volumen_id,
                                    DbType.Int32, ParameterDirection.Input);
            parametrosSentencia.Add("@volumen", volumen,
                                    DbType.Single, ParameterDirection.Input);

            string sentenciaSQL = "SELECT v.envasado_id id, v.envasado nombre, v.unidad_volumen_id, unidad_volumen, volumen " +
                                    "FROM v_info_envasados_cervezas v " +
                                    "WHERE v.envasado_id = @envasado_id " +
                                    "AND v.cerveza_id = @cerveza_id " +
                                    "AND v.unidad_volumen_id = @unidad_volumen_id " +
                                    "AND v.volumen = @volumen";

            var resultado = await contextoDB.Conexion.QueryAsync<EnvasadoCerveza>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unEnvasadoCerveza = resultado.First();

            return unEnvasadoCerveza;
        }

        public async Task<bool> CreateAsync(Envasado unEnvasado)
        {
            bool resultadoAccion = false;

            try
            {
                string sentenciaSQL = "INSERT INTO envasados (nombre) " +
                                      "VALUES (@Nombre)";

                int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL,
                                        unEnvasado);

                if (filasAfectadas > 0)
                    resultadoAccion = true;
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
                string sentenciaSQL = "UPDATE envasados SET nombre = @Nombre " +
                                      "WHERE id = @Id";

                int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL,
                                        unEnvasado);

                if (filasAfectadas > 0)
                    resultadoAccion = true;
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
                string sentenciaSQL = "DELETE FROM envasados WHERE id = @Id";

                int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL,
                                        unEnvasado);

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
