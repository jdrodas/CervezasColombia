using CervezasColombia_CS_API_SQLite_Dapper.Models;

namespace CervezasColombia_CS_API_SQLite_Dapper.Interfaces
{
    public interface ICervezaRepository
    {
        public Task<IEnumerable<Cerveza>> GetAllAsync();
        public Task<Cerveza> GetByIdAsync(int cerveza_id);
        public Task<Cerveza> GetByNameAndBreweryAsync(string cerveza_nombre, string cerveceria);
        //public Task<int> GetTotalAssociatedIngredientsAsync(int cerveza_id);
        //public Task<IEnumerable<Ingrediente>> GetAssociatedIngredientsAsync(int cerveza_id);
        //public Task<int> GetTotalAssociatedPackagingsAsync(int cerveza_id);
        //public Task<IEnumerable<EnvasadoCerveza>> GetAssociatedPackagingsAsync(int cerveza_id);
        public Task<string> GetAbvRangeNameAsync(double abv);
        public Task<bool> CreateAsync(Cerveza unaCerveza);
        //public Task<bool> CreateBeerPackagingAsync(int cerveza_id, EnvasadoCerveza unEnvasadoCerveza);
        //public Task<bool> CreateBeerIngredientAsync(int cerveza_id, Ingrediente unIngrediente);
        //public Task<bool> UpdateAsync(Cerveza unaCerveza);
        //public Task<bool> DeleteAsync(Cerveza unaCerveza);
        //public Task<bool> DeleteBeerPackagingAsync(int cerveza_id, EnvasadoCerveza unEnvasadoCerveza);
        //public Task<bool> DeleteBeerIngredientAsync(int cerveza_id, Ingrediente unIngrediente);
    }
}
