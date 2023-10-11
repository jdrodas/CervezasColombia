using CervezasColombia_CS_API_Mongo.Models;

namespace CervezasColombia_CS_API_Mongo.Interfaces
{
    public interface IResumenRepository
    {
        public Task<Resumen> GetAllAsync();
    }
}