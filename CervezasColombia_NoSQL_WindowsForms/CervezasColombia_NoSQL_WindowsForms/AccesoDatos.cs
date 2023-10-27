using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CervezasColombia_NoSQL_WindowsForms.Modelos;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CervezasColombia_NoSQL_WindowsForms
{
    public class AccesoDatos
    {
        static CervezasDatabaseSettings configDB = ObtenerDBSettings();

        public static CervezasDatabaseSettings ObtenerDBSettings()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration miConfiguracion = builder.Build();

            CervezasDatabaseSettings miConfigDB = new()
            {
                ConnectionString = miConfiguracion["CervezasColombiaDatabase:ConnectionString"]!,
                DatabaseName = miConfiguracion["CervezasColombiaDatabase:DatabaseName"]!,
                ColeccionCervecerias = miConfiguracion["CervezasColombiaDatabase:ColeccionCervecerias"]!
            };

            return miConfigDB;
        }

        public static List<string> ObtenerListaNombresCervecerias()
        {
            var clienteDB = new MongoClient(configDB.ConnectionString);
            var miDB = clienteDB.GetDatabase(configDB.DatabaseName);
            var coleccionCervecerias = configDB.ColeccionCervecerias;

            var listaCervecerias = miDB.GetCollection<Cerveceria>(coleccionCervecerias)
                .Find(_ => true)
                .SortBy(cerveceria => cerveceria.Nombre)
                .ToList();

            List<string> listaNombres = new List<string>();

            foreach (Cerveceria unaCerveceria in listaCervecerias)
                listaNombres.Add(unaCerveceria.Nombre!);

            return listaNombres;
        }

        public static Cerveceria ObtenerCerveceriaPorNombre(string nombreCerveceria)
        {
            var clienteDB = new MongoClient(configDB.ConnectionString);
            var miDB = clienteDB.GetDatabase(configDB.DatabaseName);
            var coleccionCervecerias = configDB.ColeccionCervecerias;

            var unaCerveceria = miDB.GetCollection<Cerveceria>(coleccionCervecerias)
                .Find(cerveceria => cerveceria.Nombre == nombreCerveceria)
                .FirstOrDefault();

            if (unaCerveceria is null)
                return new Cerveceria();

            return unaCerveceria;
        }
    }
}
