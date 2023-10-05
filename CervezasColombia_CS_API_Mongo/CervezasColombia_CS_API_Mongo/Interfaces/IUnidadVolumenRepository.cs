using CervezasColombia_CS_API_PostgreSQL_Dapper.Models;

namespace CervezasColombia_CS_API_PostgreSQL_Dapper.Interfaces
{
    public interface IUnidadVolumenRepository
    {
        public Task<UnidadVolumen> GetByNameAsync(string unidad_volumen_nombre);
    }
}
