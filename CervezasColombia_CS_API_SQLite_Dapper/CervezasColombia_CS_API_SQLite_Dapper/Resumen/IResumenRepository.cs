namespace CervezasColombia_CS_API_SQLite_Dapper.Resumen
{
    public interface IResumenRepository
    {
        public Task<Resumen> GetAllAsync();
    }
}