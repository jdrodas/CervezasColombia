namespace CervezasColombia_CS_PoC_Consola
{
    public class PoC_SQLite
    {
        public static void Ejecuta_PoC()
        {
            string? cadenaConexion = AccesoDatosSQLite.ObtieneCadenaConexion();
            Console.WriteLine($"El string de conexión obtenido es:\n{cadenaConexion}");

            //R del CRUD - Lectura de registros existentes - SELECT
            VisualizaNombresEstilosCerveza();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            VisualizaEstilosCerveza();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            //C del CRUD - Creación de un nuevo registro - INSERT
            Estilo unEstilo = new() { Nombre = "UchuvIPA" };
            Console.WriteLine($"\nRegistro de nuevo estilo de cerveza: {unEstilo.Nombre}:");

            bool resultadoInsercion = AccesoDatosSQLite.InsertaEstiloCerveza(unEstilo);

            if (resultadoInsercion == false)
                Console.WriteLine($"Inserción fallida para el estilo {unEstilo.Nombre}");
            else
            {
                Console.WriteLine($"Inserción exitosa! Este fue el estilo registrado");

                //Obtenemos el estilo por nombre
                unEstilo = AccesoDatosSQLite.ObtieneEstiloCerveza(unEstilo.Nombre);
                Console.WriteLine($"Id: {unEstilo.Id}, Nombre: {unEstilo.Nombre}");
            }

            VisualizaEstilosCerveza();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            //U del CRUD - Actualización de un nuevo registro - UPDATE
            unEstilo.Nombre = "MaracuyIPA";
            Console.WriteLine($"\n\nActualizando el estilo No. {unEstilo.Id} " +
                $"al nuevo nombre de {unEstilo.Nombre}...");

            bool resultadoActualizacion = AccesoDatosSQLite.ActualizaEstiloCerveza(unEstilo);

            if (resultadoActualizacion == false)
                Console.WriteLine($"Actualización fallida para el estilo {unEstilo.Nombre}");
            else
            {
                Console.WriteLine($"Actualización exitosa! Este fue el estilo actualizado");

                //Obtenemos el estilo por Id
                unEstilo = AccesoDatosSQLite.ObtieneEstiloCerveza(unEstilo.Id);
                Console.WriteLine($"Id: {unEstilo.Id}, Nombre: {unEstilo.Nombre}");
            }

            VisualizaEstilosCerveza();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            //D del CRUD - Borrado de un estilo existente - DELETE
            Console.WriteLine($"\n\nBorrando el estilo {unEstilo.Nombre} ...");

            string mensajeEliminacion;
            bool resultadoEliminacion = AccesoDatosSQLite.EliminaEstiloCerveza(unEstilo, out mensajeEliminacion);

            if (resultadoEliminacion == false)
                Console.WriteLine(mensajeEliminacion);
            else
            {
                Console.WriteLine($"Eliminación exitosa! el estilo {unEstilo.Nombre} fue eliminado");
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
            List<string> losNombresEstilos = AccesoDatosSQLite.ObtieneNombresEstilosCerveza();

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
            List<Estilo> losEstilos = AccesoDatosSQLite.ObtieneEstilosCerveza();

            Console.WriteLine("\n\nLos Estilos con Id y Nombre son:");

            foreach (Estilo unEstilo in losEstilos)
                Console.WriteLine($"Id: {unEstilo.Id}\tNombre: {unEstilo.Nombre}");
        }
    }
}
