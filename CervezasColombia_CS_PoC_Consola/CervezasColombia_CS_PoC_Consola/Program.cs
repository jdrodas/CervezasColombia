namespace CervezasColombia_CS_PoC_Consola
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("PoC - Cervezas Artesanales de Colombia");

            string? cadenaConexion = AccesoDatos.ObtieneCadenaConexion();
            Console.WriteLine($"El string de conexión obtenido es : {cadenaConexion}");

            //Aqui demostramos una consulta a una tabla devolviendo una lista de strings
            Console.WriteLine("\n*** Sentencias de Consulta ***");
            VisualizaNombresEstilosCerveza();

            //Aqui demostramos la manipulación de una lista de objetos
            List<Estilo> losEstilos = AccesoDatos.ObtieneEstilosCerveza();

            Console.WriteLine("\n\nLos Estilos con Id y Nombre son:");

            foreach (Estilo unEstilo in losEstilos)
                Console.WriteLine($"Id: {unEstilo.Id}, Nombre: {unEstilo.Nombre}");

            //Aqui creamos un nuevo estilo y lo insertamos en la base de datos
            string nuevoEstilo = "UchuvIPA";
            Console.WriteLine($"\nRegistro de nuevo estilo de cerveza: {nuevoEstilo}:");

            bool resultadoInsercion = AccesoDatos.InsertaEstiloCerveza(nuevoEstilo);

            if (resultadoInsercion == false)
                Console.WriteLine($"Inserción fallida para el estilo {nuevoEstilo}");
            else
            {
                Console.WriteLine($"Inserción exitosa! Este fue el estilo registrado");

                //Obtenemos el estilo por nombre
                Estilo unEstilo = AccesoDatos.ObtieneEstiloCerveza(nuevoEstilo);
                Console.WriteLine($"Id: {unEstilo.Id}, Nombre: {unEstilo.Nombre}");
            }

            //Aqui hacemos una actualización basado en un objeto
            Estilo estiloActualizado = new Estilo() { Id=1, Nombre="MaracuyIPA" };
            Console.WriteLine($"\n\nActualizando el estilo No. {estiloActualizado.Id} al nuevo nombre de {estiloActualizado.Nombre}...");

            bool resultadoActualizacion = AccesoDatos.ActualizaEstiloCerveza(estiloActualizado);

            if (resultadoActualizacion == false)
                Console.WriteLine($"Actualización fallida para el estilo {estiloActualizado}");
            else
            {
                Console.WriteLine($"Actualización exitosa! Este fue el estilo actualizado");

                //Obtenemos el estilo por nombre
                Estilo unEstilo = AccesoDatos.ObtieneEstiloCerveza(estiloActualizado.Nombre);
                Console.WriteLine($"Id: {unEstilo.Id}, Nombre: {unEstilo.Nombre}");
            }

            //Aqui hacemos un borrado de un estilo buscándolo por nombre
            Console.WriteLine($"\n\nBorrando el estilo {nuevoEstilo} ...");

            string mensajeEliminacion;
            bool resultadoEliminacion = AccesoDatos.EliminaEstiloCerveza(nuevoEstilo, out mensajeEliminacion);

            if (resultadoEliminacion == false)
                Console.WriteLine(mensajeEliminacion);
            else
            {
                Console.WriteLine($"Actualización exitosa! el estilo {nuevoEstilo} fue eliminado");
                VisualizaNombresEstilosCerveza();
            }
        }

        /// <summary>
        /// Visualiza la lista de nombres de cerveza registrados
        /// </summary>
        public static void VisualizaNombresEstilosCerveza()
        {
            Console.WriteLine($"Estilos de Cerveza registrados en la DB:");
            List<string> losEstilos = AccesoDatos.ObtieneNombresEstilosCerveza();

            if (losEstilos.Count == 0)
                Console.WriteLine("No se encontraron Estilos de Cerveza");
            else
            {
                Console.WriteLine($"\nSe encontraron {losEstilos.Count} Estilos:");

                foreach (string unEstilo in losEstilos)
                    Console.WriteLine($"- {unEstilo}");
            }
        }
    }
}