using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_SQLite_Dapper.Models
{
    public class CerveceriaDetallada : Cerveceria
    {
        [JsonPropertyName("cervezas")]
        public List<Cerveza> Cervezas { get; set; } = new List<Cerveza>();
    }
}
