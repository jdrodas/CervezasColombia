using Dapper;
using System.Data;

namespace CervezasColombia_CS_API_SQLite_Dapper.Unidades
{
    public class UnidadVolumenRepository(SQLiteDbContext unContexto) : IUnidadVolumenRepository
    {
        private readonly SQLiteDbContext contextoDB = unContexto;

        public async Task<Unidad> GetByNameAsync(string unidad_nombre)
        {
            Unidad unaUnidadVolumen = new();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@nombre", unidad_nombre,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre, abreviatura " +
                                  "FROM unidades_volumen " +
                                  "WHERE nombre = @nombre ";

            var resultado = await contextoDB.Conexion.QueryAsync<Unidad>(sentenciaSQL,
                parametrosSentencia);

            if (resultado.Any())
                unaUnidadVolumen = resultado.First();

            return unaUnidadVolumen;
        }
    }
}