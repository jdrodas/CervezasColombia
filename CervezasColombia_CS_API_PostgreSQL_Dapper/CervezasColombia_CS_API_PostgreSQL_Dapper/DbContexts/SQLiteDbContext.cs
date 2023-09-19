using Microsoft.Data.Sqlite;
using System.Data;

namespace CervezasColombia_CS_API_PostgreSQL_Dapper.DbContexts
{
    public class SQLiteDbContext
    {
        private IDbConnection conexionDB;

        public SQLiteDbContext(IConfiguration unaConfiguracion)
        {
            conexionDB = new SqliteConnection(
                unaConfiguracion.GetConnectionString("SQLite"));
        }
        public IDbConnection Conexion => conexionDB;
    }
}
