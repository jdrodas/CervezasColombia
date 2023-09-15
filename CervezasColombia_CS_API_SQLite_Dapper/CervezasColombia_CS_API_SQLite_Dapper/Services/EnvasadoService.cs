using CervezasColombia_CS_API_SQLite_Dapper.Models;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;

namespace CervezasColombia_CS_API_SQLite_Dapper.Services
{
    public class EnvasadoService
    {
        private readonly IEnvasadoRepository _envasadoRepository;

        public EnvasadoService(IEnvasadoRepository envasadoRepository)
        {
            _envasadoRepository = envasadoRepository;
        }

        public async Task<IEnumerable<Envasado>> GetAllAsync()
        {
            return await _envasadoRepository.GetAllAsync();
        }

        public async Task<Envasado> GetByIdAsync(int id)
        {
            //Validamos que la Cerveceria exista con ese Id
            var unEnvasado = await _envasadoRepository.GetByIdAsync(id);

            if (unEnvasado.Id == 0)
                throw new AppValidationException($"Envasado no encontrado con el id {id}");

            return unEnvasado;
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int id)
        {
            //Validamos que la Cerveceria exista con ese Id
            var unEnvasado = await _envasadoRepository.GetByIdAsync(id);

            if (unEnvasado.Id == 0)
                throw new AppValidationException($"Envasado no encontrado con el id {id}");

            //Si la cerveceria existe, validamos que tenga cervezas asociadas
            var cantidadCervezasAsociadas = await _envasadoRepository.GetTotalAssociatedBeersAsync(id);

            if (cantidadCervezasAsociadas == 0)
                throw new AppValidationException($"No Existen cervezas asociadas al envasado {unEnvasado.Nombre}");

            return await _envasadoRepository.GetAssociatedBeersAsync(id);
        }
    }
}
