using CervezasColombia_CS_API_Mongo.Models;

namespace CervezasColombia_CS_API_Mongo.Interfaces
{
    public interface ICerveceriaRepository
    {
        public Task<IEnumerable<Cerveceria>> GetAllAsync();
        public Task<Cerveceria> GetByIdAsync(string cerveceria_id);
        public Task<CerveceriaDetallada> GetDetailsByIdAsync(string cerveceria_id);
        public Task<Cerveceria> GetByNameAsync(string cerveceria_nombre);
        public Task<Cerveceria> GetByInstagramAsync(string cerveceria_instagram);
        public Task<Cerveceria> GetBySitioWebAsync(string cerveceria_sitio_web);
        public Task<int> GetTotalAssociatedBeersAsync(string cerveceria_id);
        public Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(string cerveceria_id);
        public Task<Ubicacion> GetAssociatedLocationAsync(string cerveceria_id);
        public Task<bool> CreateAsync(Cerveceria unaCerveceria);
        public Task<bool> UpdateAsync(Cerveceria unaCerveceria);
        public Task<bool> DeleteAsync(Cerveceria unaCerveceria);
        public Task<bool> DeleteAssociatedBeersAsync(string cerveceria_id);
    }
}
