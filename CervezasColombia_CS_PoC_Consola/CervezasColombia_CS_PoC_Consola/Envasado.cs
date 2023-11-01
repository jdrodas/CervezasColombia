using System.Text.Json.Serialization;

namespace CervezasColombia_CS_PoC_Consola
{
    public class Envasado
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; } = string.Empty;

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;
    }
}



