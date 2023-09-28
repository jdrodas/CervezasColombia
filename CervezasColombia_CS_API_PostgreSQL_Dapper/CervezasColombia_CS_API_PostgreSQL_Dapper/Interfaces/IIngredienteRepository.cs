using CervezasColombia_CS_API_PostgreSQL_Dapper.Models;


namespace CervezasColombia_CS_API_PostgreSQL_Dapper.Interfaces
{
    public interface IIngredienteRepository
    {
        public Task<IEnumerable<Ingrediente>> GetAllAsync();
        public Task<Ingrediente> GetByIdAsync(int ingrediente_id);
        public Task<Ingrediente> GetByNameAndTypeAsync(string ingrediente_nombre, string ingrediente_tipo);
        public Task<int> GetTotalAssociatedBeersAsync(int ingrediente_id);
        public Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int ingrediente_id);
        public Task<Cerveza> GetAssociatedBeerByIdAsync(int ingrediente_id, int cerveza_id);
        public Task<int> GetAssociatedIngredientTypeIdAsync(string tipo_ingrediente_nombre);
        public Task<bool> CreateAsync(Ingrediente unIngrediente);
        public Task<bool> UpdateAsync(Ingrediente unIngrediente);
        public Task<bool> DeleteAsync(Ingrediente unIngrediente);
    }
}
