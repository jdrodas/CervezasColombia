using Dapper;
using System.Data;
using Microsoft.Data.Sqlite;
using CervezasColombia_CS_API_SQLite_Dapper.DbContexts;
using CervezasColombia_CS_API_SQLite_Dapper.Models;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;

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
                var total = await contextoDB.Conexion.QueryAsync<int>(sentenciaSQL,new DynamicParameters());
                unResumen.Ubicaciones = total.First();

                //Total Cervecerías
                sentenciaSQL = "select count(id) total from cervecerias";
                total = await contextoDB.Conexion.QueryAsync<int>(sentenciaSQL, new DynamicParameters());
                unResumen.Cervecerias = total.First();

                //Total Cervezas
                sentenciaSQL = "select count(id) total from cervezas";
                total = await contextoDB.Conexion.QueryAsync<int>(sentenciaSQL, new DynamicParameters());
                unResumen.Cervezas = total.First();

                //Total Estilos
                sentenciaSQL = "select count(id) total from estilos";
                total = await contextoDB.Conexion.QueryAsync<int>(sentenciaSQL, new DynamicParameters());
                unResumen.Estilos = total.First();

                //Total envasados
                sentenciaSQL = "select count(id) total from envasados";
                total = await contextoDB.Conexion.QueryAsync<int>(sentenciaSQL, new DynamicParameters());
                unResumen.Envasados = total.First();

                //Total ingredientes
                sentenciaSQL = "select count(id) total from ingredientes";
                total = await contextoDB.Conexion.QueryAsync<int>(sentenciaSQL, new DynamicParameters());
                unResumen.Ingredientes = total.First();
            }

            return unResumen;
        }
    }
}