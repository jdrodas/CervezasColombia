/*
 DbOperationException:
 Excepcion creada para enviar mensajes relacionados 
 con las acciones a nivel de base de datos en todas
 las operaciones CRUD de la aplicación
*/

namespace CervezasColombia_CS_API_SQLite_Dapper.Helpers
{
    public class DbOperationException : Exception
    {
        public DbOperationException(string message) : base(message) { }
    }
}