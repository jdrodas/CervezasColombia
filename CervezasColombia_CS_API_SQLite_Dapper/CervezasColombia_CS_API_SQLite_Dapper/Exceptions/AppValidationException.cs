﻿/*
 AppValidationException:
 Excepcion creada para enviar mensajes relacionados 
 con la validación en todas las operaciones CRUD de la aplicación
*/


namespace CervezasColombia_CS_API_SQLite_Dapper.Exceptions
{
    public class AppValidationException(string message) : Exception(message)
    {
    }
}