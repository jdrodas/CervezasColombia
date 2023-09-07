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
        public async Task<IEnumerable<Estilo>> GetAllAsync()
        {
            return await _estiloRepository.GetAllAsync();
        }

        public async Task<Estilo> GetByIdAsync(int id)
        {
            var unEstilo = await _estiloRepository.GetByIdAsync(id);

            if (unEstilo.Id ==0)
                throw new KeyNotFoundException("Estilo no encontrado");

            return unEstilo;
        }
    }
}
