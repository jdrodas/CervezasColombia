using CervezasColombia_CS_API_Mongo.Interfaces;
using CervezasColombia_CS_API_Mongo.Models;

namespace CervezasColombia_CS_API_Mongo.Services
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
