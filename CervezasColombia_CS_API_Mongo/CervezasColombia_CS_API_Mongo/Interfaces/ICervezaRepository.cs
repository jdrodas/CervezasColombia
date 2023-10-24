using CervezasColombia_CS_API_Mongo.Models;

namespace CervezasColombia_CS_API_Mongo.Interfaces
{
    public interface ICervezaRepository
    {
        public Task<IEnumerable<Cerveza>> GetAllAsync();
        public Task<Cerveza> GetByIdAsync(string cerveza_id);
        public Task<Cerveza> GetByNameAndBreweryAsync(string cerveza_nombre, string cerveceria);
        public Task<int> GetTotalAssociatedIngredientsAsync(string cerveza_id);
        public Task<IEnumerable<IngredienteCerveza>> GetAssociatedIngredientsAsync(string cerveza_id);
        public Task<int> GetTotalAssociatedPackagingsAsync(string cerveza_id);
        public Task<IEnumerable<EnvasadoCerveza>> GetAssociatedPackagingsAsync(string cerveza_id);
        public Task<EnvasadoCerveza> GetPackagedBeerByNameAsync(string cerveza_id, string envasado_nombre, string unidad_volumen, double volumen);
        public Task<string> GetAbvRangeNameAsync(double abv);
        public Task<string> GetIbuRangeNameAsync(double ibu);
        public Task<bool> CreateAsync(Cerveza unaCerveza);
        public Task<bool> CreateBeerPackagingAsync(EnvasadoCerveza unEnvasadoCerveza);
        public Task<bool> CreateBeerIngredientAsync(string cerveza_id, Ingrediente unIngrediente);
        public Task<bool> UpdateAsync(Cerveza unaCerveza);
        public Task<bool> DeleteAsync(Cerveza unaCerveza);
        public Task<bool> DeleteBeerPackagingAsync(string cerveza_id, EnvasadoCerveza unEnvasadoCerveza);
        public Task<bool> DeleteBeerIngredientAsync(string cerveza_id, Ingrediente unIngrediente);
    }
}


