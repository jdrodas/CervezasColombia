using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_SQLite_Dapper.Models
{
    public class CervezaEnvasada : Cerveza
    {
        [JsonPropertyName("envasado")]
        public string Envasado { get; set; } = string.Empty;

        [JsonPropertyName("unidad_volumen")]
        public string Unidad_Volumen { get; set; } = string.Empty;

        [JsonPropertyName("volumen")]
        public double Volumen { get; set; } = 0.0d;
    }
}
