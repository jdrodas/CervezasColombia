using CervezasColombia_CS_API_PostgreSQL_Dapper.DbContexts;
using CervezasColombia_CS_API_PostgreSQL_Dapper.Helpers;
using CervezasColombia_CS_API_PostgreSQL_Dapper.Interfaces;
using CervezasColombia_CS_API_PostgreSQL_Dapper.Models;
using Dapper;
using Npgsql;
using System.Data;

namespace CervezasColombia_CS_API_PostgreSQL_Dapper.Repositories
{
    public class IngredienteRepository : IIngredienteRepository
    {
        private readonly PgsqlDbContext contextoDB;

        public IngredienteRepository(PgsqlDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Ingrediente>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT DISTINCT v.ingrediente_id id, v.ingrediente nombre, v.tipo_ingrediente, v.tipo_ingrediente_id " +
                        "FROM v_info_ingredientes v " +
                        "ORDER BY v.ingrediente_id DESC ";

            var resultadoEnvasados = await conexion.QueryAsync<Ingrediente>(sentenciaSQL,
                                    new DynamicParameters());

            return resultadoEnvasados;
        }

        public async Task<Ingrediente> GetByIdAsync(int ingrediente_id)
        {
            Ingrediente unIngrediente = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ingrediente_id", ingrediente_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT v.ingrediente_id id, v.ingrediente nombre, v.tipo_ingrediente, v.tipo_ingrediente_id " +
                                    "FROM v_info_ingredientes v " +
                                    "WHERE v.ingrediente_id = @ingrediente_id ";

            var resultado = await conexion.QueryAsync<Ingrediente>(sentenciaSQL,
                parametrosSentencia);

            if (resultado.Any())
                unIngrediente = resultado.First();

            return unIngrediente;
        }

        public async Task<Ingrediente> GetByNameAndTypeAsync(string ingrediente_nombre, string ingrediente_tipo)
        {
            Ingrediente unIngrediente = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ingrediente_nombre", ingrediente_nombre,
                                    DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@ingrediente_tipo", ingrediente_tipo,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT v.ingrediente_id id, v.ingrediente nombre, v.tipo_ingrediente, v.tipo_ingrediente_id " +
                                    "FROM v_info_ingredientes v " +
                                    "WHERE LOWER(v.ingrediente) = LOWER(@ingrediente_nombre) " +
                                    "AND LOWER(v.tipo_ingrediente) = LOWER(@ingrediente_tipo)";

            var resultado = await conexion.QueryAsync<Ingrediente>(sentenciaSQL,
                                parametrosSentencia);

            if (resultado.Any())
                unIngrediente = resultado.First();

            return unIngrediente;
        }

        public async Task<int> GetTotalAssociatedBeersAsync(int ingrediente_id)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ingrediente_id", ingrediente_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT COUNT(v.cerveza_id) totalCervezas " +
                                    "FROM v_info_ingredientes_cervezas v " +
                                    "WHERE v.ingrediente_id = @ingrediente_id ";

            var totalCervezas = await conexion.QueryFirstAsync<int>(sentenciaSQL,
                                    parametrosSentencia);

            return totalCervezas;
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int ingrediente_id)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ingrediente_id", ingrediente_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT vc.cerveza_id id, vc.cerveza nombre, vc.cerveceria, vc.cerveceria_id, " +
                                    "vc.estilo, vc.estilo_id, vc.ibu, vc.abv, vc.rango_ibu, vc.rango_abv " +
                                    "FROM v_info_cervezas vc " +
                                    "JOIN v_info_ingredientes_cervezas vi ON vc.cerveza_id = vi.cerveza_id " +
                                    "WHERE vi.ingrediente_id = @ingrediente_id " +
                                    "ORDER BY vc.cerveza_id DESC ";

            var resultadoCervezas = await conexion.QueryAsync<Cerveza>(sentenciaSQL, parametrosSentencia);

            return resultadoCervezas;
        }

        public async Task<Cerveza> GetAssociatedBeerByIdAsync(int ingrediente_id, int cerveza_id)
        {
            Cerveza cervezaExistente = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ingrediente_id", ingrediente_id,
                                    DbType.Int32, ParameterDirection.Input);
            parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT cerveza_id id, cerveza nombre, cerveceria, " +
                "estilo, ibu, abv " +
                "FROM v_info_cervezas " +
                "WHERE cerveza_id in (SELECT cerveza_id FROM ingredientes_cervezas " +
                "WHERE ingrediente_id = @ingrediente_id) AND cerveza_id = @cerveza_id";

            var resultado = await conexion.QueryAsync<Cerveza>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                cervezaExistente = resultado.First();

            return cervezaExistente;
        }


        public async Task<int> GetAssociatedIngredientTypeIdAsync(string tipo_ingrediente_nombre)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@tipo_ingrediente", tipo_ingrediente_nombre,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id FROM tipos_ingredientes ti " +
                                    "WHERE LOWER(ti.nombre) = LOWER(@tipo_ingrediente) ";

            var resultadotipoIngrediente = await conexion.QueryAsync<int>(sentenciaSQL,
                                            parametrosSentencia);

            if (resultadotipoIngrediente.Any())
                return resultadotipoIngrediente.First();
            else
                return 0;
        }


        public async Task<bool> CreateAsync(Ingrediente unIngrediente)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_inserta_ingrediente";
                var parametros = new
                {
                    p_nombre = unIngrediente.Nombre,
                    p_tipo_ingrediente_id = unIngrediente.Tipo_Ingrediente_Id
                };

                var cantidad_filas = await conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
                    resultadoAccion = true;
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Ingrediente unIngrediente)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_actualiza_ingrediente";
                var parametros = new
                {
                    p_id = unIngrediente.Id,
                    p_nombre = unIngrediente.Nombre,
                    p_tipo_ingrediente_id = unIngrediente.Tipo_Ingrediente_Id
                };

                var cantidad_filas = await conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
                    resultadoAccion = true;


            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> DeleteAsync(Ingrediente unIngrediente)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_elimina_ingrediente";
                var parametros = new
                {
                    p_id = unIngrediente.Id
                };

                var cantidad_filas = await conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
                    resultadoAccion = true;
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }
    }
}
