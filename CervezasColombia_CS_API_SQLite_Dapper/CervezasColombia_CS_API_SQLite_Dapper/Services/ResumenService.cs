using CervezasColombia_CS_API_SQLite_Dapper.Data.Entities;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;

namespace CervezasColombia_CS_API_SQLite_Dapper.Services
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
            return await _resumenRepository.GetAllAsync();
        }
    }
}
