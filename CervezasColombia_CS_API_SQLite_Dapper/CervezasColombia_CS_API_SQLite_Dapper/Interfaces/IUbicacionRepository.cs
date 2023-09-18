using CervezasColombia_CS_API_SQLite_Dapper.Models;

namespace CervezasColombia_CS_API_SQLite_Dapper.Interfaces
{
    public interface IUbicacionRepository
    {
        public Task<IEnumerable<Ubicacion>> GetAllAsync();
        public Task<Ubicacion> GetByIdAsync(int ubicacion_id);
        public Task<Ubicacion> GetByNameAsync(string ubicacion_municipio, string ubicacion_departamento);
        public Task<int> GetTotalAssociatedBreweriesAsync(int ubicacion_id);
        public Task<IEnumerable<Cerveceria>> GetAssociatedBreweriesAsync(int ubicacion_id);
        public Task<bool> CreateAsync(Ubicacion unaUbicacion);
        public Task<bool> UpdateAsync(Ubicacion unaUbicacion);
        public Task<bool> DeleteAsync(Ubicacion unaUbicacion);
    }
}
