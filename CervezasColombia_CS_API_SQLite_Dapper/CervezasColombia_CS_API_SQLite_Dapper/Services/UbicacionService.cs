using CervezasColombia_CS_API_SQLite_Dapper.Models;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;
using CervezasColombia_CS_API_SQLite_Dapper.Repositories;

namespace CervezasColombia_CS_API_SQLite_Dapper.Services
{
    public class UbicacionService
    {
        private readonly IUbicacionRepository _ubicacionRepository;

        public UbicacionService(IUbicacionRepository ubicacionRepository)
        {
            _ubicacionRepository = ubicacionRepository;
        }

        public async Task<IEnumerable<Ubicacion>> GetAllAsync()
        {
            return await _ubicacionRepository.GetAllAsync();
        }

        public async Task<Ubicacion> GetByIdAsync(int id)
        {
            //Validamos que el estilo exista con ese Id
            var unaUbicacion = await _ubicacionRepository.GetByIdAsync(id);

            if (unaUbicacion.Id == 0)
                throw new AppValidationException($"Ubicacion no encontrada con el id {id}");

            return unaUbicacion;
        }

        public async Task<IEnumerable<Cerveceria>> GetAssociatedBreweriesAsync(int id)
        {
            //Validamos que la ubicacion exista con ese Id
            var unaUbicacion = await _ubicacionRepository.GetByIdAsync(id);

            if (unaUbicacion.Id == 0)
                throw new AppValidationException($"Ubicación no encontrada con el id {id}");

            //Si la ubicacion existe, validamos que tenga cervecerias asociadas
            var cantidadCerveceriasAsociadas = await _ubicacionRepository.GetTotalAssociatedBreweriesAsync(id);

            if (cantidadCerveceriasAsociadas == 0)
                throw new AppValidationException($"No Existen cervecerias asociadas a la ubicación {unaUbicacion.Municipio}, {unaUbicacion.Departamento}");

            return await _ubicacionRepository.GetAssociatedBreweriesAsync(id);
        }
    }
}