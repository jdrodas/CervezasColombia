namespace CervezasColombia_CS_API_SQLite_Dapper.Unidades
{
    public interface IUnidadRepository
    {
        public Task<IEnumerable<Unidad>> GetAllAsync();
        public Task<Unidad> GetByAttributeAsync<T>(T atributo_valor, string atributo_nombre);
    }
}
