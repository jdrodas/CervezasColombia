using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CervezasColombia_CS_PoC_Consola
{
    public class AccesoDatosAPI
    {
        public static string? ObtieneCadenaConexion()
        {
            //Parametrizamos el acceso al archivo de configuración appsettings.json
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration miConfiguracion = builder.Build();

            return miConfiguracion["ConnectionString:API"];
        }

        public static async Task<List<Estilo>> ObtieneEstilosCerveza()
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            HttpClient miCliente = new();
            miCliente.BaseAddress = new Uri(cadenaConexion!);
            miCliente.DefaultRequestHeaders.Accept.Clear();
            miCliente.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var resultado = await miCliente.GetAsync("api/Estilos");

            List<Estilo>? listaEstilos = new();

            if (resultado.IsSuccessStatusCode)
            {
                var contenido = await resultado.Content.ReadAsStringAsync();
                listaEstilos = JsonSerializer.Deserialize<List<Estilo>>(contenido);
            }

            return listaEstilos!;
        }

        public static async Task<List<string>> ObtieneNombresEstilosCerveza()
        {
            var listaEstilos = await ObtieneEstilosCerveza();

            List<string> listaNombresEstilos = new List<string>();

            foreach (Estilo unEstilo in listaEstilos)
                listaNombresEstilos.Add(unEstilo.Nombre!);

            return listaNombresEstilos;
        }
    }
}
