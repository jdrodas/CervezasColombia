using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_SQLite_Dapper.Models
{
    public class Estilo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;
    }
}
