using CervezasColombia_CS_API_SQLite_Dapper.Data.Entities;

namespace CervezasColombia_CS_API_SQLite_Dapper.Interfaces
{
    public interface IEstiloRepository
    {
        public Task<IEnumerable<Estilo>> GetAllAsync();

        public Task<Estilo> GetByIdAsync(int id);

        public Task<Estilo> GetByNameAsync(string nombre);

        public Task CreateAsync(Estilo unEstilo);
    }
}
