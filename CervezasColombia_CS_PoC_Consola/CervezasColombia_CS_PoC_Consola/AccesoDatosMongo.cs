using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CervezasColombia_CS_PoC_Consola
{
    public class AccesoDatosMongo
    {
        public static string? ObtieneCadenaConexion()
        {
            //Parametrizamos el acceso al archivo de configuración appsettings.json
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration miConfiguracion = builder.Build();

            return miConfiguracion["ConnectionString:Mongo"];
        }


        public static List<Estilo> ObtieneEstilosCerveza()
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            var clienteDB = new MongoClient(cadenaConexion);
            var miDB = clienteDB.GetDatabase("cervezas_db");
            var ColeccionEstilos = "Estilos";

            var listaEstilos = miDB.GetCollection<Estilo>(ColeccionEstilos)
                .Find(new BsonDocument())
                .SortBy(estilo => estilo.Nombre)
                .ToList();

            return listaEstilos;
        }

        public static List<string> ObtieneNombresEstilosCerveza()
        {
            var listaEstilos = ObtieneEstilosCerveza();

            List<string> listaNombresEstilos = new List<string>();

            foreach (Estilo unEstilo in listaEstilos)
                listaNombresEstilos.Add(unEstilo.Nombre!);

            return listaNombresEstilos;
        }

        public static Estilo ObtieneEstiloCerveza(string nombre_estilo)
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            var clienteDB = new MongoClient(cadenaConexion);
            var miDB = clienteDB.GetDatabase("cervezas_db");
            var ColeccionEstilos = "Estilos";

            var filtroEstilo = new BsonDocument { { "Nombre", nombre_estilo } };

            var unEstilo = miDB.GetCollection<Estilo>(ColeccionEstilos)
                .Find(filtroEstilo)
                .FirstOrDefault();

            return unEstilo;
        }

        public static string ObtenerObjectIdEstiloCerveza(string nombre_estilo)
        {

            string? cadenaConexion = ObtieneCadenaConexion();

            var clienteDB = new MongoClient(cadenaConexion);
            var miDB = clienteDB.GetDatabase("cervezas_db");
            var ColeccionEstilos = "Estilos";

            var filtroEstilo = new BsonDocument { { "Nombre", nombre_estilo } };

            var unEstilo = miDB.GetCollection<Estilo>(ColeccionEstilos)
                .Find(filtroEstilo)
                .FirstOrDefault();

            return unEstilo.ObjectId!;
        }

        public static bool InsertaEstiloCerveza(Estilo unEstilo)
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            var clienteDB = new MongoClient(cadenaConexion);
            var miDB = clienteDB.GetDatabase("cervezas_db");
            var miColeccion = miDB.GetCollection<Estilo>("Estilos");

            miColeccion.InsertOne(unEstilo);

            string ObjectIdEstilo = ObtenerObjectIdEstiloCerveza(unEstilo.Nombre!);

            if (string.IsNullOrEmpty(ObjectIdEstilo))
                return false;
            else
                return true;
        }

        public static bool ActualizaEstiloCerveza(Estilo unEstilo)
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            var clienteDB = new MongoClient(cadenaConexion);
            var miDB = clienteDB.GetDatabase("cervezas_db");
            var miColeccion = miDB.GetCollection<Estilo>("Estilos");

            var resultadoActualizacion = miColeccion
                                            .ReplaceOne(documento => documento.Id == unEstilo.Id, unEstilo);

            return resultadoActualizacion.IsAcknowledged;
        }

        public static bool EliminaEstiloCerveza(Estilo unEstilo)
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            var clienteDB = new MongoClient(cadenaConexion);
            var miDB = clienteDB.GetDatabase("cervezas_db");
            var miColeccion = miDB.GetCollection<Estilo>("Estilos");

            var resultadoEliminacion = miColeccion.DeleteOne(documento => documento.Id == unEstilo.Id);

            return resultadoEliminacion.IsAcknowledged;
        }
    }
}
