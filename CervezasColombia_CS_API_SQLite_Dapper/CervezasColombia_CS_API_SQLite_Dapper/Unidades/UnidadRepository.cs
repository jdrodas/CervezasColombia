using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using Dapper;
using System.Data;

namespace CervezasColombia_CS_API_SQLite_Dapper.Unidades
{
    public class UnidadRepository(SQLiteDbContext unContexto) : IUnidadRepository
    {
        private readonly SQLiteDbContext contextoDB = unContexto;

        public async Task<IEnumerable<Unidad>> GetAllAsync()
        {
            string sentenciaSQL = "SELECT id, nombre, abreviatura " +
                                  "FROM unidades ";

            var resultadoUnidades = await contextoDB.Conexion
                .QueryAsync<Unidad>(sentenciaSQL, new DynamicParameters());

            return resultadoUnidades;
        }

        public async Task<Unidad> GetByAttributeAsync<T>(T atributo_valor, string atributo_nombre)
        {
            Unidad unaUnidad = new();
            DynamicParameters parametrosSentencia = new();

            string sentenciaSQL = "SELECT id, nombre, abreviatura " +
                                  "FROM unidades ";

            switch (atributo_nombre.ToLower())
            {
                case "id":
                    sentenciaSQL += "WHERE id = @unidad_id ";
                    parametrosSentencia.Add("@unidad_id", atributo_valor,
                        DbType.Int32, ParameterDirection.Input);
                    break;

                case "nombre":
                    sentenciaSQL += "WHERE LOWER(nombre) = LOWER(@unidad_nombre) ";
                    parametrosSentencia.Add("@unidad_nombre", atributo_valor,
                        DbType.String, ParameterDirection.Input);
                    break;

                case "abreviatura":
                    sentenciaSQL += "WHERE LOWER(abreviatura) = LOWER(@unidad_abreviatura) ";
                    parametrosSentencia.Add("@unidad_abreviatura", atributo_valor,
                        DbType.String, ParameterDirection.Input);
                    break;

            }

            var resultado = await contextoDB.Conexion
                .QueryAsync<Unidad>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unaUnidad = resultado.First();

            return unaUnidad;
        }
    }
}