using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;
using CervezasColombia_CS_API_SQLite_Dapper.Models;

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
            return await _envasadoRepository
                .GetAllAsync();
        }

        public async Task<Envasado> GetByIdAsync(int envasado_id)
        {
            //Validamos que la Cerveceria exista con ese Id
            var unEnvasado = await _envasadoRepository
                .GetByIdAsync(envasado_id);

            if (unEnvasado.Id == 0)
                throw new AppValidationException($"Envasado no encontrado con el id {envasado_id}");

            return unEnvasado;
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int envasado_id)
        {
            //Validamos que la Cerveceria exista con ese Id
            var unEnvasado = await _envasadoRepository
                .GetByIdAsync(envasado_id);

            if (unEnvasado.Id == 0)
                throw new AppValidationException($"Envasado no encontrado con el id {envasado_id}");

            //Si la cerveceria existe, validamos que tenga cervezas asociadas
            var cantidadCervezasAsociadas = await _envasadoRepository
                .GetTotalAssociatedBeersAsync(envasado_id);

            if (cantidadCervezasAsociadas == 0)
                throw new AppValidationException($"No Existen cervezas asociadas al envasado {unEnvasado.Nombre}");

            return await _envasadoRepository
                .GetAssociatedBeersAsync(envasado_id);
        }
    }
}
