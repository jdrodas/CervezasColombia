using CervezasColombia_CS_API_Mongo.DbContexts;
using CervezasColombia_CS_API_Mongo.Interfaces;
using CervezasColombia_CS_API_Mongo.Models;

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
            var coleccionUbicaciones = conexion.GetCollection<Ubicacion>(contextoDB.configuracionColecciones.ColeccionUbicaciones);
            var totalUbicaciones = await coleccionUbicaciones
                .EstimatedDocumentCountAsync();

            unResumen.Ubicaciones = totalUbicaciones;

            ////Total Estilos
            var coleccionEstilos = conexion.GetCollection<Estilo>(contextoDB.configuracionColecciones.ColeccionEstilos);
            var totalEstilos = await coleccionEstilos
                .EstimatedDocumentCountAsync();

            unResumen.Estilos = totalEstilos;

            //Total Tipos de Ingredientes
            var coleccionTiposIngredientes = conexion.GetCollection<TipoIngrediente>(contextoDB.configuracionColecciones.ColeccionTiposIngredientes);
            var totalTiposIngredientes = await coleccionTiposIngredientes
                .EstimatedDocumentCountAsync();

            unResumen.Tipos_Ingredientes = totalTiposIngredientes;

            //Total envasados
            var coleccionEnvasados = conexion.GetCollection<Envasado>(contextoDB.configuracionColecciones.ColeccionEnvasados);
            var totalEnvasados = await coleccionEnvasados
                .EstimatedDocumentCountAsync();

            unResumen.Envasados = totalEnvasados;

            //Total ingredientes
            var coleccionIngredientes = conexion.GetCollection<Ingrediente>(contextoDB.configuracionColecciones.ColeccionIngredientes);
            var totalIngredientes = await coleccionIngredientes
                .EstimatedDocumentCountAsync();

            unResumen.Ingredientes = totalIngredientes;

            //Total Cervecerías
            var coleccionCervecerias = conexion.GetCollection<Cerveceria>(contextoDB.configuracionColecciones.ColeccionCervecerias);
            var totalCervecerias = await coleccionCervecerias
                .EstimatedDocumentCountAsync();

            unResumen.Cervecerias = totalCervecerias;

            //Total Cervezas
            var coleccionCervezas = conexion.GetCollection<Cerveza>(contextoDB.configuracionColecciones.ColeccionCervezas);
            var totalCervezas = await coleccionCervezas
                .EstimatedDocumentCountAsync();

            unResumen.Cervezas = totalCervezas;

            //Unidades de Volumen
            var coleccionUnidadesVolumen = conexion.GetCollection<UnidadVolumen>(contextoDB.configuracionColecciones.ColeccionUnidadesVolumen);
            var totalUnidadesVolumen = await coleccionUnidadesVolumen
                .EstimatedDocumentCountAsync();

            unResumen.Unidades_Volumen = totalUnidadesVolumen;

            return unResumen;
        }
    }
}