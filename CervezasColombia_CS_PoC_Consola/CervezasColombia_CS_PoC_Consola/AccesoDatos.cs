using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SQLite;
using System.Data;

namespace CervezasColombia_CS_PoC_Consola
{
    public class AccesoDatos
    {
        public static string? ObtieneCadenaConexion()
        {
            //Parametrizamos el acceso al archivo de configuración appsettings.json
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration miConfiguracion = builder.Build();

            return miConfiguracion["ConnectionString:Sqlite"];
        }

        #region Estilos

        /// <summary>
        /// Obtiene la lista con los nombres de los estilos de cerveza
        /// </summary>
        /// <returns>Lista con los nombres de los estilos</returns>
        public static List<string> ObtieneNombresEstilosCerveza()
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            using (IDbConnection cxnDB = new SQLiteConnection(cadenaConexion))
            {
                var resultadoEstilos = cxnDB.Query<string>("SELECT nombre FROM estilos ORDER BY nombre", new DynamicParameters());

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

            using (IDbConnection cxnDB = new SQLiteConnection(cadenaConexion))
            {
                var resultadoEstilos = cxnDB.Query<Estilo>("SELECT id, nombre FROM estilos ORDER BY nombre", new DynamicParameters());

                return resultadoEstilos.AsList();
            }
        }

        /// <summary>
        /// Obtiene un Estilo de Cerveza de acuerdo al nombre
        /// </summary>
        /// <param name="nombreEstilo"></param>
        /// <returns></returns>
        public static Estilo ObtieneEstiloCerveza(string nombreEstilo)
        {
            Estilo estiloResultado = new Estilo();
            string? cadenaConexion = ObtieneCadenaConexion();

            //Aqui buscamos el estilo asociado al nombre
            using (IDbConnection cxnDB = new SQLiteConnection(cadenaConexion))
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@nombre_estilo", nombreEstilo,
                    DbType.String, ParameterDirection.Input);

                string? consultaEstiloSql = "SELECT id,nombre FROM estilos WHERE nombre = @nombre_estilo";
                var salida = cxnDB.Query<Estilo>(consultaEstiloSql, parametrosSentencia);

                if (salida.ToArray().Length > 0)
                    estiloResultado = salida.First();

                return estiloResultado;
            }
        }

        /// <summary>
        /// Inserta un nuevo estilo de cerveza
        /// </summary>
        /// <param name="unEstilo">El nombre del estilo</param>
        /// <returns>Verdadero si la inserción se hizo correctamente</returns>
        public static bool InsertaEstiloCerveza(string unEstilo)
        {
            int cantidadFilas = 0;
            bool resultado = false;
            string? cadenaConexion = ObtieneCadenaConexion();

            //Aqui validamos primero que el nombre del estilo de cerveza no exista
            using (IDbConnection cxnDB = new SQLiteConnection(cadenaConexion))
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@nombre_estilo", unEstilo,
                    DbType.String, ParameterDirection.Input);

                //Preguntamos si ya existe un estilo con ese nombre
                string consultaEstiloSql = "SELECT COUNT(id) total FROM estilos WHERE nombre = @nombre_estilo";

                cantidadFilas = cxnDB.Query<int>(consultaEstiloSql, parametrosSentencia).FirstOrDefault();

                //Si no hay filas, se puede insertar nuevo registro
                if (cantidadFilas == 0)
                {
                    try
                    {
                        string insertaEstiloSql = "INSERT INTO estilos (nombre) VALUES (@nombre_estilo)";
                        cantidadFilas = cxnDB.Execute(insertaEstiloSql, parametrosSentencia);
                    }
                    catch (SQLiteException)
                    {
                        resultado = false;
                        cantidadFilas = 0;
                    }

                    //Si la inserción fue correcta, devolvemos true
                    if (cantidadFilas > 0)
                        resultado = true;
                }
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
            int cantidadFilas = 0;
            bool resultado = false;
            string? cadenaConexion = ObtieneCadenaConexion();

            //Aqui validamos primero que el Estilo previamente exists
            using (IDbConnection cxnDB = new SQLiteConnection(cadenaConexion))
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@estilo_id", estiloActualizado.Id,
                    DbType.Int32, ParameterDirection.Input);

                string consultaEstiloSql = "SELECT COUNT(id) total FROM estilos " +
                    "WHERE id = @estilo_id";

                cantidadFilas = cxnDB.Query<int>(consultaEstiloSql, parametrosSentencia).FirstOrDefault();

                //Si no hay filas, no existe departamento que actualizar
                if (cantidadFilas == 0)
                    return false;
                else
                {
                    parametrosSentencia = new DynamicParameters();
                    parametrosSentencia.Add("@estilo_nombre", estiloActualizado.Nombre,
                        DbType.String, ParameterDirection.Input);

                    //Validamos si el nuevo nombre no exista
                    consultaEstiloSql = "SELECT COUNT(id) total FROM estilos WHERE nombre = @estilo_nombre";

                    cantidadFilas = cxnDB.Query<int>(consultaEstiloSql, parametrosSentencia).FirstOrDefault();

                    //Si hay filas, el nuevo nombre a utilizar ya existe!
                    if (cantidadFilas != 0)
                        return false;
                    else
                    {
                        try
                        {
                            string actualizaEstiloSql = "UPDATE estilos SET nombre = @Nombre " +
                                "WHERE id = @Id"; ;

                            cantidadFilas = cxnDB.Execute(actualizaEstiloSql, estiloActualizado);
                        }
                        catch (SQLiteException)
                        {
                            resultado = false;
                            cantidadFilas = 0;
                        }

                        //Si la actualización fue correcta, devolvemos true
                        if (cantidadFilas > 0)
                            resultado = true;
                    }
                }
            }

            return resultado;
        }

        /// <summary>
        /// Elimina un estilo existente
        /// </summary>
        /// <param name="nombreEstilo"></param>
        /// <returns>Verdadero si la eliminación se hizo correctamente</returns>
        public static bool EliminaEstiloCerveza(string nombreEstilo, out string mensajeEliminacion)
        {
            mensajeEliminacion = string.Empty;
            int cantidadFilas = 0;
            bool resultado = false;
            string? cadenaConexion = ObtieneCadenaConexion();

            //Primero, identificamos si hay un estilo con este nombre
            using (IDbConnection cxnDB = new SQLiteConnection(cadenaConexion))
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@nombre_estilo", nombreEstilo,
                    DbType.String, ParameterDirection.Input);

                string consultaEstiloSql = "SELECT COUNT(id) total FROM estilos WHERE nombre = @nombre_estilo";

                cantidadFilas = cxnDB.Query<int>(consultaEstiloSql, parametrosSentencia).FirstOrDefault();

                //Si no hay filas, no existe un estilo con ese nombre... no hay nada que eliminar.
                if (cantidadFilas == 0)
                {
                    mensajeEliminacion = $"Eliminación Fallida. No existe un estilo con el nombre {nombreEstilo}.";
                    return false;
                }

                //Luego verificamos que el estilo no esté asociado a una cerveza
                consultaEstiloSql = "SELECT COUNT(c.id) total_cervezas FROM cervezas c " +
                    "JOIN estilos e ON c.estilo_id = e.id WHERE e.nombre = @nombre_estilo";

                cantidadFilas = cxnDB.Query<int>(consultaEstiloSql, parametrosSentencia).FirstOrDefault();

                //Si hay filas, existen cervezas asociadas con ese estilo, no se puede eliminar
                if (cantidadFilas > 0)
                {
                    mensajeEliminacion = $"Eliminación Fallida. Existen {cantidadFilas} cervezas asociadas" +
                        $" al estilo {nombreEstilo}.";
                    return false;
                }

                //Pasadas las validaciones, borramos el estilo
                try
                {
                    string eliminaEstiloSql = "DELETE FROM estilos WHERE nombre = @nombre_estilo";
                    cantidadFilas = cxnDB.Execute(eliminaEstiloSql, parametrosSentencia);

                    if (cantidadFilas > 0)
                    {
                        resultado = true;
                        mensajeEliminacion = "Eliminación Exitosa";
                    }
                }
                catch (SQLiteException elError)
                {
                    resultado = false;
                    cantidadFilas = 0;
                    mensajeEliminacion = $"Error de borrado en la DB. {elError.Message}";
                }
            }

            return resultado;
        }

        #endregion Estilos

    }
}

