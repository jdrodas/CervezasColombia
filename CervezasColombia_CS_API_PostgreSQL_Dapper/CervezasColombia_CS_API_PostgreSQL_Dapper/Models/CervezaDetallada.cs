using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_PostgreSQL_Dapper.Models
{
    public class CervezaDetallada : Cerveza
    {
        [JsonPropertyName("envasados")]
        public List<EnvasadoCerveza> Envasados { get; set; } = new List<EnvasadoCerveza>();

        [JsonPropertyName("ingredientes")]
        public List<Ingrediente> Ingredientes { get; set; } = new List<Ingrediente>();
    }
}
