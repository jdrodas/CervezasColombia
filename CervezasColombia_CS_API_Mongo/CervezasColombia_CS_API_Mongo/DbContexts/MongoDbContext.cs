using CervezasColombia_CS_API_Mongo.Models;
using MongoDB.Driver;

namespace CervezasColombia_CS_API_Mongo.DbContexts
{
    public class MongoDbContext
    {
        private readonly string cadenaConexion;
        private readonly CervezasDatabaseSettings _cervezasDatabaseSettings;
        public MongoDbContext(IConfiguration unaConfiguracion)
        {
            cadenaConexion = unaConfiguracion.GetConnectionString("Mongo")!;
            _cervezasDatabaseSettings = new CervezasDatabaseSettings(unaConfiguracion);
        }

        public IMongoDatabase CreateConnection()
        {
            var clienteDB = new MongoClient(cadenaConexion);
            var miDB = clienteDB.GetDatabase(_cervezasDatabaseSettings.DatabaseName);

            return miDB;
        }

        public CervezasDatabaseSettings configuracionColecciones
        {
            get { return _cervezasDatabaseSettings; }
        }
    }
}