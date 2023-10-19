using CervezasColombia_CS_API_Mongo.DbContexts;
using CervezasColombia_CS_API_Mongo.Helpers;
using CervezasColombia_CS_API_Mongo.Interfaces;
using CervezasColombia_CS_API_Mongo.Models;
using MongoDB.Driver;
using System.Data;

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

        public async Task<int> GetTotalAssociatedBeersAsync(string envasado_id)
        {
            var cervezasAsociadas = await GetAssociatedBeersAsync(envasado_id);

            return cervezasAsociadas.Count();
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(string envasado_id)
        {
            Envasado unEvasado = await GetByIdAsync(envasado_id);
            List<Cerveza> lasCervezas = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionEnvasadosCervezas = conexion.GetCollection<EnvasadoCerveza>("envasados_cervezas");

            var losEnvasadosCervezas = await coleccionEnvasadosCervezas
                .Find(envasado_cerveza => envasado_cerveza.Envasado == unEvasado.Nombre)
                .SortBy(envasado_cerveza => envasado_cerveza.Cerveza)
                .ToListAsync();

            //Creamos una lista de cervezas y la llenamos con el detalle de las cervezas
            if (losEnvasadosCervezas.Any())
            {
                var coleccionCervezas = conexion.GetCollection<Cerveza>("cervezas");

                foreach (EnvasadoCerveza unEnvasadoCerveza in losEnvasadosCervezas)
                {
                    var builder = Builders<Cerveza>.Filter;
                    var filtro = builder.And(
                        builder.Eq(cerveza => cerveza.Nombre, unEnvasadoCerveza.Cerveza),
                        builder.Eq(cerveza => cerveza.Cerveceria, unEnvasadoCerveza.Cerveceria));


                    var unaCerveza = await coleccionCervezas
                        .Find(filtro)
                        .FirstOrDefaultAsync();

                    lasCervezas.Add(unaCerveza);
                }
            }

            return lasCervezas;
        }

        //TODO: EnvasadoRepository: Obtener envasados asociados

        //public async Task<EnvasadoCerveza> GetAssociatedBeerPackagingAsync(int cerveza_id, int envasado_id, int unidad_volumen_id, float volumen)
        //{
        //    EnvasadoCerveza unEnvasadoCerveza = new();

        //    var conexion = contextoDB.CreateConnection();

        //    DynamicParameters parametrosSentencia = new();
        //    parametrosSentencia.Add("@cerveza_id", cerveza_id,
        //                            DbType.Int32, ParameterDirection.Input);
        //    parametrosSentencia.Add("@envasado_id", envasado_id,
        //                            DbType.Int32, ParameterDirection.Input);
        //    parametrosSentencia.Add("@unidad_volumen_id", unidad_volumen_id,
        //                            DbType.Int32, ParameterDirection.Input);
        //    parametrosSentencia.Add("@volumen", volumen,
        //                            DbType.Single, ParameterDirection.Input);

        //    string sentenciaSQL = "SELECT v.envasado_id id, v.envasado nombre, v.unidad_volumen_id, unidad_volumen, volumen " +
        //                            "FROM v_info_envasados_cervezas v " +
        //                            "WHERE v.envasado_id = @envasado_id " +
        //                            "AND v.cerveza_id = @cerveza_id " +
        //                            "AND v.unidad_volumen_id = @unidad_volumen_id " +
        //                            "AND v.volumen = @volumen";

        //    var resultado = await conexion.QueryAsync<EnvasadoCerveza>(sentenciaSQL, parametrosSentencia);

        //    if (resultado.Any())
        //        unEnvasadoCerveza = resultado.First();

        //    return unEnvasadoCerveza;
        //}

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
    }
}
