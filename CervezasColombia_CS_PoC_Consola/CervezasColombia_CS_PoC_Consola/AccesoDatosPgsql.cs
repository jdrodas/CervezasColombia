using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using Npgsql;

namespace CervezasColombia_CS_PoC_Consola
{
    public class AccesoDatosPgsql
    {
        public static string? ObtieneCadenaConexion()
        {
            //Parametrizamos el acceso al archivo de configuración appsettings.json
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration miConfiguracion = builder.Build();

            return miConfiguracion["ConnectionString:Pgsql"];
        }


        #region Estilos

        /// <summary>
        /// Obtiene la lista con los nombres de los estilos de cerveza
        /// </summary>
        /// <returns>Lista con los nombres de los estilos</returns>
        public static List<string> ObtieneNombresEstilosCerveza()
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            using (IDbConnection cxnDB = new NpgsqlConnection(cadenaConexion))
            {
                string sentenciaSQL = "SELECT nombre FROM estilos ORDER BY nombre";
                var resultadoEstilos = cxnDB.Query<string>(sentenciaSQL, new DynamicParameters());

                return resultadoEstilos.AsList();
            }
        }

        /// <summary>
        /// Obtiene la lista con los estilos de cerveza
        /// </summary>
        /// <returns></returns>
        public static List<Estilo> ObtieneEstilosCerveza()
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            using (IDbConnection cxnDB = new NpgsqlConnection(cadenaConexion))
            {
                string sentenciaSQL = "SELECT id, nombre FROM estilos ORDER BY nombre";
                var resultadoEstilos = cxnDB.Query<Estilo>(sentenciaSQL, new DynamicParameters());

                return resultadoEstilos.AsList();
            }
        }

        /// <summary>
        /// Obtiene un Estilo de Cerveza de acuerdo al nombre
        /// </summary>
        /// <param name="nombreEstilo">Nombre del estilo a buscar</param>
        /// <returns>El estilo identificado según el parámetro</returns>
        public static Estilo ObtieneEstiloCerveza(string nombreEstilo)
        {
            Estilo estiloResultado = new Estilo();
            string? cadenaConexion = ObtieneCadenaConexion();

            //Aqui buscamos el estilo asociado al nombre
            using (IDbConnection cxnDB = new NpgsqlConnection(cadenaConexion))
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@nombre_estilo", nombreEstilo,
                                        DbType.String, ParameterDirection.Input);

                string? sentenciaSQL = "SELECT id,nombre " +
                                        "FROM estilos " +
                                        "WHERE nombre = @nombre_estilo";

                var salida = cxnDB.Query<Estilo>(sentenciaSQL, parametrosSentencia);

                if (salida.ToArray().Length > 0)
                    estiloResultado = salida.First();

                return estiloResultado;
            }
        }

        /// <summary>
        /// Obtiene un Estilo de Cerveza de acuerdo al Id
        /// </summary>
        /// <param name="idEstilo">Id del estilo a buscar</param>
        /// <returns>El estilo identificado según el parámetro</returns>
        public static Estilo ObtieneEstiloCerveza(int idEstilo)
        {
            Estilo estiloResultado = new Estilo();
            string? cadenaConexion = ObtieneCadenaConexion();

            //Aqui buscamos el estilo asociado al nombre
            using (IDbConnection cxnDB = new NpgsqlConnection(cadenaConexion))
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@id_estilo", idEstilo,
                                        DbType.Int32, ParameterDirection.Input);

                string? sentenciaSQL = "SELECT id,nombre " +
                                        "FROM estilos " +
                                        "WHERE id = @id_estilo";

                var salida = cxnDB.Query<Estilo>(sentenciaSQL, parametrosSentencia);

                if (salida.ToArray().Length > 0)
                    estiloResultado = salida.First();

                return estiloResultado;
            }
        }

        /// <summary>
        /// Inserta un nuevo estilo de cerveza
        /// </summary>
        /// <param name="unEstilo">El estilo al insertar</param>
        /// <returns>Verdadero si la inserción se hizo correctamente</returns>
        public static bool InsertaEstiloCerveza(Estilo unEstilo)
        {
            //Validaciones previas: 
            // - Que el estilo nuevo no exista previamente

            int cantidadFilas = 0;
            bool resultado = false;
            string? cadenaConexion = ObtieneCadenaConexion();

            using (IDbConnection cxnDB = new NpgsqlConnection(cadenaConexion))
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@nombre_estilo", unEstilo.Nombre,
                    DbType.String, ParameterDirection.Input);

                //Preguntamos si ya existe un estilo con ese nombre
                string consultaSQL = "SELECT COUNT(id) total " +
                                           "FROM estilos " +
                                           "WHERE LOWER(nombre) = LOWER(@nombre_estilo)";

                cantidadFilas = cxnDB.Query<int>(consultaSQL, parametrosSentencia).FirstOrDefault();

                // Si hay filas, ya existe un estilo con ese nombre
                if (cantidadFilas != 0)
                    return false;

                try
                {
                    string insertaEstiloSQL = "INSERT INTO estilos (nombre) " +
                                               "VALUES (@nombre_estilo)";

                    cantidadFilas = cxnDB.Execute(insertaEstiloSQL, parametrosSentencia);
                }
                catch (NpgsqlException)
                {
                    resultado = false;
                    cantidadFilas = 0;
                }

                //Si la inserción fue correcta, se afectaron filas y podemos retornar true.
                if (cantidadFilas > 0)
                    resultado = true;

            }

            return resultado;
        }

        /// <summary>
        /// Actualiza el nombre en estilo de cerveza existente
        /// </summary>
        /// <param name="estiloActualizado">El objeto estilo para actualizar</param>
        /// <returns>Verdadero si la actualización se hizo correctamente</returns>
        public static bool ActualizaEstiloCerveza(Estilo estiloActualizado)
        {
            //Validaciones previas: 
            // - Que el estilo a actualizar exista - Busqueda por ID
            // - Que el nombre nuevo no exista previamente

            int cantidadFilas = 0;
            bool resultado = false;
            string? cadenaConexion = ObtieneCadenaConexion();


            using (IDbConnection cxnDB = new NpgsqlConnection(cadenaConexion))
            {
                //Aqui validamos primero que el Estilo previamente existe

                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@estilo_id", estiloActualizado.Id,
                                        DbType.Int32, ParameterDirection.Input);

                string consultaSQL = "SELECT COUNT(id) total " +
                                     "FROM estilos " +
                                     "WHERE id = @estilo_id";

                cantidadFilas = cxnDB.Query<int>(consultaSQL, parametrosSentencia).FirstOrDefault();

                //Si no hay filas, no existe estilo que actualizar
                if (cantidadFilas == 0)
                    return false;

                //Aqui validamos que no exista estilos con el nuevo nombre
                parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@estilo_nombre", estiloActualizado.Nombre,
                                        DbType.String, ParameterDirection.Input);

                //Validamos si el nuevo nombre no exista
                consultaSQL = "SELECT COUNT(id) total " +
                              "FROM estilos " +
                              "WHERE nombre = @estilo_nombre";

                cantidadFilas = cxnDB.Query<int>(consultaSQL, parametrosSentencia).FirstOrDefault();

                //Si hay filas, el nuevo nombre a utilizar ya existe!
                if (cantidadFilas != 0)
                    return false;

                //Terminadas las validaciones, realizamos el update
                try
                {
                    string actualizaEstiloSql = "UPDATE estilos SET nombre = @Nombre " +
                        "WHERE id = @Id"; ;

                    //Aqui no usamos parámetros dinámicos, pasamos el objeto!!!
                    cantidadFilas = cxnDB.Execute(actualizaEstiloSql, estiloActualizado);
                }
                catch (NpgsqlException)
                {
                    resultado = false;
                    cantidadFilas = 0;
                }

                //Si la actualización fue correcta, devolvemos true
                if (cantidadFilas > 0)
                    resultado = true;
            }

            return resultado;
        }

        /// <summary>
        /// Elimina un estilo existente
        /// </summary>
        /// <param name="nombreEstilo"></param>
        /// <returns>Verdadero si la eliminación se hizo correctamente</returns>
        public static bool EliminaEstiloCerveza(Estilo unEstilo, out string mensajeEliminacion)
        {
            //Validaciones previas: 
            // - Que el estilo a actualizar exista - Busqueda por ID
            // - Que no tenga cervezas asociadas

            mensajeEliminacion = string.Empty;
            int cantidadFilas = 0;
            bool resultado = false;
            string? cadenaConexion = ObtieneCadenaConexion();

            using (IDbConnection cxnDB = new NpgsqlConnection(cadenaConexion))
            {
                //Primero, identificamos si hay un estilo con este nombre

                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@nombre_estilo", unEstilo.Nombre,
                                        DbType.String, ParameterDirection.Input);

                string consultaSQL = "SELECT COUNT(id) total " +
                                     "FROM estilos " +
                                     "WHERE nombre = @nombre_estilo";

                cantidadFilas = cxnDB.Query<int>(consultaSQL, parametrosSentencia).FirstOrDefault();

                //Si no hay filas, no existe un estilo con ese nombre... no hay nada que eliminar.
                if (cantidadFilas == 0)
                {
                    mensajeEliminacion = $"Eliminación Fallida. No existe un estilo con el nombre {unEstilo.Nombre}.";
                    return false;
                }

                //Luego verificamos que el estilo no esté asociado a una cerveza
                consultaSQL = "SELECT COUNT(c.id) total_cervezas " +
                              "FROM cervezas c " +
                              "JOIN estilos e ON c.estilo_id = e.id " +
                              "WHERE e.nombre = @nombre_estilo";

                cantidadFilas = cxnDB.Query<int>(consultaSQL, parametrosSentencia).FirstOrDefault();

                //Si hay filas, existen cervezas asociadas con ese estilo, no se puede eliminar
                if (cantidadFilas > 0)
                {
                    mensajeEliminacion = $"Eliminación Fallida. Existen {cantidadFilas} cervezas asociadas" +
                                         $" al estilo {unEstilo.Nombre}.";
                    return false;
                }

                //Pasadas las validaciones, borramos el estilo
                try
                {
                    string eliminaEstiloSQL = "DELETE FROM estilos " +
                                              "WHERE nombre = @Nombre";

                    //Aqui no usamos parámetros dinámicos, pasamos el objeto!!!
                    cantidadFilas = cxnDB.Execute(eliminaEstiloSQL, unEstilo);
                }
                catch (NpgsqlException elError)
                {
                    resultado = false;
                    cantidadFilas = 0;
                    mensajeEliminacion = $"Error de borrado en la DB. {elError.Message}";
                }

                if (cantidadFilas > 0)
                {
                    resultado = true;
                    mensajeEliminacion = $"Eliminación Exitosa. " +
                        $"Se eliminó el estilo {unEstilo.Id} - {unEstilo.Nombre}";
                }
            }

            return resultado;
        }

        #endregion Estilos
    }
}
