using CervezasColombia_CS_API_SQLite_Dapper.DbContexts;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;
using CervezasColombia_CS_API_SQLite_Dapper.Models;
using Dapper;

namespace CervezasColombia_CS_API_SQLite_Dapper.Repositories
{
    public class ResumenRepository : IResumenRepository
    {
        private readonly SQLiteDbContext contextoDB;

        public ResumenRepository(SQLiteDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<Resumen> GetAllAsync()
        {
            Resumen unResumen = new();

            using (contextoDB.Conexion)
            {
                //Total Ubicaciones
                string sentenciaSQL = "SELECT COUNT(id) total FROM ubicaciones";
                unResumen.Ubicaciones = await contextoDB.Conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

                //Total Cervecerías
                sentenciaSQL = "SELECT COUNT(id) total FROM cervecerias";
                unResumen.Cervecerias = await contextoDB.Conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

                //Total Cervezas
                sentenciaSQL = "SELECT COUNT(id) total FROM cervezas";
                unResumen.Cervezas = await contextoDB.Conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

                //Total Estilos
                sentenciaSQL = "SELECT COUNT(id) total FROM estilos";
                unResumen.Estilos = await contextoDB.Conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

                //Total envasados
                sentenciaSQL = "SELECT COUNT(id) total FROM envasados";
                unResumen.Envasados = await contextoDB.Conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

                //Total ingredientes
                sentenciaSQL = "SELECT COUNT(id) total FROM ingredientes";
                unResumen.Ingredientes = await contextoDB.Conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

                //Total Tipos de ingredientes
                sentenciaSQL = "SELECT COUNT(id) total FROM tipos_ingredientes";
                unResumen.Tipos_Ingredientes = await contextoDB.Conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());
            }

            return unResumen;
        }
    }
}