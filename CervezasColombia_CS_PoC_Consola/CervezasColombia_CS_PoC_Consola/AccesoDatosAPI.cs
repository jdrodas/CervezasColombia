using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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

        public static async Task<List<Envasado>> ObtieneEnvasadosCerveza()
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            HttpClient miCliente = new();
            miCliente.BaseAddress = new Uri(cadenaConexion!);
            miCliente.DefaultRequestHeaders.Accept.Clear();
            miCliente.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var resultado = await miCliente
                .GetAsync("api/Envasados");

            List<Envasado>? listaEnvasados = new();

            if (resultado.IsSuccessStatusCode)
            {
                var contenido = await resultado.Content.ReadAsStringAsync();
                listaEnvasados = JsonSerializer.Deserialize<List<Envasado>>(contenido);
            }

            return listaEnvasados!;
        }

        public static async Task<List<string>> ObtieneNombresEnvasadosCerveza()
        {
            var listaEnvasados = await ObtieneEnvasadosCerveza();

            List<string> listaNombreEnvasados = new();

            foreach (Envasado unEnvasado in listaEnvasados)
                listaNombreEnvasados.Add(unEnvasado.Nombre!);

            return listaNombreEnvasados;
        }

        public static async Task<Envasado> ObtieneEnvasadoCerveza(string nombreEnvasado)
        {
            Envasado envasadoExistente = new();
            var listaEnvasados = await ObtieneEnvasadosCerveza();

            envasadoExistente = listaEnvasados.Find(envasado => envasado.Nombre == nombreEnvasado)!;
            return envasadoExistente;
        }

        public static async Task<bool> InsertaEnvasadoCerveza(Envasado unEnvasado)
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            HttpClient miCliente = new();
            miCliente.BaseAddress = new Uri(cadenaConexion!);
            miCliente.DefaultRequestHeaders.Accept.Clear();
            miCliente.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var resultado = await miCliente
                .PostAsJsonAsync("api/Envasados", unEnvasado);

            return resultado.IsSuccessStatusCode;
        }

        public static async Task<bool> ActualizaEnvasadoCerveza(Envasado unEnvasado)
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            HttpClient miCliente = new();
            miCliente.BaseAddress = new Uri(cadenaConexion!);
            miCliente.DefaultRequestHeaders.Accept.Clear();
            miCliente.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var resultado = await miCliente
                .PutAsJsonAsync($"api/Envasados/{unEnvasado.Id}", unEnvasado);

            return resultado.IsSuccessStatusCode;

        }

        public static async Task<bool> EliminaEnvasadoCerveza(Envasado unEnvasado)
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            HttpClient miCliente = new();
            miCliente.BaseAddress = new Uri(cadenaConexion!);
            miCliente.DefaultRequestHeaders.Accept.Clear();
            miCliente.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var resultado = await miCliente
                .DeleteAsync($"api/Envasados/{unEnvasado.Id}");

            return resultado.IsSuccessStatusCode;
        }


    }
}
