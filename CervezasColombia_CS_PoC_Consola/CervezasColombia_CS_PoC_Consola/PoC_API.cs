namespace CervezasColombia_CS_PoC_Consola
{
    public class PoC_API
    {
        public static void Ejecuta_PoC()
        {
            string? cadenaConexion = AccesoDatosAPI.ObtieneCadenaConexion();
            Console.WriteLine($"El string de conexión obtenido es: \n{cadenaConexion}\n");

            VisualizaNombresEnvasadosCerveza().Wait();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            VisualizaEnvasadosCerveza().Wait();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            //C del CRUD - Creación de un nuevo registro - INSERT
            Envasado nuevoEnvasado = new() {Nombre = "Vasito de Ahsoka" };
            Console.WriteLine($"\nRegistro de nuevo envasado de cerveza: {nuevoEnvasado.Nombre}:");

            InsertaEnvasadoCerveza(nuevoEnvasado).Wait();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            VisualizaEnvasadosCerveza().Wait();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            //U del CRUD - Actualización de un registro existente - UPDATE
            string nombreActualizadoEnvasado = "Vasito de Star Wars:Ahsoka";
            ActualizaEnvasadoCerveza(nuevoEnvasado.Nombre, nombreActualizadoEnvasado).Wait();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            VisualizaEnvasadosCerveza().Wait();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            //D del CRUD - Eliminación de un registro existente - DELETE
            EliminaEnvasadoCerveza(nombreActualizadoEnvasado).Wait();

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();

            VisualizaEnvasadosCerveza().Wait();
        }

        public static async Task EliminaEnvasadoCerveza(string nombreEnvasado)
        {
            //Obtenemos el envasado existente con ese nombre
            var envasadoExistente = await AccesoDatosAPI.ObtieneEnvasadoCerveza(nombreEnvasado);

            if (string.IsNullOrEmpty(envasadoExistente.Id))
            {
                Console.WriteLine($"No se encontró el envasado {nombreEnvasado}");
            }
            else
            {
                bool resultadoEliminacion = await AccesoDatosAPI.EliminaEnvasadoCerveza(envasadoExistente);

                if (resultadoEliminacion == false)
                    Console.WriteLine($"Eliminación fallida para el envasado {nombreEnvasado}");
                else
                    Console.WriteLine($"Eliminación exitosa! Se ha borrado el envasado {nombreEnvasado}");
            }
        }

        public static async Task ActualizaEnvasadoCerveza(string nombreEnvasado, string nuevoNombre)
        {
            //Obtenemos el envasado existente con ese nombre
            var envasadoExistente = await AccesoDatosAPI.ObtieneEnvasadoCerveza(nombreEnvasado);

            if (string.IsNullOrEmpty(envasadoExistente.Id))
            {
                Console.WriteLine($"No se encontró el envasado {nombreEnvasado}");
            }
            else
            {
                envasadoExistente.Nombre = nuevoNombre;

                bool resultadoActualizacion = await AccesoDatosAPI.ActualizaEnvasadoCerveza(envasadoExistente);

                if (resultadoActualizacion == false)
                    Console.WriteLine($"Inserción fallida para el envasado {nombreEnvasado}");
                else
                {
                    Console.WriteLine($"Actualización exitosa! Asi quedó el envasado:");
                    Console.WriteLine($"Id: {envasadoExistente.Id}, Nombre: {envasadoExistente.Nombre}");
                }
            }            
        }

        public static async Task InsertaEnvasadoCerveza(Envasado unEnvasado)
        {
            bool resultadoInsercion = await AccesoDatosAPI.InsertaEnvasadoCerveza(unEnvasado);

            if (resultadoInsercion == false)
                Console.WriteLine($"Inserción fallida para el envasado {unEnvasado.Nombre}");
            else
            {
                Console.WriteLine($"Inserción exitosa! Este fue el estilo registrado");

                //Obtenemos el estilo por nombre
                var nuevoEnvasado = await AccesoDatosAPI.ObtieneEnvasadoCerveza(unEnvasado.Nombre);
                Console.WriteLine($"Id: {nuevoEnvasado.Id}, Nombre: {nuevoEnvasado.Nombre}");
            }
        }

        public static async Task VisualizaNombresEnvasadosCerveza()
        {
            Console.WriteLine($"Envasados de Cerveza accesibles a través del API:");
            List<string> losNombresEnvasados = await AccesoDatosAPI.ObtieneNombresEnvasadosCerveza();

            if (losNombresEnvasados.Count == 0)
                Console.WriteLine("No se encontraron Envasados de Cerveza");
            else
            {
                Console.WriteLine($"\nSe encontraron {losNombresEnvasados.Count} Envasados:");

                foreach (string unNombreEnvasado in losNombresEnvasados)
                    Console.WriteLine($"- {unNombreEnvasado}");
            }
        }

        public static async Task VisualizaEnvasadosCerveza()
        {
            //Aqui demostramos la manipulación de una lista de objetos tipo Estilo
            List<Envasado> losEnvasados = await AccesoDatosAPI.ObtieneEnvasadosCerveza();

            Console.WriteLine("\n\nLos Estilos con Id y Nombre son:");

            foreach (Envasado unEnvasado in losEnvasados)
                Console.WriteLine($"Id: {unEnvasado.Id}\tNombre: {unEnvasado.Nombre}");
        }
    }
}
