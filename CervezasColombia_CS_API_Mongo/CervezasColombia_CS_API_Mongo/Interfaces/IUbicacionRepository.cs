using CervezasColombia_CS_API_Mongo.Models;

namespace CervezasColombia_CS_API_Mongo.Interfaces
{
    public interface IUbicacionRepository
    {
        public Task<IEnumerable<Ubicacion>> GetAllAsync();
        public Task<Ubicacion> GetByIdAsync(string ubicacion_id);
        public Task<Ubicacion> GetByNameAsync(string ubicacion_municipio, string ubicacion_departamento);
        public Task<Ubicacion> GetByNameAsync(string ubicacion_nombre);
        public Task<int> GetTotalAssociatedBreweriesAsync(string ubicacion_id);
        public Task<IEnumerable<Cerveceria>> GetAssociatedBreweriesAsync(string ubicacion_id);
        public Task<bool> CreateAsync(Ubicacion unaUbicacion);
        public Task<bool> UpdateAsync(Ubicacion unaUbicacion);
        public Task<bool> DeleteAsync(Ubicacion unaUbicacion);
    }
}
