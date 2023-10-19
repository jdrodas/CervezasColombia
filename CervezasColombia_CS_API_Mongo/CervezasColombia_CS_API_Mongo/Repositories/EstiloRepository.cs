using CervezasColombia_CS_API_Mongo.DbContexts;
using CervezasColombia_CS_API_Mongo.Interfaces;
using CervezasColombia_CS_API_Mongo.Models;
using MongoDB.Driver;

namespace CervezasColombia_CS_API_Mongo.Repositories
{
    public class EstiloRepository : IEstiloRepository
    {
        private readonly MongoDbContext contextoDB;

        public EstiloRepository(MongoDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Estilo>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();
            var coleccionEstilos = conexion.GetCollection<Estilo>("estilos");

            var losEstilos = await coleccionEstilos
                .Find(_ => true)
                .SortBy(estilo => estilo.Nombre)
                .ToListAsync();

            return losEstilos;
        }

        public async Task<Estilo> GetByIdAsync(string estilo_id)
        {
            Estilo unEstilo = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionEstilos = conexion.GetCollection<Estilo>("estilos");

            var resultado = await coleccionEstilos
                .Find(estilo => estilo.Id == estilo_id)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unEstilo = resultado;

            return unEstilo;
        }

        public async Task<EstiloDetallado> GetDetailsByIdAsync(string estilo_id)
        {
            EstiloDetallado unEstilo = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionEstilos = conexion.GetCollection<EstiloDetallado>("estilos");

            var resultado = await coleccionEstilos
                .Find(estilo => estilo.Id == estilo_id)
                .FirstOrDefaultAsync();

            if (resultado is not null)
            {
                unEstilo = resultado;
                var lasCervezas = await GetAssociatedBeersAsync(estilo_id);
                unEstilo.Cervezas = lasCervezas.ToList();
            }

            return unEstilo;
        }

        public async Task<Estilo> GetByNameAsync(string estilo_nombre)
        {
            Estilo unEstilo = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionEstilos = conexion.GetCollection<Estilo>("estilos");

            var resultado = await coleccionEstilos
                .Find(estilo => estilo.Nombre == estilo_nombre)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unEstilo = resultado;

            return unEstilo;
        }

        public async Task<int> GetTotalAssociatedBeersAsync(string estilo_id)
        {
            var unEstilo = await GetByIdAsync(estilo_id);

            var conexion = contextoDB.CreateConnection();
            var coleccionCervezas = conexion.GetCollection<Cerveza>("cervezas");

            var lasCervezas = await coleccionCervezas
                .Find(cerveza => cerveza.Estilo == unEstilo.Nombre)
                .ToListAsync();

            int totalCervezas = lasCervezas.Count();

            return totalCervezas;
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(string estilo_id)
        {
            var unEstilo = await GetByIdAsync(estilo_id);

            var conexion = contextoDB.CreateConnection();
            var coleccionCervezas = conexion.GetCollection<Cerveza>("cervezas");

            var lasCervezas = await coleccionCervezas
                .Find(cerveza => cerveza.Estilo == unEstilo.Nombre)
                .ToListAsync();

            return lasCervezas;
        }

        public async Task<bool> CreateAsync(Estilo unEstilo)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionEstilos = conexion.GetCollection<Estilo>("estilos");

            await coleccionEstilos
                .InsertOneAsync(unEstilo);

            var resultado = await coleccionEstilos
                .Find(estilo => estilo.Nombre == unEstilo.Nombre)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Estilo unEstilo)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionEstilos = conexion.GetCollection<Estilo>("estilos");

            var resultado = await coleccionEstilos.ReplaceOneAsync(estilo => estilo.Id == unEstilo.Id, unEstilo);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }


        public async Task<bool> DeleteAsync(Estilo unEstilo)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionEstilos = conexion.GetCollection<Estilo>("estilos");

            var resultado = await coleccionEstilos
                .DeleteOneAsync(estilo => estilo.Id == unEstilo.Id);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }
    }
}