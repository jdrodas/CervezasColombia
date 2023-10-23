using CervezasColombia_CS_API_Mongo.DbContexts;
using CervezasColombia_CS_API_Mongo.Interfaces;
using CervezasColombia_CS_API_Mongo.Models;
using MongoDB.Driver;

namespace CervezasColombia_CS_API_Mongo.Repositories
{
    public class UnidadVolumenRepository : IUnidadVolumenRepository
    {
        private readonly MongoDbContext contextoDB;

        public UnidadVolumenRepository(MongoDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<UnidadVolumen> GetByNameAsync(string unidad_volumen_nombre)
        {
            UnidadVolumen unaUnidadVolumen = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionUnidadesVolumen = conexion.GetCollection<UnidadVolumen>("unidades_volumen");

            var resultado = await coleccionUnidadesVolumen
                .Find(unidadVolumen => unidadVolumen.Nombre == unidad_volumen_nombre)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaUnidadVolumen = resultado;

            return unaUnidadVolumen;
        }

    }
}