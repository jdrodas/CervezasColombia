using CervezasColombia_CS_API_PostgreSQL_Dapper.Models;

namespace CervezasColombia_CS_API_PostgreSQL_Dapper.Interfaces
{
    public interface IEnvasadoRepository
    {
        public Task<IEnumerable<Envasado>> GetAllAsync();
        public Task<Envasado> GetByIdAsync(int envasado_id);
        public Task<Envasado> GetByNameAsync(string envasado_nombre);
        public Task<int> GetTotalAssociatedBeersAsync(int envasado_id);
        public Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int envasado_id);
        public Task<EnvasadoCerveza> GetAssociatedBeerPackagingAsync(int cerveza_id, int envasado_id, int unidad_volumen_id, float volumen);
        public Task<bool> CreateAsync(Envasado unEnvasado);
        public Task<bool> UpdateAsync(Envasado unEnvasado);
        public Task<bool> DeleteAsync(Envasado unEnvasado);
    }
}