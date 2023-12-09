using Microsoft.Data.Sqlite;
using System.Data;

namespace CervezasColombia_CS_API_SQLite_Dapper.Helpers
{
    public class SQLiteDbContext(IConfiguration unaConfiguracion)
    {
        private IDbConnection conexionDB = new SqliteConnection(
                unaConfiguracion.GetConnectionString("SQLite"));

        public IDbConnection Conexion => conexionDB;
    }
}
