using CervezasColombia_CS_API_SQLite_Dapper.Data.Entities;

namespace CervezasColombia_CS_API_SQLite_Dapper.Interfaces
{
    public interface IResumenRepository
    {
        public Task<Resumen> GetAllAsync();
    }
}