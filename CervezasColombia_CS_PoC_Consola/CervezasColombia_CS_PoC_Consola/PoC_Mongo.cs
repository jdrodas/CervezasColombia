namespace CervezasColombia_CS_PoC_Consola
{
    public class PoC_Mongo
    {
        public static void Ejecuta_PoC()
        {
            string? cadenaConexion = AccesoDatosMongo.ObtieneCadenaConexion();
            Console.WriteLine($"El string de conexión obtenido es: \n{cadenaConexion}\n");

            //R del CRUD - Lectura de registros existentes - SELECT
            VisualizaNombresEstilosCerveza();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            VisualizaEstilosCerveza();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            //C del CRUD - Creación de un nuevo registro - INSERT
            Estilo nuevoEstilo = new Estilo() { Id = 100, Nombre = "UchuvIPA" };
            Console.WriteLine($"\nRegistro de nuevo estilo de cerveza: {nuevoEstilo.Nombre}:");

            bool resultadoInsercion = AccesoDatosMongo.InsertaEstiloCerveza(nuevoEstilo);

            if (resultadoInsercion == false)
                Console.WriteLine($"Inserción fallida para el estilo {nuevoEstilo}");
            else
            {
                Console.WriteLine($"Inserción exitosa! Este fue el estilo registrado");

                //Obtenemos el estilo por nombre
                nuevoEstilo = AccesoDatosMongo.ObtieneEstiloCerveza(nuevoEstilo.Nombre);
                Console.WriteLine($"Id: {nuevoEstilo.Id}, Nombre: {nuevoEstilo.Nombre}");
            }

            VisualizaEstilosCerveza();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            //U del CRUD - Actualización de un nuevo registro - UPDATE
            nuevoEstilo.Nombre = "MaracuyIPA";
            Console.WriteLine($"\n\nActualizando el estilo No. {nuevoEstilo.Id} " +
                $"al nuevo nombre de {nuevoEstilo.Nombre}...");

            bool resultadoActualizacion = AccesoDatosMongo.ActualizaEstiloCerveza(nuevoEstilo);

            if (resultadoActualizacion == false)
                Console.WriteLine($"Actualización fallida para el estilo {nuevoEstilo.Nombre}");
            else
            {
                Console.WriteLine($"Actualización exitosa! Este fue el estilo actualizado");

                //Obtenemos el estilo por Id
                Estilo unEstilo = AccesoDatosMongo.ObtieneEstiloCerveza(nuevoEstilo.Nombre!);
                Console.WriteLine($"Id: {unEstilo.Id}, Nombre: {unEstilo.Nombre}");
            }

            VisualizaEstilosCerveza();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            //D del CRUD - Borrado de un estilo existente - DELETE
            Console.WriteLine($"\n\nBorrando el estilo {nuevoEstilo.Nombre} ...");

            bool resultadoEliminacion = AccesoDatosMongo.EliminaEstiloCerveza(nuevoEstilo);

            if (resultadoEliminacion == false)
                Console.WriteLine($"Eliminación fallida! el estilo {nuevoEstilo.Nombre} NO fue eliminado");
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
            List<string> losNombresEstilos = AccesoDatosMongo.ObtieneNombresEstilosCerveza();

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
            List<Estilo> losEstilos = AccesoDatosMongo.ObtieneEstilosCerveza();

            Console.WriteLine("\n\nLos Estilos con Id y Nombre son:");

            foreach (Estilo unEstilo in losEstilos)
                Console.WriteLine($"Id: {unEstilo.Id}\tNombre: {unEstilo.Nombre}");
        }
    }
}
