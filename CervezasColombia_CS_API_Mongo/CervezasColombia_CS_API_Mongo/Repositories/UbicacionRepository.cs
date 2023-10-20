using CervezasColombia_CS_API_Mongo.DbContexts;
using CervezasColombia_CS_API_Mongo.Interfaces;
using CervezasColombia_CS_API_Mongo.Models;
using MongoDB.Driver;

namespace CervezasColombia_CS_API_Mongo.Repositories
{
    public class UbicacionRepository : IUbicacionRepository
    {
        private readonly MongoDbContext contextoDB;

        public UbicacionRepository(MongoDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Ubicacion>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();
            var coleccionUbicaciones = conexion.GetCollection<Ubicacion>("ubicaciones");

            var lasUbicaciones = await coleccionUbicaciones
                .Find(_ => true)
                .SortBy(ubicacion => ubicacion.Departamento)
                .ToListAsync();

            return lasUbicaciones;
        }


        public async Task<Ubicacion> GetByIdAsync(string ubicacion_id)
        {
            Ubicacion unaUbicacion = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionUbicaciones = conexion.GetCollection<Ubicacion>("ubicaciones");

            var resultado = await coleccionUbicaciones
                .Find(ubicacion => ubicacion.Id == ubicacion_id)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaUbicacion = resultado;

            return unaUbicacion;
        }

        public async Task<Ubicacion> GetByNameAsync(string ubicacion_municipio, string ubicacion_departamento)
        {
            Ubicacion unaUbicacion = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionUbicaciones = conexion.GetCollection<Ubicacion>("ubicaciones");

            var builder = Builders<Ubicacion>.Filter;
            var filtro = builder.And(
                builder.Eq(ubicacion => ubicacion.Municipio, ubicacion_municipio),
                builder.Eq(ubicacion => ubicacion.Departamento, ubicacion_departamento));

            var resultado = await coleccionUbicaciones
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaUbicacion = resultado;

            return unaUbicacion;
        }

        public async Task<Ubicacion> GetByNameAsync(string ubicacion_nombre)
        {
            Ubicacion unaUbicacion = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionUbicaciones = conexion.GetCollection<Ubicacion>("ubicaciones");

            string[] partesUbicacion = ubicacion_nombre.Split(',');

            var builder = Builders<Ubicacion>.Filter;
            var filtro = builder.And(
                builder.Eq(ubicacion => ubicacion.Municipio, partesUbicacion[0].Trim()),
                builder.Eq(ubicacion => ubicacion.Departamento, partesUbicacion[1].Trim()));

            var resultado = await coleccionUbicaciones
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaUbicacion = resultado;

            return unaUbicacion;
        }

        public async Task<int> GetTotalAssociatedBreweriesAsync(string ubicacion_id)
        {
            var lasCervecerias = await GetAssociatedBreweriesAsync(ubicacion_id);

            return lasCervecerias.Count();
        }

        public async Task<IEnumerable<Cerveceria>> GetAssociatedBreweriesAsync(string ubicacion_id)
        {
            var unaUbicacion = await GetByIdAsync(ubicacion_id);
            string ubicacionConsolidada = unaUbicacion.Municipio + ", " + unaUbicacion.Departamento;

            var conexion = contextoDB.CreateConnection();
            var coleccionCervecerias = conexion.GetCollection<Cerveceria>("cervecerias");

            var lasCervecerias = await coleccionCervecerias
                .Find(cerveceria => cerveceria.Ubicacion == ubicacionConsolidada)
                .ToListAsync();

            return lasCervecerias;
        }

        public async Task<bool> CreateAsync(Ubicacion unaUbicacion)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionUbicaciones = conexion.GetCollection<Ubicacion>("ubicaciones");

            await coleccionUbicaciones
                .InsertOneAsync(unaUbicacion);

            var resultado = await GetByNameAsync(unaUbicacion.Municipio, unaUbicacion.Departamento);

            if (resultado is not null)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Ubicacion unaUbicacion)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionUbicaciones = conexion.GetCollection<Ubicacion>("ubicaciones");

            var resultado = await coleccionUbicaciones.ReplaceOneAsync(ubicacion => ubicacion.Id == unaUbicacion.Id, unaUbicacion);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> DeleteAsync(Ubicacion unaUbicacion)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionUbicaciones = conexion.GetCollection<Ubicacion>("ubicaciones");

            var resultado = await coleccionUbicaciones
                .DeleteOneAsync(ubicacion => ubicacion.Id == unaUbicacion.Id);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }
    }
}