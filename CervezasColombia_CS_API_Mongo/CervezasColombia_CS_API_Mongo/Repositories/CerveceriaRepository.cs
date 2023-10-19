using CervezasColombia_CS_API_Mongo.DbContexts;
using CervezasColombia_CS_API_Mongo.Helpers;
using CervezasColombia_CS_API_Mongo.Interfaces;
using CervezasColombia_CS_API_Mongo.Models;
using MongoDB.Driver;
using System.Data;

namespace CervezasColombia_CS_API_Mongo.Repositories
{
    public class CerveceriaRepository : ICerveceriaRepository
    {
        private readonly MongoDbContext contextoDB;

        public CerveceriaRepository(MongoDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Cerveceria>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();
            var coleccionCervecerias = conexion.GetCollection<Cerveceria>("cervecerias");

            var lasCervecerias = await coleccionCervecerias
                .Find(_ => true)
                .SortBy(cerveceria => cerveceria.Nombre)
                .ToListAsync();

            return lasCervecerias;
        }

        public async Task<Cerveceria> GetByIdAsync(string cerveceria_id)
        {
            Cerveceria unaCerveceria= new();

            var conexion = contextoDB.CreateConnection();
            var coleccionCervecerias = conexion.GetCollection<Cerveceria>("cervecerias");

            var resultado = await coleccionCervecerias
                .Find(cerveceria => cerveceria.Id == cerveceria_id)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaCerveceria = resultado;

            return unaCerveceria;
        }

        public async Task<CerveceriaDetallada> GetDetailsByIdAsync(string cerveceria_id)
        {
            CerveceriaDetallada unaCerveceriaDetallada = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionCervecerias = conexion.GetCollection<CerveceriaDetallada>("cervecerias");

            var resultado = await coleccionCervecerias
                .Find(cerveceria => cerveceria.Id == cerveceria_id)
                .FirstOrDefaultAsync();

            if (resultado is not null)
            {
                unaCerveceriaDetallada = resultado;

                //Aqui buscamos las cervezas asociadas
                var cervezasAsociadas = await GetAssociatedBeersAsync(unaCerveceriaDetallada.Id!);
                unaCerveceriaDetallada.Cervezas = cervezasAsociadas.ToList();
            }

            return unaCerveceriaDetallada;                       
        }

        public async Task<Cerveceria> GetByNameAsync(string cerveceria_nombre)
        {
            Cerveceria unaCerveceria = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionCervecerias = conexion.GetCollection<Cerveceria>("cervecerias");

            var resultado = await coleccionCervecerias
                .Find(cerveceria => cerveceria.Nombre == cerveceria_nombre)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaCerveceria = resultado;

            return unaCerveceria;
        }

        public async Task<Cerveceria> GetByInstagramAsync(string cerveceria_instagram)
        {
            Cerveceria unaCerveceria = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionCervecerias = conexion.GetCollection<Cerveceria>("cervecerias");

            var resultado = await coleccionCervecerias
                .Find(cerveceria => cerveceria.Instagram == cerveceria_instagram)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaCerveceria = resultado;

            return unaCerveceria;
        }

        public async Task<Cerveceria> GetBySitioWebAsync(string cerveceria_sitio_web)
        {
            Cerveceria unaCerveceria = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionCervecerias = conexion.GetCollection<Cerveceria>("cervecerias");

            var resultado = await coleccionCervecerias
                .Find(cerveceria => cerveceria.Sitio_Web == cerveceria_sitio_web)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaCerveceria = resultado;

            return unaCerveceria;
        }

        public async Task<int> GetTotalAssociatedBeersAsync(string cerveceria_id)
        {
            Cerveceria unaCerveceria = await GetByIdAsync(cerveceria_id);

            var conexion = contextoDB.CreateConnection();
            var coleccionCervezas = conexion.GetCollection<Cerveza>("cervezas");

            var lasCervezas = await coleccionCervezas
                .Find(cerveza => cerveza.Cerveceria == unaCerveceria.Nombre)
                .SortBy(cerveza => cerveza.Nombre)
                .ToListAsync();

            return lasCervezas.Count();
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(string cerveceria_id)
        {
            Cerveceria unaCerveceria = await GetByIdAsync(cerveceria_id);

            var conexion = contextoDB.CreateConnection();
            var coleccionCervezas = conexion.GetCollection<Cerveza>("cervezas");

            var lasCervezas = await coleccionCervezas
                .Find(cerveza => cerveza.Cerveceria == unaCerveceria.Nombre)
                .SortBy(cerveza => cerveza.Nombre)
                .ToListAsync();

            return lasCervezas;
        }

        public async Task<Ubicacion> GetAssociatedLocationAsync(string cerveceria_id)
        {
            Cerveceria unaCerveceria = await GetByIdAsync(cerveceria_id);

            string[] partesUbicacion = unaCerveceria.Ubicacion.Split(',');

            var conexion = contextoDB.CreateConnection();
            var coleccionUbicaciones = conexion.GetCollection<Ubicacion>("ubicaciones");

            var builder = Builders<Ubicacion>.Filter;
            var filtro = builder.And(
                builder.Eq(ubicacion => ubicacion.Municipio, partesUbicacion[0].Trim()),
                builder.Eq(ubicacion => ubicacion.Departamento, partesUbicacion[1].Trim()));

            var resultado = await coleccionUbicaciones
                .Find(filtro)
                .FirstOrDefaultAsync();

            Ubicacion unaUbicacion = new();

            if (resultado is not null)
                unaUbicacion = resultado;

            return unaUbicacion;
        }

        public async Task<bool> CreateAsync(Cerveceria unaCerveceria)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionCervecerias = conexion.GetCollection<Cerveceria>("cervecerias");

            await coleccionCervecerias
                .InsertOneAsync(unaCerveceria);

            var resultado = await GetByNameAsync(unaCerveceria.Nombre);

            if (resultado is not null)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Cerveceria unaCerveceria)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionCervecerias = conexion.GetCollection<Cerveceria>("cervecerias");

            var resultado = await coleccionCervecerias.ReplaceOneAsync(cerveceria => cerveceria.Id == unaCerveceria.Id, unaCerveceria);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> DeleteAsync(Cerveceria unaCerveceria)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionCervecerias = conexion.GetCollection<Cerveceria>("cervecerias");

            var resultado = await coleccionCervecerias
                .DeleteOneAsync(cerveceria => cerveceria.Id == unaCerveceria.Id);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> DeleteAssociatedBeersAsync(string cerveceria_id)
        {
            bool resultadoAccion = false;

            var unaCerveceria = await GetByIdAsync(cerveceria_id);

            var conexion = contextoDB.CreateConnection();
            var coleccionCervezas = conexion.GetCollection<Cerveza>("cervezas");

            var resultado = await coleccionCervezas
                .DeleteManyAsync(cerveza => cerveza.Cerveceria == unaCerveceria.Nombre);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }
    }
}
