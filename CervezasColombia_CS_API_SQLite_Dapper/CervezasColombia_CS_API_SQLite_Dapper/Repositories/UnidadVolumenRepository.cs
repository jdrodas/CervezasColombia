using CervezasColombia_CS_API_SQLite_Dapper.DbContexts;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;
using CervezasColombia_CS_API_SQLite_Dapper.Models;
using Dapper;
using System.Data;

namespace CervezasColombia_CS_API_SQLite_Dapper.Repositories
{
    public class UnidadVolumenRepository : IUnidadVolumenRepository
    {
        private readonly SQLiteDbContext contextoDB;

        public UnidadVolumenRepository(SQLiteDbContext unContexto)
        {
            contextoDB = unContexto;
        }



        public async Task<UnidadVolumen> GetByNameAsync(string unidad_volumen_nombre)
        {
            UnidadVolumen unaUnidadVolumen = new();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@nombre", unidad_volumen_nombre,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre, abreviatura " +
                                  "FROM unidades_volumen " +
                                  "WHERE nombre = @nombre ";

            var resultado = await contextoDB.Conexion.QueryAsync<UnidadVolumen>(sentenciaSQL,
                parametrosSentencia);

            if (resultado.Any())
                unaUnidadVolumen = resultado.First();

            return unaUnidadVolumen;
        }
    }
}