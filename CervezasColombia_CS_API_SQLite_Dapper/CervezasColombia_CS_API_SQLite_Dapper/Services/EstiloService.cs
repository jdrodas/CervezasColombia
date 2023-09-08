using CervezasColombia_CS_API_SQLite_Dapper.Data.Entities;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
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

        public async Task CreateAsync(Estilo unEstilo)
        {
            // validamos que el estilo a crear no esté previamente creado
            var estiloExistente = await _estiloRepository.GetByNameAsync(unEstilo.Nombre!);

            if(estiloExistente.Id !=0)
                throw new AppValidationException($"Ya existe un estilo con el nombre {unEstilo.Nombre}");

            try
            {
                await _estiloRepository.CreateAsync(unEstilo);
            }
            catch (AppValidationException error)
            {
                throw error;
            }            
        }
    }
}
