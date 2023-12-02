using CervezasColombia_CS_API_SQLite_Dapper.Models;

namespace CervezasColombia_CS_API_SQLite_Dapper.Interfaces
{
    public interface ICerveceriaRepository
    {
        public Task<IEnumerable<Cerveceria>> GetAllAsync();
        public Task<Cerveceria> GetByIdAsync(int cerveceria_id);
        public Task<CerveceriaDetallada> GetDetailsByIdAsync(int cerveceria_id);
        public Task<Cerveceria> GetByNameAsync(string cerveceria_nombre);
        public Task<Cerveceria> GetByInstagramAsync(string cerveceria_instagram);
        public Task<int> GetTotalAssociatedBeersAsync(int cerveceria_id);
        public Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int cerveceria_id);
        public Task<Ubicacion> GetBreweryLocation(int cerveceria_id);
        public Task<bool> CreateAsync(Cerveceria unaCerveceria);
        public Task<bool> UpdateAsync(Cerveceria unaCerveceria);
        public Task<bool> DeleteAsync(Cerveceria unaCerveceria);
        public Task<bool> DeleteAssociatedBeersAsync(int cerveceria_id);
    }
}
