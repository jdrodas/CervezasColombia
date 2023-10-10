using MongoDB.Driver;

namespace CervezasColombia_CS_API_Mongo.DbContexts
{
    public class MongoDbContext
    {
        private readonly string cadenaConexion;
        public MongoDbContext(IConfiguration unaConfiguracion)
        {
            cadenaConexion = unaConfiguracion.GetConnectionString("Mongo")!;
        }

        public IMongoDatabase CreateConnection()
        {
            var clienteDB = new MongoClient(cadenaConexion);
            var miDB = clienteDB.GetDatabase("cervezas_db");

            return miDB;
        }
    }
}