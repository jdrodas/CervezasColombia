using CervezasColombia_CS_API_SQLite_Dapper.Data.Entities;

namespace CervezasColombia_CS_API_SQLite_Dapper.Interfaces
{
    public interface IEstiloRepository
    {
        public Task<List<Estilo>> GetAllEstilosAsync();

        public Task<Estilo> GetEstiloByIdAsync(int id);
    }
}
