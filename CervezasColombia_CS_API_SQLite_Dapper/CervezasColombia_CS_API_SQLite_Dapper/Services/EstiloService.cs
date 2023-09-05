using CervezasColombia_CS_API_SQLite_Dapper.Data.Entities;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;

namespace CervezasColombia_CS_API_SQLite_Dapper.Services
{
    public class EstiloService
    {
        private readonly IEstiloRepository _estiloRepository;

        public EstiloService(IEstiloRepository estiloRepository)
        {
            _estiloRepository = estiloRepository;
        }
        public async Task<List<Estilo>> GetAllEstilosAsync()
        {
            return await _estiloRepository.GetAllEstilosAsync();
        }

        public async Task<Estilo> GetEstiloAsync(int id)
        {
            return await _estiloRepository.GetEstiloAsync(id);
        }
    }
}
