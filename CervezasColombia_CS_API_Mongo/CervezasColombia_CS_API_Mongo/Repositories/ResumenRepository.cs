using CervezasColombia_CS_API_Mongo.DbContexts;
using CervezasColombia_CS_API_Mongo.Interfaces;
using CervezasColombia_CS_API_Mongo.Models;
using MongoDB.Driver;
using System.Data;

namespace CervezasColombia_CS_API_Mongo.Repositories
{
    public class ResumenRepository : IResumenRepository
    {
        private readonly MongoDbContext contextoDB;

        public ResumenRepository(MongoDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<Resumen> GetAllAsync()
        {
            Resumen unResumen = new();            
            var conexion = contextoDB.CreateConnection();

            ////Total Ubicaciones
            var coleccionUbicaciones = conexion.GetCollection<Ubicacion>("ubicaciones");
            var totalUbicaciones = await coleccionUbicaciones
                .EstimatedDocumentCountAsync();

            unResumen.Ubicaciones = totalUbicaciones;

            ////Total Estilos
            var coleccionEstilos = conexion.GetCollection<Estilo>("estilos");
            var totalEstilos = await coleccionEstilos
                .EstimatedDocumentCountAsync();

            unResumen.Estilos = totalEstilos;

            //Total Tipos de Ingredientes
            var coleccionTiposIngredientes = conexion.GetCollection<TipoIngrediente>("tipos_ingredientes");
            var totalTiposIngredientes = await coleccionTiposIngredientes
                .EstimatedDocumentCountAsync();

            unResumen.Tipos_Ingredientes = totalTiposIngredientes;

            //Total envasados
            var coleccionEnvasados = conexion.GetCollection<Envasado>("envasados");
            var totalEnvasados = await coleccionEnvasados
                .EstimatedDocumentCountAsync();

            unResumen.Envasados = totalEnvasados;

            //Total ingredientes
            var coleccionIngredientes = conexion.GetCollection<Ingrediente>("ingredientes");
            var totalIngredientes = await coleccionIngredientes
                .EstimatedDocumentCountAsync();

            unResumen.Ingredientes = totalIngredientes;

            //Total Cervecerías
            var coleccionCervecerias = conexion.GetCollection<Cerveceria>("cervecerias");
            var totalCervecerias = await coleccionCervecerias
                .EstimatedDocumentCountAsync();

            unResumen.Cervecerias = totalCervecerias;

            //Total Cervezas
            var coleccionCervezas = conexion.GetCollection<Cerveza>("cervezas");
            var totalCervezas = await coleccionCervezas
                .EstimatedDocumentCountAsync();

            unResumen.Cervezas = totalCervezas;

            return unResumen;
        }
    }
}