using CervezasColombia_CS_API_SQLite_Dapper.Data.DbContexts;
using CervezasColombia_CS_API_SQLite_Dapper.Data.Entities;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;
using Dapper;
using System.Data;

namespace CervezasColombia_CS_API_SQLite_Dapper.Repositories
{
    public class EstiloRepository : IEstiloRepository
    {
        private readonly SQLiteDbContext contextoDB;

        public EstiloRepository(SQLiteDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<List<Estilo>> GetAllEstilosAsync()
        {
            using (contextoDB.Conexion)
            {
                string sentenciaSQL = "SELECT id, nombre " +
                                      "FROM estilos " +
                                      "ORDER BY nombre";

                var resultadoEstilos = await contextoDB.Conexion.QueryAsync<Estilo>(sentenciaSQL, 
                                            new DynamicParameters());

                return resultadoEstilos.AsList();
            }
        }

        public async Task<Estilo> GetEstiloByIdAsync(int id)
        {
            Estilo unEstilo = new Estilo();

            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@estilo_id", id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT id, nombre " +
                                      "FROM estilos " +
                                      "WHERE id = @estilo_id " +
                                      "ORDER BY nombre";

                var resultado = await contextoDB.Conexion.QueryAsync<Estilo>(sentenciaSQL, 
                                    parametrosSentencia);

                if (resultado.ToArray().Length > 0)
                    unEstilo = resultado.First();
            }

            return unEstilo;
        }
    }
}
