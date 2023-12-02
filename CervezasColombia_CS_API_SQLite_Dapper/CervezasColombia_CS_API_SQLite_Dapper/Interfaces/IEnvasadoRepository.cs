using CervezasColombia_CS_API_SQLite_Dapper.Models;

namespace CervezasColombia_CS_API_SQLite_Dapper.Interfaces
{
    public interface IEnvasadoRepository
    {
        public Task<IEnumerable<Envasado>> GetAllAsync();
        public Task<Envasado> GetByIdAsync(int envasado_id);
        public Task<Envasado> GetByNameAsync(string envasado_nombre);
        public Task<int> GetTotalAssociatedPackagedBeersAsync(int envasado_id);
        public Task<IEnumerable<EnvasadoCerveza>> GetAssociatedPackagedBeersAsync(int envasado_id);
        public Task<EnvasadoCerveza> GetAssociatedBeerPackagingAsync(int cerveza_id, int envasado_id);
        public Task<bool> CreateAsync(Envasado unEnvasado);
        public Task<bool> UpdateAsync(Envasado unEnvasado);
        public Task<bool> DeleteAsync(Envasado unEnvasado);
        public Task<bool> DeleteAssociatedBeersAsync(int envasado_id);
    }
}