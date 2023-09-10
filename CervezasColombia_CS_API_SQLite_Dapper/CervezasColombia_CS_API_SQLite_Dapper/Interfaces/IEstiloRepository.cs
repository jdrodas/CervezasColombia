using CervezasColombia_CS_API_SQLite_Dapper.Data.Entities;

namespace CervezasColombia_CS_API_SQLite_Dapper.Interfaces
{
    public interface IEstiloRepository
    {
        public Task<IEnumerable<Estilo>> GetAllAsync();
        public Task<Estilo> GetByIdAsync(int id);
        public Task<Estilo> GetByNameAsync(string nombre);
        public Task<int> GetTotalAssociatedBeersAsync(int id);
        public Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int id);
        public Task CreateAsync(Estilo unEstilo);
        public Task UpdateAsync(Estilo unEstilo);
        public Task DeleteAsync(Estilo unEstilo);
    }
}
