﻿using CervezasColombia_CS_API_SQLite_Dapper.Cervezas;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace CervezasColombia_CS_API_SQLite_Dapper.Estilos
{
    public class EstiloRepository(SQLiteDbContext unContexto) : IEstiloRepository
    {
        private readonly SQLiteDbContext contextoDB = unContexto;

        public async Task<IEnumerable<Estilo>> GetAllAsync()
        {
            string sentenciaSQL = "SELECT id, nombre " +
                                  "FROM estilos " +
                                  "ORDER BY id DESC";

            var resultadoEstilos = await contextoDB.Conexion.QueryAsync<Estilo>(sentenciaSQL,
                                        new DynamicParameters());

            return resultadoEstilos;
        }

        public async Task<Estilo> GetByIdAsync(int estilo_id)
        {
            Estilo unEstilo = new();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@estilo_id", estilo_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre " +
                                  "FROM estilos " +
                                  "WHERE id = @estilo_id ";

            var resultado = await contextoDB.Conexion.QueryAsync<Estilo>(sentenciaSQL,
                                parametrosSentencia);

            if (resultado.Any())
                unEstilo = resultado.First();

            return unEstilo;
        }

        public async Task<EstiloDetallado> GetDetailsByIdAsync(int estilo_id)
        {
            EstiloDetallado unEstilo = new();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@estilo_id", estilo_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre " +
                                  "FROM estilos " +
                                  "WHERE id = @estilo_id ";

            var resultado = await contextoDB.Conexion.QueryAsync<EstiloDetallado>(sentenciaSQL,
                                parametrosSentencia);

            if (resultado.Any())
            {
                unEstilo = resultado.First();

                var lasCervezas = await GetAssociatedBeersAsync(unEstilo.Id);
                unEstilo.Cervezas = lasCervezas.ToList();
            }

            return unEstilo;
        }

        public async Task<Estilo> GetByNameAsync(string estilo_nombre)
        {
            Estilo unEstilo = new();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@estilo_nombre", estilo_nombre,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre " +
                                  "FROM estilos " +
                                  "WHERE LOWER(nombre) = LOWER(@estilo_nombre) ";

            var resultado = await contextoDB.Conexion.QueryAsync<Estilo>(sentenciaSQL,
                                parametrosSentencia);

            if (resultado.Any())
                unEstilo = resultado.First();

            return unEstilo;
        }

        public async Task<int> GetTotalAssociatedBeersAsync(int estilo_id)
        {
            var lasCervezas = await GetAssociatedBeersAsync(estilo_id);

            return lasCervezas.ToList().Count;

        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int estilo_id)
        {
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@estilo_id", estilo_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT cerveza_id id, cerveza nombre, cerveceria, cerveceria_id, estilo, estilo_id, " +
                                    "abv, rango_abv " +
                                    "FROM v_info_cervezas " +
                                    "WHERE estilo_id = @estilo_id " +
                                    "ORDER BY cerveza_id DESC";

            var resultadoCervezas = await contextoDB.Conexion.QueryAsync<Cerveza>(sentenciaSQL,
                                        parametrosSentencia);

            return resultadoCervezas;
        }

        public async Task<bool> CreateAsync(Estilo unEstilo)
        {
            bool resultadoAccion = false;

            try
            {
                string sentenciaSQL = "INSERT INTO estilos (nombre) " +
                                        "VALUES (@Nombre)";

                int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL,
                                        unEstilo);

                if (filasAfectadas > 0)
                    resultadoAccion = true;
            }
            catch (SqliteException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Estilo unEstilo)
        {
            bool resultadoAccion = false;

            try
            {
                string sentenciaSQL = "UPDATE estilos SET nombre = @Nombre " +
                                        "WHERE id = @Id";

                int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL,
                                        unEstilo);

                if (filasAfectadas > 0)
                    resultadoAccion = true;
            }
            catch (SqliteException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }


        public async Task<bool> DeleteAsync(Estilo unEstilo)
        {
            bool resultadoAccion = false;

            try
            {
                string sentenciaSQL = "DELETE FROM estilos WHERE id = @Id";

                int filasAfectadas = await contextoDB.Conexion.ExecuteAsync(sentenciaSQL,
                                        unEstilo);

                if (filasAfectadas > 0)
                    resultadoAccion = true;
            }
            catch (SqliteException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }
    }
}