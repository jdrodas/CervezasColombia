using CervezasColombia_CS_API_SQLite_Dapper.Models;

namespace CervezasColombia_CS_API_SQLite_Dapper.Interfaces
{
    public interface IEstiloRepository
    {
        public Task<IEnumerable<Estilo>> GetAllAsync();
        public Task<Estilo> GetByIdAsync(int id);
        public Task<Estilo> GetByNameAsync(string nombre);
        public Task<int> GetTotalAssociatedBeersAsync(int id);
        public Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int id);
        public Task<bool> CreateAsync(Estilo unEstilo);
        public Task<bool> UpdateAsync(Estilo unEstilo);
        public Task<bool> DeleteAsync(Estilo unEstilo);
    }
}
