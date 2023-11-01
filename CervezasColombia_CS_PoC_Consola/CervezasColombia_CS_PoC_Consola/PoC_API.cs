namespace CervezasColombia_CS_PoC_Consola
{
    public class PoC_API
    {
        public static void Ejecuta_PoC()
        {
            string? cadenaConexion = AccesoDatosAPI.ObtieneCadenaConexion();
            Console.WriteLine($"El string de conexión obtenido es: \n{cadenaConexion}\n");

            VisualizaNombresEstilosCerveza().Wait();
        }


        /// <summary>
        /// Visualiza la lista de nombres de cerveza registrados en la DB
        /// </summary>
        public static async Task VisualizaNombresEstilosCerveza()
        {
            Console.WriteLine($"Estilos de Cerveza registrados en la DB:");
            List<string> losNombresEstilos = await AccesoDatosAPI.ObtieneNombresEstilosCerveza();

            if (losNombresEstilos.Count == 0)
                Console.WriteLine("No se encontraron Estilos de Cerveza");
            else
            {
                Console.WriteLine($"\nSe encontraron {losNombresEstilos.Count} Estilos:");

                foreach (string unNombreEstilo in losNombresEstilos)
                    Console.WriteLine($"- {unNombreEstilo}");
            }
        }
    }
}
