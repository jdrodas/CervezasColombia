﻿namespace CervezasColombia_CS_PoC_Consola
{
    public class PoC_Pgsql
    {
        public static void Ejecuta_PoC()
        {
            string? cadenaConexion = AccesoDatosPgsql.ObtieneCadenaConexion();
            Console.WriteLine($"El string de conexión obtenido es: \n{cadenaConexion}\n");

            //R del CRUD - Lectura de registros existentes - SELECT
            VisualizaNombresEstilosCerveza();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            VisualizaEstilosCerveza();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            //C del CRUD - Creación de un nuevo registro - INSERT
            Estilo nuevoEstilo = new Estilo() { Nombre = "UchuvIPA" };
            Console.WriteLine($"\nRegistro de nuevo estilo de cerveza: {nuevoEstilo.Nombre}:");

            bool resultadoInsercion = AccesoDatosPgsql.InsertaEstiloCerveza(nuevoEstilo);

            if (resultadoInsercion == false)
                Console.WriteLine($"Inserción fallida para el estilo {nuevoEstilo}");
            else
            {
                Console.WriteLine($"Inserción exitosa! Este fue el estilo registrado");

                //Obtenemos el estilo por nombre
                nuevoEstilo = AccesoDatosPgsql.ObtieneEstiloCerveza(nuevoEstilo.Nombre);
                Console.WriteLine($"Id: {nuevoEstilo.Id}, Nombre: {nuevoEstilo.Nombre}");
            }

            VisualizaEstilosCerveza();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            //U del CRUD - Actualización de un nuevo registro - UPDATE
            Estilo estiloActualizado = new Estilo() { Id = 1, Nombre = "MaracuyIPA" };
            Console.WriteLine($"\n\nActualizando el estilo No. {estiloActualizado.Id} " +
                $"al nuevo nombre de {estiloActualizado.Nombre}...");

            bool resultadoActualizacion = AccesoDatosPgsql.ActualizaEstiloCerveza(estiloActualizado);

            if (resultadoActualizacion == false)
                Console.WriteLine($"Actualización fallida para el estilo {estiloActualizado.Nombre}");
            else
            {
                Console.WriteLine($"Actualización exitosa! Este fue el estilo actualizado");

                //Obtenemos el estilo por Id
                Estilo unEstilo = AccesoDatosPgsql.ObtieneEstiloCerveza(estiloActualizado.Id);
                Console.WriteLine($"Id: {unEstilo.Id}, Nombre: {unEstilo.Nombre}");
            }

            VisualizaEstilosCerveza();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            //Devolvemos el estilo a su valor orignal
            estiloActualizado.Nombre = "Amber Ale";
            AccesoDatosPgsql.ActualizaEstiloCerveza(estiloActualizado);

            //D del CRUD - Borrado de un estilo existente - DELETE
            nuevoEstilo = AccesoDatosPgsql.ObtieneEstiloCerveza(nuevoEstilo.Nombre!);
            Console.WriteLine($"\n\nBorrando el estilo {nuevoEstilo.Nombre} ...");

            string mensajeEliminacion;
            bool resultadoEliminacion = AccesoDatosPgsql.EliminaEstiloCerveza(nuevoEstilo, out mensajeEliminacion);

            if (resultadoEliminacion == false)
                Console.WriteLine(mensajeEliminacion);
            else
            {
                Console.WriteLine($"Eliminación exitosa! el estilo {nuevoEstilo.Nombre} fue eliminado");
                VisualizaEstilosCerveza();
            }

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();
        }


        /// <summary>
        /// Visualiza la lista de nombres de cerveza registrados en la DB
        /// </summary>
        public static void VisualizaNombresEstilosCerveza()
        {
            Console.WriteLine($"Estilos de Cerveza registrados en la DB:");
            List<string> losNombresEstilos = AccesoDatosPgsql.ObtieneNombresEstilosCerveza();

            if (losNombresEstilos.Count == 0)
                Console.WriteLine("No se encontraron Estilos de Cerveza");
            else
            {
                Console.WriteLine($"\nSe encontraron {losNombresEstilos.Count} Estilos:");

                foreach (string unNombreEstilo in losNombresEstilos)
                    Console.WriteLine($"- {unNombreEstilo}");
            }
        }

        public static void VisualizaEstilosCerveza()
        {
            //Aqui demostramos la manipulación de una lista de objetos tipo Estilo
            List<Estilo> losEstilos = AccesoDatosPgsql.ObtieneEstilosCerveza();

            Console.WriteLine("\n\nLos Estilos con Id y Nombre son:");

            foreach (Estilo unEstilo in losEstilos)
                Console.WriteLine($"Id: {unEstilo.Id}\tNombre: {unEstilo.Nombre}");
        }
    }
}
