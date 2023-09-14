using CervezasColombia_CS_API_SQLite_Dapper.Models;

namespace CervezasColombia_CS_API_SQLite_Dapper.Interfaces
{
    public interface IUbicacionRepository
    {
        public Task<IEnumerable<Ubicacion>> GetAllAsync();

        public Task<Ubicacion> GetByIdAsync(int id);

        public Task<int> GetTotalAssociatedBreweriesAsync(int id);
        public Task<IEnumerable<Cerveceria>> GetAssociatedBreweriesAsync(int id);

    }
}
