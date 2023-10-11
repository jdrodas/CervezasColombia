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

        //public async Task<int> GetTotalAssociatedBeersAsync(int envasado_id)
        //{
        //    var conexion = contextoDB.CreateConnection();

        //    DynamicParameters parametrosSentencia = new();
        //    parametrosSentencia.Add("@envasado_id", envasado_id,
        //                            DbType.Int32, ParameterDirection.Input);

        //    string sentenciaSQL = "SELECT COUNT(cerveza_id) totalCervezas " +
        //                            "FROM v_info_envasados_cervezas v " +
        //                            "WHERE envasado_id = @envasado_id ";

        //    var totalCervezas = await conexion.QueryFirstAsync<int>(sentenciaSQL,
        //                            parametrosSentencia);

        //    return totalCervezas;
        //}

        //public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int envasado_id)
        //{
        //    var conexion = contextoDB.CreateConnection();

        //    DynamicParameters parametrosSentencia = new();
        //    parametrosSentencia.Add("@envasado_id", envasado_id,
        //                            DbType.Int32, ParameterDirection.Input);

        //    string sentenciaSQL = "SELECT vc.cerveza_id id, vc.cerveza nombre, vc.cerveceria, " +
        //                            "vc.estilo, vc.ibu, vc.abv, vc.rango_ibu, vc.rango_abv " +
        //                            "FROM v_info_cervezas vc " +
        //                            "JOIN v_info_envasados_cervezas ve ON vc.cerveza_id = ve.cerveza_id " +
        //                            "WHERE ve.envasado_id = @envasado_id " +
        //                            "ORDER BY vc.cerveza_id DESC";

        //    var resultadoCervezas = await conexion.QueryAsync<Cerveza>(sentenciaSQL, parametrosSentencia);

        //    return resultadoCervezas;
        //}

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
