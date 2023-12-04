using CervezasColombia_CS_API_SQLite_Dapper.Cervezas;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace CervezasColombia_CS_API_SQLite_Dapper.Ingredientes
{
    public class IngredienteRepository(SQLiteDbContext unContexto) : IIngredienteRepository
    {
        private readonly SQLiteDbContext contextoDB = unContexto;

        public async Task<IEnumerable<Ingrediente>> GetAllAsync()
        {
            string sentenciaSQL = "SELECT DISTINCT v.ingrediente_id id, v.ingrediente nombre, v.tipo_ingrediente, v.tipo_ingrediente_id " +
                                    "FROM v_info_ingredientes v " +
                                    "ORDER BY v.ingrediente_id DESC ";

            var resultadoEnvasados = await contextoDB.Conexion
                .QueryAsync<Ingrediente>(sentenciaSQL, new DynamicParameters());

            return resultadoEnvasados;
        }

        public async Task<Ingrediente> GetByIdAsync(int ingrediente_id)
        {
            Ingrediente unIngrediente = new();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ingrediente_id", ingrediente_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT v.ingrediente_id id, v.ingrediente nombre, v.tipo_ingrediente, v.tipo_ingrediente_id " +
                                    "FROM v_info_ingredientes v " +
                                    "WHERE v.ingrediente_id = @ingrediente_id ";

            var resultado = await contextoDB.Conexion
                .QueryAsync<Ingrediente>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unIngrediente = resultado.First();

            return unIngrediente;
        }

        public async Task<Ingrediente> GetByNameAndTypeAsync(string ingrediente_nombre, string ingrediente_tipo)
        {
            Ingrediente unIngrediente = new();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ingrediente_nombre", ingrediente_nombre,
                                    DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@ingrediente_tipo", ingrediente_tipo,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT v.ingrediente_id id, v.ingrediente nombre, v.tipo_ingrediente, v.tipo_ingrediente_id " +
                                    "FROM v_info_ingredientes v " +
                                    "WHERE LOWER(v.ingrediente) = LOWER(@ingrediente_nombre) " +
                                    "AND LOWER(v.tipo_ingrediente) = LOWER(@ingrediente_tipo)";

            var resultado = await contextoDB.Conexion
                .QueryAsync<Ingrediente>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unIngrediente = resultado.First();

            return unIngrediente;
        }

        public async Task<int> GetTotalAssociatedBeersAsync(int ingrediente_id)
        {
            var lasCervezas = await GetAssociatedBeersAsync(ingrediente_id);
            return lasCervezas.ToList().Count;
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int ingrediente_id)
        {
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ingrediente_id", ingrediente_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT vc.cerveza_id id, vc.cerveza nombre, vc.cerveceria, vc.cerveceria_id, " +
                                    "vc.estilo, vc.estilo_id, vc.abv, vc.rango_abv " +
                                    "FROM v_info_cervezas vc " +
                                    "JOIN v_info_ingredientes_cervezas vi ON vc.cerveza_id = vi.cerveza_id " +
                                    "WHERE vi.ingrediente_id = @ingrediente_id " +
                                    "ORDER BY vc.cerveza_id DESC ";

            var resultadoCervezas = await contextoDB.Conexion
                .QueryAsync<Cerveza>(sentenciaSQL, parametrosSentencia);

            return resultadoCervezas;
        }

        public async Task<Cerveza> GetAssociatedBeerByIdAsync(int ingrediente_id, int cerveza_id)
        {
            Cerveza cervezaExistente = new();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ingrediente_id", ingrediente_id,
                                    DbType.Int32, ParameterDirection.Input);
            parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT cerveza_id id, cerveza nombre, cerveceria, " +
                "estilo, abv " +
                "FROM v_info_cervezas " +
                "WHERE cerveza_id in (SELECT cerveza_id FROM ingredientes_cervezas " +
                "WHERE ingrediente_id = @ingrediente_id) AND cerveza_id = @cerveza_id";

            var resultado = await contextoDB.Conexion
                .QueryAsync<Cerveza>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                cervezaExistente = resultado.First();

            return cervezaExistente;
        }

        public async Task<int> GetAssociatedIngredientTypeIdAsync(string tipo_ingrediente_nombre)
        {
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@tipo_ingrediente", tipo_ingrediente_nombre,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id FROM tipos_ingredientes ti " +
                                    "WHERE LOWER(ti.nombre) = LOWER(@tipo_ingrediente) ";

            var resultadotipoIngrediente = await contextoDB.Conexion
                .QueryAsync<int>(sentenciaSQL, parametrosSentencia);

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
                string sentenciaSQL = "INSERT INTO ingredientes (nombre, tipo_ingrediente_id) " +
                                        "VALUES (@Nombre, @Tipo_Ingrediente_Id)";

                int filasAfectadas = await contextoDB.Conexion
                    .ExecuteAsync(sentenciaSQL, unIngrediente);

                if (filasAfectadas > 0)
                    resultadoAccion = true;
            }
            catch (SqliteException error)
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
                string sentenciaSQL = "UPDATE ingredientes SET nombre = @Nombre, tipo_ingrediente_id = @Tipo_Ingrediente_Id " +
                                      "WHERE id = @Id";

                int filasAfectadas = await contextoDB.Conexion
                    .ExecuteAsync(sentenciaSQL, unIngrediente);

                if (filasAfectadas > 0)
                    resultadoAccion = true;
            }
            catch (SqliteException error)
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
                string sentenciaSQL = "DELETE FROM ingredientes WHERE id = @Id";

                int filasAfectadas = await contextoDB.Conexion
                    .ExecuteAsync(sentenciaSQL, unIngrediente);

                if (filasAfectadas > 0)
                    resultadoAccion = true;
            }
            catch (SqliteException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<IEnumerable<IngredienteCerveza>> GetAssociatedBeerIngredientsAsync(int ingrediente_id)
        {
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ingrediente_id", ingrediente_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT cerveceria, cerveza, tipo_ingrediente, ingrediente " +
                "FROM v_info_ingredientes_cervezas " +
                "WHERE ingrediente_id = @ingrediente_id";

            var resultadoIngredientesCerveza = await contextoDB.Conexion
                .QueryAsync<IngredienteCerveza>(sentenciaSQL, parametrosSentencia);

            return resultadoIngredientesCerveza.ToList();
        }
    }
}
