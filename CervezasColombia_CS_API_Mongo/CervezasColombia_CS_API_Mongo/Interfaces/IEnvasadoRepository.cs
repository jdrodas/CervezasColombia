using CervezasColombia_CS_API_Mongo.Models;

namespace CervezasColombia_CS_API_Mongo.Interfaces
{
    public interface IEnvasadoRepository
    {
        public Task<IEnumerable<Envasado>> GetAllAsync();
        public Task<Envasado> GetByIdAsync(string envasado_id);
        public Task<Envasado> GetByNameAsync(string envasado_nombre);
        public Task<int> GetTotalAssociatedPackagedBeersAsync(string envasado_id);
        public Task<IEnumerable<EnvasadoCerveza>> GetAssociatedPackagedBeersAsync(string envasado_id);
        //public Task<EnvasadoCerveza> GetAssociatedBeerPackagingAsync(int cerveza_id, int envasado_id, int unidad_volumen_id, float volumen);
        public Task<bool> CreateAsync(Envasado unEnvasado);
        public Task<bool> UpdateAsync(Envasado unEnvasado);
        public Task<bool> DeleteAsync(Envasado unEnvasado);
    }
}