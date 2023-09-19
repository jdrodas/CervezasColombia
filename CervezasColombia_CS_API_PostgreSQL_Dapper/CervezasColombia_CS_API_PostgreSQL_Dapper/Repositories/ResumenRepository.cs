    using CervezasColombia_CS_API_PostgreSQL_Dapper.DbContexts;
using CervezasColombia_CS_API_PostgreSQL_Dapper.Interfaces;
using CervezasColombia_CS_API_PostgreSQL_Dapper.Models;
using Dapper;

namespace CervezasColombia_CS_API_PostgreSQL_Dapper.Repositories
{
    public class ResumenRepository : IResumenRepository
    {
        private readonly PgsqlDbContext contextoDB;

        public ResumenRepository(PgsqlDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<Resumen> GetAllAsync()
        {
            Resumen unResumen = new Resumen();

            using (var conexion = contextoDB.CreateConnection())
            {
                //Total Ubicaciones
                string sentenciaSQL = "SELECT COUNT(id) total FROM ubicaciones";
                unResumen.Ubicaciones = await conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

                //Total Cervecerías
                sentenciaSQL = "SELECT COUNT(id) total FROM cervecerias";
                unResumen.Cervecerias = await conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

                //Total Cervezas
                sentenciaSQL = "SELECT COUNT(id) total FROM cervezas";
                unResumen.Cervezas = await conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

                //Total Estilos
                sentenciaSQL = "SELECT COUNT(id) total FROM estilos";
                unResumen.Estilos = await conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

                //Total envasados
                sentenciaSQL = "SELECT COUNT(id) total FROM envasados";
                unResumen.Envasados = await conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

                //Total ingredientes
                sentenciaSQL = "SELECT COUNT(id) total FROM ingredientes";
                unResumen.Ingredientes = await conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());
            }

            return unResumen;
        }
    }
}