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
            Resumen unResumen = new Resumen();

            using (contextoDB.Conexion)
            {
                //Total Ubicaciones
                string sentenciaSQL = "select count(id) total from ubicaciones";
                unResumen.Ubicaciones = await contextoDB.Conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

                //Total Cervecerías
                sentenciaSQL = "select count(id) total from cervecerias";
                unResumen.Cervecerias = await contextoDB.Conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

                //Total Cervezas
                sentenciaSQL = "select count(id) total from cervezas";
                unResumen.Cervezas = await contextoDB.Conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

                //Total Estilos
                sentenciaSQL = "select count(id) total from estilos";
                unResumen.Estilos = await contextoDB.Conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

                //Total envasados
                sentenciaSQL = "select count(id) total from envasados";
                unResumen.Envasados = await contextoDB.Conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

                //Total ingredientes
                sentenciaSQL = "select count(id) total from ingredientes";
                unResumen.Ingredientes = await contextoDB.Conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());
            }

            return unResumen;
        }
    }
}