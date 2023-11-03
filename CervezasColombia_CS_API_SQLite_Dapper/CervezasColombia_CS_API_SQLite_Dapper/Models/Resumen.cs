using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_SQLite_Dapper.Models
{
    public class Resumen
    {
        [JsonPropertyName("ubicaciones")]
        public int Ubicaciones { get; set; } = 0;

        [JsonPropertyName("cervecerias")]
        public int Cervecerias { get; set; } = 0;

        [JsonPropertyName("cervezas")]
        public int Cervezas { get; set; } = 0;

        [JsonPropertyName("estilos")]
        public int Estilos { get; set; } = 0;

        [JsonPropertyName("envasados")]
        public int Envasados { get; set; } = 0;

        [JsonPropertyName("ingredientes")]
        public int Ingredientes { get; set; } = 0;

        [JsonPropertyName("tipos_ingredientes")]
        public int Tipos_Ingredientes { get; set; } = 0;
    }
}
