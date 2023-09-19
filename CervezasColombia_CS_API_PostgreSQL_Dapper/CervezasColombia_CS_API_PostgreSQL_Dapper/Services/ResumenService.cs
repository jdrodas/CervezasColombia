using CervezasColombia_CS_API_PostgreSQL_Dapper.Interfaces;
using CervezasColombia_CS_API_PostgreSQL_Dapper.Models;

namespace CervezasColombia_CS_API_PostgreSQL_Dapper.Services
{
    public class ResumenService
    {
        private readonly IResumenRepository _resumenRepository;

        public ResumenService(IResumenRepository resumenRepository)
        {
            _resumenRepository = resumenRepository;
        }

        public async Task<Resumen> GetAllAsync()
        {
            return await _resumenRepository
                .GetAllAsync();
        }
    }
}
