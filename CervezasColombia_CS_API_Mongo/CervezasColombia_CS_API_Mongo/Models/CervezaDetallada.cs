using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_Mongo.Models
{
    public class CervezaDetallada : Cerveza
    {
        [JsonPropertyName("envasados")]
        public List<EnvasadoCerveza> Envasados { get; set; } = new List<EnvasadoCerveza>();

        [JsonPropertyName("ingredientes")]
        public List<IngredienteCerveza> Ingredientes { get; set; } = new List<IngredienteCerveza>();
    }
}
