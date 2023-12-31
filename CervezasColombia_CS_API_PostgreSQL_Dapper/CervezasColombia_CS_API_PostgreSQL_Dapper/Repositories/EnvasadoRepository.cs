﻿using CervezasColombia_CS_API_PostgreSQL_Dapper.DbContexts;
using CervezasColombia_CS_API_PostgreSQL_Dapper.Helpers;
using CervezasColombia_CS_API_PostgreSQL_Dapper.Interfaces;
using CervezasColombia_CS_API_PostgreSQL_Dapper.Models;
using Dapper;
using Npgsql;
using System.Data;

namespace CervezasColombia_CS_API_PostgreSQL_Dapper.Repositories
{
    public class EnvasadoRepository : IEnvasadoRepository
    {
        private readonly PgsqlDbContext contextoDB;

        public EnvasadoRepository(PgsqlDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Envasado>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT DISTINCT  e.id, e.nombre " +
                        "FROM envasados e " +
                        "ORDER BY e.id DESC ";

            var resultadoEnvasados = await conexion.QueryAsync<Envasado>(sentenciaSQL,
                                    new DynamicParameters());

            return resultadoEnvasados;
        }

        public async Task<Envasado> GetByIdAsync(int envasado_id)
        {
            Envasado unEnvasado = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@envasado_id", envasado_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT  e.id, e.nombre " +
                                    "FROM envasados e " +
                                    "WHERE e.id = @envasado_id ";

            var resultado = await conexion.QueryAsync<Envasado>(sentenciaSQL,
                parametrosSentencia);

            if (resultado.Any())
                unEnvasado = resultado.First();

            return unEnvasado;
        }

        public async Task<Envasado> GetByNameAsync(string envasado_nombre)
        {
            Envasado unEnvasado = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@envasado_nombre", envasado_nombre,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre " +
                                  "FROM envasados " +
                                  "WHERE LOWER(nombre) = LOWER(@envasado_nombre) ";

            var resultado = await conexion.QueryAsync<Envasado>(sentenciaSQL,
                                parametrosSentencia);

            if (resultado.Any())
                unEnvasado = resultado.First();

            return unEnvasado;
        }

        public async Task<int> GetTotalAssociatedBeersAsync(int envasado_id)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@envasado_id", envasado_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT COUNT(cerveza_id) totalCervezas " +
                                    "FROM v_info_envasados_cervezas v " +
                                    "WHERE envasado_id = @envasado_id ";

            var totalCervezas = await conexion.QueryFirstAsync<int>(sentenciaSQL,
                                    parametrosSentencia);

            return totalCervezas;
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int envasado_id)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@envasado_id", envasado_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT vc.cerveza_id id, vc.cerveza nombre, vc.cerveceria, " +
                                    "vc.estilo, vc.ibu, vc.abv, vc.rango_ibu, vc.rango_abv " +
                                    "FROM v_info_cervezas vc " +
                                    "JOIN v_info_envasados_cervezas ve ON vc.cerveza_id = ve.cerveza_id " +
                                    "WHERE ve.envasado_id = @envasado_id " +
                                    "ORDER BY vc.cerveza_id DESC";

            var resultadoCervezas = await conexion.QueryAsync<Cerveza>(sentenciaSQL, parametrosSentencia);

            return resultadoCervezas;
        }

        public async Task<EnvasadoCerveza> GetAssociatedBeerPackagingAsync(int cerveza_id, int envasado_id, int unidad_volumen_id, float volumen)
        {
            EnvasadoCerveza unEnvasadoCerveza = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@cerveza_id", cerveza_id,
                                    DbType.Int32, ParameterDirection.Input);
            parametrosSentencia.Add("@envasado_id", envasado_id,
                                    DbType.Int32, ParameterDirection.Input);
            parametrosSentencia.Add("@unidad_volumen_id", unidad_volumen_id,
                                    DbType.Int32, ParameterDirection.Input);
            parametrosSentencia.Add("@volumen", volumen,
                                    DbType.Single, ParameterDirection.Input);

            string sentenciaSQL = "SELECT v.envasado_id id, v.envasado nombre, v.unidad_volumen_id, unidad_volumen, volumen " +
                                    "FROM v_info_envasados_cervezas v " +
                                    "WHERE v.envasado_id = @envasado_id " +
                                    "AND v.cerveza_id = @cerveza_id " +
                                    "AND v.unidad_volumen_id = @unidad_volumen_id " +
                                    "AND v.volumen = @volumen";

            var resultado = await conexion.QueryAsync<EnvasadoCerveza>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unEnvasadoCerveza = resultado.First();

            return unEnvasadoCerveza;
        }

        public async Task<bool> CreateAsync(Envasado unEnvasado)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_inserta_envasado";
                var parametros = new
                {
                    p_nombre = unEnvasado.Nombre
                };

                var cantidad_filas = await conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
                    resultadoAccion = true;
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Envasado unEnvasado)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_actualiza_envasado";
                var parametros = new
                {
                    p_id = unEnvasado.Id,
                    p_nombre = unEnvasado.Nombre
                };

                var cantidad_filas = await conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
                    resultadoAccion = true;
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }


        public async Task<bool> DeleteAsync(Envasado unEnvasado)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_elimina_envasado";
                var parametros = new
                {
                    p_id = unEnvasado.Id
                };

                var cantidad_filas = await conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
                    resultadoAccion = true;
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }
    }
}
