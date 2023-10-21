using CervezasColombia_CS_API_Mongo.DbContexts;
using CervezasColombia_CS_API_Mongo.Interfaces;
using CervezasColombia_CS_API_Mongo.Models;
using MongoDB.Driver;

namespace CervezasColombia_CS_API_Mongo.Repositories
{
    public class EnvasadoRepository : IEnvasadoRepository
    {
        private readonly MongoDbContext contextoDB;

        public EnvasadoRepository(MongoDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Envasado>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();
            var coleccionEnvasados = conexion.GetCollection<Envasado>("envasados");

            var losEnvasados = await coleccionEnvasados
                .Find(_ => true)
                .SortBy(envasado => envasado.Nombre)
                .ToListAsync();

            return losEnvasados;
        }

        public async Task<Envasado> GetByIdAsync(string envasado_id)
        {
            Envasado unEnvasado = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionEnvasados = conexion.GetCollection<Envasado>("envasados");

            var resultado = await coleccionEnvasados
                .Find(envasado => envasado.Id == envasado_id)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unEnvasado = resultado;

            return unEnvasado;
        }

        public async Task<Envasado> GetByNameAsync(string envasado_nombre)
        {
            Envasado unEnvasado = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionEnvasados = conexion.GetCollection<Envasado>("envasados");

            var resultado = await coleccionEnvasados
                .Find(envasado => envasado.Nombre == envasado_nombre)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unEnvasado = resultado;

            return unEnvasado;
        }

        public async Task<int> GetTotalAssociatedPackagedBeersAsync(string envasado_id)
        {
            var cervezasAsociadas = await GetAssociatedPackagedBeersAsync(envasado_id);

            return cervezasAsociadas.Count();
        }

        public async Task<IEnumerable<EnvasadoCerveza>> GetAssociatedPackagedBeersAsync(string envasado_id)
        {
            Envasado unEvasado = await GetByIdAsync(envasado_id);
            List<Cerveza> lasCervezas = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionEnvasadosCervezas = conexion.GetCollection<EnvasadoCerveza>("envasados_cervezas");

            var losEnvasadosCervezas = await coleccionEnvasadosCervezas
                .Find(envasado_cerveza => envasado_cerveza.Envasado == unEvasado.Nombre)
                .SortBy(envasado_cerveza => envasado_cerveza.Cerveza)
                .ToListAsync();
           
            return losEnvasadosCervezas;
        }

        public async Task<bool> CreateAsync(Envasado unEnvasado)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionEnvasados = conexion.GetCollection<Envasado>("envasados");

            await coleccionEnvasados
                .InsertOneAsync(unEnvasado);

            var resultado = await coleccionEnvasados
                .Find(envasado => envasado.Nombre == unEnvasado.Nombre)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Envasado unEnvasado)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionEnvasados = conexion.GetCollection<Envasado>("envasados");

            var resultado = await coleccionEnvasados
                .ReplaceOneAsync(envasado => envasado.Id == unEnvasado.Id, unEnvasado);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> DeleteAsync(Envasado unEnvasado)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionEnvasados = conexion.GetCollection<Envasado>("envasados");

            var resultado = await coleccionEnvasados
                .DeleteOneAsync(envasado => envasado.Id == unEnvasado.Id);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> DeleteAssociatedBeersAsync(string envasado_id)
        {
            bool resultadoAccion = false;

            var unEnvasado = await GetByIdAsync(envasado_id);

            var conexion = contextoDB.CreateConnection();
            var coleccionEnvasadosCervezas = conexion.GetCollection<EnvasadoCerveza>("envasados_cervezas");

            var resultado = await coleccionEnvasadosCervezas
                .DeleteManyAsync(envasadoCerveza => envasadoCerveza.Envasado == unEnvasado.Nombre);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }
    }
}
