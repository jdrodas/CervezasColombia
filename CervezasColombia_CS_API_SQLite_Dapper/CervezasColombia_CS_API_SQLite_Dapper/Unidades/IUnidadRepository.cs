namespace CervezasColombia_CS_API_SQLite_Dapper.Unidades
{
    public interface IUnidadVolumenRepository
    {
        public Task<Unidad> GetByNameAsync(string unidad_nombre);
    }
}
