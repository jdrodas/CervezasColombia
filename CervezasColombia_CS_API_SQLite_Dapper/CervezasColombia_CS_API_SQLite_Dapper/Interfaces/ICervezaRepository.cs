using CervezasColombia_CS_API_SQLite_Dapper.Models;

namespace CervezasColombia_CS_API_SQLite_Dapper.Interfaces
{
    public interface ICervezaRepository
    {
        public Task<IEnumerable<Cerveza>> GetAllAsync();
        public Task<Cerveza> GetByIdAsync(int id);
        public Task<Cerveza> GetByNameAndBreweryAsync(string cerveza_nombre, string cerveceria);
        public Task<int> GetTotalAssociatedIngredientsAsync(int id);
        public Task<IEnumerable<Ingrediente>> GetAssociatedIngredientsAsync(int id);
        public Task<int> GetTotalAssociatedPackagingsAsync(int id);
        public Task<IEnumerable<Envasado>> GetAssociatedPackagingsAsync(int id);
        public Task<bool> CreateAsync(Cerveza unaCerveza);
        public Task<bool> UpdateAsync(Cerveza unaCerveza);
        public Task<bool> DeleteAsync(Cerveza unaCerveza);
    }
}
