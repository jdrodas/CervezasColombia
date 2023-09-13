using CervezasColombia_CS_API_SQLite_Dapper.Models;

namespace CervezasColombia_CS_API_SQLite_Dapper.Interfaces
{
    public interface ICerveceriaRepository
    {
        public Task<IEnumerable<Cerveceria>> GetAllAsync();
        public Task<Cerveceria> GetByIdAsync(int id);
        public Task<Cerveceria> GetByNameAsync(string nombre);
        public Task<Cerveceria> GetByInstagramAsync(string instagram);
        public Task<Cerveceria> GetBySitioWebAsync(string sitio_web);
        public Task<int> GetTotalAssociatedBeersAsync(int id);
        public Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int id);
        public Task<int> GetAssociatedLocationIdAsync(string unaUbicacion);
        public Task<bool> CreateAsync(Cerveceria unaCerveceria);
        public Task<bool> UpdateAsync(Cerveceria unaCerveceria);
        public Task<bool> DeleteAsync(Cerveceria unaCerveceria);
    }
}
