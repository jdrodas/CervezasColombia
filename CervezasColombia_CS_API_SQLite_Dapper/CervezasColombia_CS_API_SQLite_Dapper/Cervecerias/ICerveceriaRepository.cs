using CervezasColombia_CS_API_SQLite_Dapper.Cervezas;
using CervezasColombia_CS_API_SQLite_Dapper.Ubicaciones;

namespace CervezasColombia_CS_API_SQLite_Dapper.Cervecerias
{
    public interface ICerveceriaRepository
    {
        public Task<IEnumerable<Cerveceria>> GetAllAsync();
        public Task<Cerveceria> GetByAttributeAsync<T>(T atributo_valor, string atributo_nombre);
        public Task<int> GetTotalAssociatedBeersAsync(int cerveceria_id);
        public Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int cerveceria_id);
        public Task<Ubicacion> GetBreweryLocation(int cerveceria_id);
        public Task<bool> CreateAsync(Cerveceria cerveceria);
        public Task<bool> UpdateAsync(Cerveceria cerveceria);
        public Task<bool> DeleteAsync(Cerveceria cerveceria);
        public Task<bool> DeleteAssociatedBeersAsync(int cerveceria_id);
    }
}
