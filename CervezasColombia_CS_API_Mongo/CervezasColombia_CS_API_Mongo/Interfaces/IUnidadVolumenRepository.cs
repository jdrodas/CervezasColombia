using CervezasColombia_CS_API_Mongo.Models;

namespace CervezasColombia_CS_API_Mongo.Interfaces
{
    public interface IUnidadVolumenRepository
    {
        public Task<UnidadVolumen> GetByNameAsync(string unidad_volumen_nombre);
    }
}
