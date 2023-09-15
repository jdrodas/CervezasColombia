using CervezasColombia_CS_API_SQLite_Dapper.Models;

namespace CervezasColombia_CS_API_SQLite_Dapper.Interfaces
{
    public interface IEnvasadoRepository
    {
        public Task<IEnumerable<Envasado>> GetAllAsync();
        public Task<Envasado> GetByIdAsync(int id);
        public Task<int> GetTotalAssociatedBeersAsync(int id);
        public Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int id);
    }
}