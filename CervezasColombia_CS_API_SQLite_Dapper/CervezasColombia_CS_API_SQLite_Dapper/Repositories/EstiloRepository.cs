using CervezasColombia_CS_API_SQLite_Dapper.Data.DbContexts;
using CervezasColombia_CS_API_SQLite_Dapper.Data.Entities;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;
using Dapper;
using System.Data;
using Microsoft.Data.Sqlite;

namespace CervezasColombia_CS_API_SQLite_Dapper.Repositories
{
    public class EstiloRepository : IEstiloRepository
    {
        private readonly SQLiteDbContext contextoDB;

        public EstiloRepository(SQLiteDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Estilo>> GetAllAsync()
        {
            using (contextoDB.Conexion)
            {
                string sentenciaSQL = "SELECT id, nombre " +
                                      "FROM estilos " +
                                      "ORDER BY id DESC";

                var resultadoEstilos = await contextoDB.Conexion.QueryAsync<Estilo>(sentenciaSQL, 
                                            new DynamicParameters());

                return resultadoEstilos;
            }
        }

        public async Task<Estilo> GetByIdAsync(int id)
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

                if(resultado.ToArray().Length>0)
                    unEstilo = resultado.First();
            }

            return unEstilo;
        }

        public async Task<Estilo> GetByNameAsync(string nombre)
        {
            Estilo unEstilo = new Estilo();

            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@estilo_nombre", nombre,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT id, nombre " +
                                      "FROM estilos " +
                                      "WHERE LOWER(nombre) = LOWER(@estilo_nombre) " +
                                      "ORDER BY nombre";

                var resultado = await contextoDB.Conexion.QueryAsync<Estilo>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.ToArray().Length > 0)
                    unEstilo = resultado.First();
            }

            return unEstilo;
        }

        public async Task CreateAsync(Estilo unEstilo)
        {
            try
            {
                using (contextoDB.Conexion) 
                {
                    string insertaEstiloSQL = "INSERT INTO estilos (nombre) " +
                                              "VALUES (@Nombre)";

                    await contextoDB.Conexion.ExecuteAsync(insertaEstiloSQL, unEstilo);
                }
            }
            catch (SqliteException error)
            {
                throw new AppValidationException(error.Message);                
            }
        }
    }
}
