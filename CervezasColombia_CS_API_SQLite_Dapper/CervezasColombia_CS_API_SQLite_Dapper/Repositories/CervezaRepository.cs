using CervezasColombia_CS_API_SQLite_Dapper.DbContexts;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;
using CervezasColombia_CS_API_SQLite_Dapper.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

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

        public async Task<Cerveza> GetByIdAsync(int cerveza_id)
        {
            Cerveza unaCerveza = new();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT cerveza_id id, cerveza nombre, cerveceria, cerveceria_id, " +
                                    "estilo, estilo_id, ibu, abv, rango_ibu, rango_abv " +
                                    "FROM v_info_cervezas " +
                                    "WHERE cerveza_id = @cerveza_id ";

            var resultado = await contextoDB.Conexion.QueryAsync<Cerveza>(sentenciaSQL,
                                parametrosSentencia);

            if (resultado.Any())
                unaCerveza = resultado.First();

            return unaCerveza;
        }

        public async Task<CervezaDetallada> GetDetailsByIdAsync(int cerveza_id)
        {
            CervezaDetallada unaCervezaDetallada = new();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT cerveza_id id, cerveza nombre, cerveceria, cerveceria_id, " +
                                    "estilo, estilo_id, ibu, abv, rango_ibu, rango_abv " +
                                    "FROM v_info_cervezas " +
                                    "WHERE cerveza_id = @cerveza_id ";

            var resultado = await contextoDB.Conexion.QueryAsync<CervezaDetallada>(sentenciaSQL,
                                parametrosSentencia);

            if (resultado.Any())
            {
                unaCervezaDetallada = resultado.First();

                var envasadosCerveza = await GetAssociatedPackagingsAsync(unaCervezaDetallada.Id);
                unaCervezaDetallada.Envasados = envasadosCerveza.ToList();

                var ingredientesCerveza = await GetAssociatedIngredientsAsync(unaCervezaDetallada.Id);
                unaCervezaDetallada.Ingredientes = ingredientesCerveza.ToList();
            }

            return unaCervezaDetallada;
        }

        public async Task<Cerveza> GetByNameAndBreweryAsync(string cerveza_nombre, string cerveceria)
        {
            Cerveza unaCerveza = new();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@cerveza", cerveza_nombre,
                                    DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@cerveceria", cerveceria,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL = "SELECT cerveza_id id, cerveza nombre, cerveceria, cerveceria_id, " +
                                    "estilo, estilo_id, ibu, abv, rango_ibu, rango_abv " +
                                    "FROM v_info_cervezas " +
                                    "WHERE LOWER(cerveza) = LOWER(@cerveza) " +
                                    "AND LOWER(cerveceria) = LOWER(@cerveceria) ";

            var resultado = await contextoDB.Conexion.QueryAsync<Cerveza>(sentenciaSQL,
                                parametrosSentencia);

            if (resultado.Any())
                unaCerveza = resultado.First();

            return unaCerveza;
        }

        public async Task<int> GetTotalAssociatedIngredientsAsync(int cerveza_id)
        {

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT COUNT(ingrediente_id) totalIngredientes " +
                                  "FROM ingredientes_cervezas " +
                                  "WHERE cerveza_id = @cerveza_id";


            var totalIngredientes = await contextoDB.Conexion.QueryFirstAsync<int>(sentenciaSQL,
                                    parametrosSentencia);

            return totalIngredientes;
        }

        public async Task<IEnumerable<Ingrediente>> GetAssociatedIngredientsAsync(int cerveza_id)
        {
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT v.ingrediente_id id, v.ingrediente nombre, v.tipo_ingrediente, v.tipo_ingrediente_id  " +
                                    "FROM v_info_ingredientes_cervezas v " +
                                    "WHERE cerveza_id = @cerveza_id " +
                                    "ORDER BY tipo_ingrediente, nombre ";

            var resultadoIngredientes = await contextoDB.Conexion.QueryAsync<Ingrediente>(sentenciaSQL, parametrosSentencia);

            return resultadoIngredientes;
        }

        public async Task<int> GetTotalAssociatedPackagingsAsync(int cerveza_id)
        {
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT COUNT(envasado_id) totalEnvasados " +
                                  "FROM envasados_cervezas " +
                                  "WHERE cerveza_id = @cerveza_id";


            var totalIngredientes = await contextoDB.Conexion.QueryFirstAsync<int>(sentenciaSQL,
                                    parametrosSentencia);

            return totalIngredientes;
        }

        public async Task<IEnumerable<EnvasadoCerveza>> GetAssociatedPackagingsAsync(int cerveza_id)
        {
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT v.envasado_id id, v.envasado nombre, v.unidad_volumen_id, v.unidad_volumen, v.volumen " +
                                    "FROM v_info_envasados_cervezas v " +
                                    "WHERE cerveza_id = @cerveza_id " +
                                    "ORDER BY envasado, unidad_volumen, volumen ";

            var resultadoEnvasados = await contextoDB.Conexion.QueryAsync<EnvasadoCerveza>(sentenciaSQL, parametrosSentencia);
            return resultadoEnvasados;
        }

        public async Task<EnvasadoCerveza> GetPackagedBeerByNameAsync(int cerveza_id, string envasado_nombre, int unidad_volumen_id, float volumen)
        {
            EnvasadoCerveza unEnvasadoCerveza = new();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                    DbType.Int32, ParameterDirection.Input);
            parametrosSentencia.Add("@envasado_nombre", envasado_nombre,
                                    DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@unidad_volumen_id", unidad_volumen_id,
                                    DbType.Int32, ParameterDirection.Input);
            parametrosSentencia.Add("@volumen", volumen,
                                    DbType.Single, ParameterDirection.Input);

            string sentenciaSQL = "SELECT v.envasado_id id, v.envasado nombre, v.unidad_volumen_id, unidad_volumen, volumen " +
                    "FROM v_info_envasados_cervezas v " +
                    "WHERE LOWER(envasado) = LOWER(@envasado_nombre) " +
                    "AND v.cerveza_id = @cerveza_id " +
                    "AND v.unidad_volumen_id = @unidad_volumen_id " +
                    "AND v.volumen = @volumen";

            var resultado = await contextoDB.Conexion.QueryAsync<EnvasadoCerveza>(sentenciaSQL,
                                parametrosSentencia);

            if (resultado.Any())
                unEnvasadoCerveza = resultado.First();

            return unEnvasadoCerveza;
        }

        public async Task<bool> CreateAsync(Cerveza unaCerveza)
        {
            bool resultadoAccion = false;

            try
            {
                string sentenciaSQL = "INSERT INTO cervezas (nombre, cerveceria_id, estilo_id, ibu, abv) " +
                                "VALUES (@Nombre, @Cerveceria_id, @Estilo_id, @Ibu, @Abv )";

                int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL, unaCerveza);

                if (filasAfectadas > 0)
                {
                    //Obtenemos el ID asignado luego de la inserción
                    var cervezaInsertada = await GetByNameAndBreweryAsync(unaCerveza.Nombre!, unaCerveza.Cerveceria!);

                    await CreateDefaultIngredient(cervezaInsertada.Id);
                    await CreateDefaultPackaging(cervezaInsertada.Id);

                    resultadoAccion = true;
                }
            }
            catch (SqliteException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> CreateDefaultIngredient(int cerveza_id)
        {
            bool resultadoAccion = false;
            try
            {
                DynamicParameters parametrosSentencia = new();
                parametrosSentencia.Add("@ingrediente_nombre", "Agua de Manantial",
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "SELECT ingrediente_id " +
                                      "FROM v_info_ingredientes " +
                                      "WHERE LOWER(ingrediente) = LOWER(@ingrediente_nombre)";

                int ingrediente_id = await contextoDB.Conexion.QueryFirstAsync<int>(sentenciaSQL,
                                        parametrosSentencia);

                parametrosSentencia = new();
                parametrosSentencia.Add("@ingrediente_id", ingrediente_id,
                                        DbType.Int32, ParameterDirection.Input);

                parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                        DbType.Int32, ParameterDirection.Input);

                sentenciaSQL = "INSERT INTO ingredientes_cervezas (cerveza_id, ingrediente_id) " +
                                "VALUES (@cerveza_id, @ingrediente_id)";

                int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL, parametrosSentencia);

                if (filasAfectadas > 0)
                    resultadoAccion = true;

                return resultadoAccion;
            }
            catch (SqliteException error)
            {
                throw new DbOperationException(error.Message);
            }
        }

        public async Task<bool> CreateDefaultPackaging(int cerveza_id)
        {
            bool resultadoAccion = false;
            try
            {
                DynamicParameters parametrosSentencia = new();
                parametrosSentencia.Add("@envasado_nombre", "Botella",
                                        DbType.String, ParameterDirection.Input);
                parametrosSentencia.Add("@unidad_volumen", "Mililitros",
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "SELECT e.id, uv.id unidad_volumen_id, 330 volumen " +
                                      "FROM envasados e, unidades_volumen uv " +
                                      "WHERE LOWER(e.nombre) = LOWER(@envasado_nombre) " +
                                      "AND LOWER(uv.nombre) = LOWER(@unidad_volumen)";

                var envasado_predeterminado = await contextoDB.Conexion.QueryFirstAsync<EnvasadoCerveza>(sentenciaSQL,
                                                                parametrosSentencia);

                parametrosSentencia.Add("@unidad_volumen_id", envasado_predeterminado.Unidad_Volumen_Id,
                                        DbType.Int32, ParameterDirection.Input);

                parametrosSentencia.Add("@volumen", envasado_predeterminado.Volumen,
                                        DbType.Int32, ParameterDirection.Input);

                parametrosSentencia.Add("@envasado_id", envasado_predeterminado.Id,
                                        DbType.Int32, ParameterDirection.Input);

                parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                        DbType.Int32, ParameterDirection.Input);

                sentenciaSQL = "INSERT INTO envasados_cervezas (cerveza_id, envasado_id, unidad_volumen_id, volumen) " +
                                "VALUES (@cerveza_id, @envasado_id, @unidad_volumen_id, @volumen)";

                int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL, parametrosSentencia);

                if (filasAfectadas > 0)
                    resultadoAccion = true;

                return resultadoAccion;
            }
            catch (SqliteException error)
            {
                throw new DbOperationException(error.Message);
            }
        }

        public async Task<bool> CreateBeerPackagingAsync(int cerveza_id, EnvasadoCerveza unEnvasadoCerveza)
        {
            bool resultadoAccion = false;

            try
            {
                string procedimiento = "core.p_inserta_envasado_cerveza";
                var parametros = new
                {
                    p_cerveza_id = cerveza_id,
                    p_envasado_id = unEnvasadoCerveza.Id,
                    p_unidad_volumen_id = unEnvasadoCerveza.Unidad_Volumen_Id,
                    p_volumen = unEnvasadoCerveza.Volumen
                };

                var cantidad_filas = await contextoDB.Conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
                    resultadoAccion = true;
            }
            catch (SqliteException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> CreateBeerIngredientAsync(int cerveza_id, Ingrediente unIngrediente)
        {
            bool resultadoAccion = false;

            try
            {
                string procedimiento = "core.p_inserta_ingrediente_cerveza";
                var parametros = new
                {
                    p_cerveza_id = cerveza_id,
                    p_ingrediente_id = unIngrediente.Id
                };

                var cantidad_filas = await contextoDB.Conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
                    resultadoAccion = true;
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
                string sentenciaSQL = "DELETE FROM cervezas WHERE id = @Id";

                int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL, unaCerveza);

                if (filasAfectadas > 0)
                    resultadoAccion = true;
            }
            catch (SqliteException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> DeleteBeerPackagingAsync(int cerveza_id, EnvasadoCerveza unEnvasadoCerveza)
        {
            bool resultadoAccion = false;

            try
            {
                string procedimiento = "core.p_elimina_envasado_cerveza";
                var parametros = new
                {
                    p_cerveza_id = cerveza_id,
                    p_envasado_id = unEnvasadoCerveza.Id,
                    p_unidad_volumen_id = unEnvasadoCerveza.Unidad_Volumen_Id,
                    p_volumen = unEnvasadoCerveza.Volumen
                };

                var cantidad_filas = await contextoDB.Conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
                    resultadoAccion = true;
            }
            catch (SqliteException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> DeleteBeerIngredientAsync(int cerveza_id, Ingrediente unIngrediente)
        {
            bool resultadoAccion = false;

            try
            {
                string procedimiento = "core.p_elimina_ingrediente_cerveza";
                var parametros = new
                {
                    p_cerveza_id = cerveza_id,
                    p_ingrediente_id = unIngrediente.Id
                };

                var cantidad_filas = await contextoDB.Conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
                    resultadoAccion = true;
            }
            catch (SqliteException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        Task<IEnumerable<IngredienteCerveza>> ICervezaRepository.GetAssociatedIngredientsAsync(int cerveza_id)
        {
            throw new NotImplementedException();
        }

        Task<EnvasadoCerveza> ICervezaRepository.GetPackagedBeerByNameAsync(int cerveza_id, string envasado_nombre, string unidad_volumen, double volumen)
        {
            throw new NotImplementedException();
        }

        Task<string> ICervezaRepository.GetAbvRangeNameAsync(double abv)
        {
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@cerveza_abv", abv,
                                    DbType.Double, ParameterDirection.Input);

            string sentenciaSQL = "SELECT nombre " +
                                    "FROM rangos_abv " +
                                    "WHERE @cerveza_abv BETWEEN valor_inicial AND valor_final";

            var resultado = await contextoDB.Conexion.QueryAsync<string>(sentenciaSQL, parametrosSentencia);


            return resultadoEnvasados;
        }

        Task<string> ICervezaRepository.GetIbuRangeNameAsync(double ibu)
        {
            throw new NotImplementedException();
        }

        Task<bool> ICervezaRepository.CreateBeerPackagingAsync(EnvasadoCerveza unEnvasadoCerveza)
        {
            throw new NotImplementedException();
        }
    }
}
