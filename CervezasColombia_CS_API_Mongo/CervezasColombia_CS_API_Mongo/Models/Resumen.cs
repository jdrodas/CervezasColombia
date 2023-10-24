using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_Mongo.Models
{
    public class Resumen
    {
        [JsonPropertyName("ubicaciones")]
        public long Ubicaciones { get; set; } = 0;

        [JsonPropertyName("estilos")]
        public long Estilos { get; set; } = 0;

        [JsonPropertyName("tipos_ingredientes")]
        public long Tipos_Ingredientes { get; set; } = 0;

        [JsonPropertyName("envasados")]
        public long Envasados { get; set; } = 0;

        [JsonPropertyName("cervecerias")]
        public long Cervecerias { get; set; } = 0;

        [JsonPropertyName("cervezas")]
        public long Cervezas { get; set; } = 0;

        [JsonPropertyName("ingredientes")]
        public long Ingredientes { get; set; } = 0;

        [JsonPropertyName("unidades_volumen")]
        public long Unidades_Volumen { get; set; } = 0;
    }
}
