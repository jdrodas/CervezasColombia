using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_SQLite_Dapper.Ingredientes
{
    public class IngredienteResponse : BaseResponse
    {
        [JsonPropertyName("data")]
        public List<Ingrediente> Data { get; set; } = [];
    }
}
