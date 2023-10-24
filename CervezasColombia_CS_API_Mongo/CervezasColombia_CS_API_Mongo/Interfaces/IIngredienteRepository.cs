using CervezasColombia_CS_API_Mongo.Models;


namespace CervezasColombia_CS_API_Mongo.Interfaces
{
    public interface IIngredienteRepository
    {
        public Task<IEnumerable<Ingrediente>> GetAllAsync();
        public Task<Ingrediente> GetByIdAsync(string ingrediente_id);
        public Task<Ingrediente> GetByNameAndTypeAsync(string ingrediente_nombre, string ingrediente_tipo);
        //public Task<int> GetTotalAssociatedBeersAsync(int ingrediente_id);
        //public Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int ingrediente_id);
        public Task<Cerveza> GetAssociatedBeerByIdAsync(string ingrediente_id, string cerveza_id);
        //public Task<int> GetAssociatedIngredientTypeIdAsync(string tipo_ingrediente_nombre);
        //public Task<bool> CreateAsync(Ingrediente unIngrediente);
        //public Task<bool> UpdateAsync(Ingrediente unIngrediente);
        //public Task<bool> DeleteAsync(Ingrediente unIngrediente);
    }
}
