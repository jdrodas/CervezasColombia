using CervezasColombia_CS_API_PostgreSQL_Dapper.DbContexts;
using CervezasColombia_CS_API_PostgreSQL_Dapper.Helpers;
using CervezasColombia_CS_API_PostgreSQL_Dapper.Interfaces;
using CervezasColombia_CS_API_PostgreSQL_Dapper.Models;
using Dapper;
using Npgsql;
using System.Data;

namespace CervezasColombia_CS_API_PostgreSQL_Dapper.Repositories
{
    public class CervezaRepository : ICervezaRepository
    {
        private readonly PgsqlDbContext contextoDB;

        public CervezaRepository(PgsqlDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Cerveza>> GetAllAsync()
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                string sentenciaSQL = "SELECT cerveza_id id, cerveza nombre, cerveceria_id, " +
                                        "cerveceria, estilo_id, estilo, ibu, abv, rango_ibu, rango_abv " +
                                        "FROM v_info_cervezas " +
                                        "ORDER BY id DESC";

                var resultadoCervezas = await conexion.QueryAsync<Cerveza>(sentenciaSQL,
                                            new DynamicParameters());

                return resultadoCervezas;
            }
        }

        public async Task<Cerveza> GetByIdAsync(int id)
        {
            Cerveza unaCerveza = new Cerveza();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveza_id", id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT cerveza_id id, cerveza nombre, cerveceria, cerveceria_id, " +
                                        "estilo, estilo_id, ibu, abv, rango_ibu, rango_abv " +
                                        "FROM v_info_cervezas " +
                                        "WHERE cerveza_id = @cerveza_id ";

                var resultado = await conexion.QueryAsync<Cerveza>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unaCerveza = resultado.First();
            }

            return unaCerveza;
        }

        public async Task<Cerveza> GetByNameAndBreweryAsync(string cerveza_nombre, string cerveceria)
        {
            Cerveza unaCerveza = new Cerveza();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveza", cerveza_nombre,
                                        DbType.String, ParameterDirection.Input);
                parametrosSentencia.Add("@cerveceria", cerveceria,
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "SELECT cerveza_id id, cerveza nombre, cerveceria, cerveceria_id, " +
                                        "estilo, estilo_id, ibu, abv, rango_ibu, rango_abv " +
                                        "FROM v_info_cervezas " +
                                        "WHERE LOWER(cerveza) = LOWER(@cerveza) " +
                                        "AND LOWER(cerveceria) = LOWER(@cerveceria) ";

                var resultado = await conexion.QueryAsync<Cerveza>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unaCerveza = resultado.First();
            }

            return unaCerveza;
        }

        public async Task<int> GetTotalAssociatedIngredientsAsync(int cerveza_id)
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT COUNT(ingrediente_id) totalIngredientes " +
                                      "FROM ingredientes_cervezas " +
                                      "WHERE cerveza_id = @cerveza_id";


                var totalIngredientes = await conexion.QueryFirstAsync<int>(sentenciaSQL,
                                        parametrosSentencia);

                return totalIngredientes;
            }
        }

        public async Task<IEnumerable<Ingrediente>> GetAssociatedIngredientsAsync(int cerveza_id)
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT DISTINCT v.ingrediente_id id, v.ingrediente nombre, v.tipo_ingrediente, v.tipo_ingrediente_id " +
                                      "FROM v_info_ingredientes_cervezas v " +
                                      "WHERE cerveza_id = @cerveza_id " +
                                      "ORDER BY tipo_ingrediente, nombre ";

                var resultadoIngredientes = await conexion.QueryAsync<Ingrediente>(sentenciaSQL, parametrosSentencia);

                return resultadoIngredientes;
            }
        }

        public async Task<int> GetTotalAssociatedPackagingsAsync(int cerveza_id)
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT COUNT(envasado_id) totalEnvasados " +
                                      "FROM envasados_cervezas " +
                                      "WHERE cerveza_id = @cerveza_id";


                var totalIngredientes = await conexion.QueryFirstAsync<int>(sentenciaSQL,
                                        parametrosSentencia);

                return totalIngredientes;
            }
        }

        public async Task<IEnumerable<EnvasadoCerveza>> GetAssociatedPackagingsAsync(int cerveza_id)
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT DISTINCT v.envasado_id id, v.envasado nombre, v.unidad_volumen_id, v.unidad_volumen, v.volumen " +
                                      "FROM v_info_envasados_cervezas v " +
                                      "WHERE cerveza_id = @cerveza_id " +
                                      "ORDER BY envasado, unidad_volumen, volumen ";

                var resultadoEnvasados = await conexion.QueryAsync<EnvasadoCerveza>(sentenciaSQL, parametrosSentencia);
                return resultadoEnvasados;
            }
        }

        public async Task<EnvasadoCerveza> GetPackagedBeerByNameAsync(int cerveza_id, string envasado_nombre, int unidad_volumen_id, float volumen)
        {
            EnvasadoCerveza unEnvasadoCerveza = new EnvasadoCerveza();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
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

                var resultado = await conexion.QueryAsync<EnvasadoCerveza>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unEnvasadoCerveza = resultado.First();
            }

            return unEnvasadoCerveza;
        }

        public async Task<bool> CreateAsync(Cerveza unaCerveza)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "core.p_inserta_cerveza";
                    var parametros = new
                    {
                        p_nombre = unaCerveza.Nombre,
                        p_cervceria_id = unaCerveza.Cerveceria_id,
                        p_estilo_id = unaCerveza.Estilo_id,
                        p_ibu = unaCerveza.Ibu,
                        p_abv = unaCerveza.Abv
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

        public async Task<bool> CreateBeerPackagingAsync(int cerveza_id, EnvasadoCerveza unEnvasadoCerveza)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "core.p_inserta_envasado_cerveza";
                    var parametros = new
                    {
                        p_cerveza_id = cerveza_id,
                        p_envasado_id = unEnvasadoCerveza.Id,
                        p_unidad_volumen_id = unEnvasadoCerveza.Unidad_Volumen_Id,
                        p_volumen = unEnvasadoCerveza.Volumen
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

        public async Task<bool> CreateBeerIngredientAsync(int cerveza_id, Ingrediente unIngrediente)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "core.p_inserta_ingrediente_cerveza";
                    var parametros = new
                    {
                        p_cerveza_id = cerveza_id,
                        p_ingrediente_id = unIngrediente.Id
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

        public async Task<bool> UpdateAsync(Cerveza unaCerveza)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "core.p_actualiza_cerveza";
                    var parametros = new
                    {
                        p_id = unaCerveza.Id,
                        p_nombre = unaCerveza.Nombre,
                        p_cervceria_id = unaCerveza.Cerveceria_id,
                        p_estilo_id = unaCerveza.Estilo_id,
                        p_ibu = unaCerveza.Ibu,
                        p_abv = unaCerveza.Abv
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

        public async Task<bool> UpdateBeerPackagingAsync(int cerveza_id, EnvasadoCerveza unEnvasadoCerveza)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "core.p_actualiza_envasado_cerveza";
                    var parametros = new
                    {
                        p_cerveza_id = cerveza_id,
                        p_envasado_id = unEnvasadoCerveza.Id,
                        p_unidad_volumen_id = unEnvasadoCerveza.Unidad_Volumen_Id,
                        p_volumen = unEnvasadoCerveza.Volumen
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

        //TODO: Hacer el método que invoca el procedimiento p_actualiza_ingrediente_cerveza
        public async Task<bool> DeleteAsync(Cerveza unaCerveza)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "core.p_elimina_cerveza";
                    var parametros = new
                    {
                        p_id = unaCerveza.Id
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

        public async Task<bool> DeleteBeerPackagingAsync(int cerveza_id, EnvasadoCerveza unEnvasadoCerveza)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "core.p_elimina_envasado_cerveza";
                    var parametros = new
                    {
                        p_cerveza_id = cerveza_id,
                        p_envasado_id = unEnvasadoCerveza.Id,
                        p_unidad_volumen_id = unEnvasadoCerveza.Unidad_Volumen_Id,
                        p_volumen = unEnvasadoCerveza.Volumen
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

        //TODO: Hacer el método que invoca el procedimiento p_elimina_ingrediente_cerveza
    }
}
