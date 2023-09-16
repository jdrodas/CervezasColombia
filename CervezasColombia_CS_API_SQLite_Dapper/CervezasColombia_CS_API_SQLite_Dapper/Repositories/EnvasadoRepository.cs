﻿using Dapper;
using System.Data;
using Microsoft.Data.Sqlite;
using CervezasColombia_CS_API_SQLite_Dapper.DbContexts;
using CervezasColombia_CS_API_SQLite_Dapper.Models;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CervezasColombia_CS_API_SQLite_Dapper.Repositories
{
    public class EnvasadoRepository : IEnvasadoRepository
    {
        private readonly SQLiteDbContext contextoDB;

        public EnvasadoRepository(SQLiteDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Envasado>> GetAllAsync()
        {
            using (contextoDB.Conexion)
            {
                string sentenciaSQL = 
                    "SELECT DISTINCT  e.id, " +
                    "e.nombre, v.unidad_volumen, v.volumen " +
                    "FROM v_info_envasados_cervezas v " +
                    "RIGHT JOIN envasados e ON v.envasado_id = e.id " +
                    "order by nombre ";

                var resultadoEnvasados = await contextoDB.Conexion.QueryAsync<Envasado>(sentenciaSQL,
                                        new DynamicParameters());

                return resultadoEnvasados;
            }
        }

        public async Task<Envasado> GetByIdAsync(int id)
        {
            Envasado unEvasado = new Envasado();

            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@envasado_id", id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL =
                    "SELECT DISTINCT  e.id, e.nombre " +
                    "FROM envasados e " +
                    "WHERE e.id = @envasado_id ";

                var resultado = await contextoDB.Conexion.QueryAsync<Envasado>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.ToArray().Length > 0)
                    unEvasado = resultado.First();
            }

            return unEvasado;
        }

        public async Task<int> GetTotalAssociatedBeersAsync(int id)
        {

            DynamicParameters parametrosSentencia = new DynamicParameters();
            parametrosSentencia.Add("@envasado_id", id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT COUNT(cerveza_id) totalCervezas " +
                "FROM v_info_envasados_cervezas v " +
                "WHERE envasado_id = @envasado_id ";

            var totalCervezas = await contextoDB.Conexion.QueryAsync<int>(sentenciaSQL,
                                    parametrosSentencia);

            return totalCervezas.First();
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int id)
        {
            using (contextoDB.Conexion)
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@envasado_id", id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT vc.cerveza_id id, vc.cerveza nombre, vc.cerveceria, " +
                    "vc.estilo, vc.ibu, vc.abv, vc.rango_ibu, vc.rango_abv " +
                    "FROM v_info_cervezas vc " +
                    "JOIN v_info_envasados_cervezas ve ON vc.cerveza_id = ve.cerveza_id " +
                    "WHERE ve.envasado_id = @envasado_id " +
                    "ORDER BY vc.cerveceria, vc.cerveza";

                var resultadoCervezas = await contextoDB.Conexion.QueryAsync<Cerveza>(sentenciaSQL, parametrosSentencia);

                return resultadoCervezas;
            }
        }
    }
}