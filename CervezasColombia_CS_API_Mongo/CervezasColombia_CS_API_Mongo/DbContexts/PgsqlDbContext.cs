using Npgsql;
using System.Data;

namespace CervezasColombia_CS_API_PostgreSQL_Dapper.DbContexts
{
    public class PgsqlDbContext
    {
        private readonly string cadenaConexion;
        public PgsqlDbContext(IConfiguration unaConfiguracion)
        {
            cadenaConexion = unaConfiguracion.GetConnectionString("PgSql")!;
        }

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(cadenaConexion);
        }
    }
}