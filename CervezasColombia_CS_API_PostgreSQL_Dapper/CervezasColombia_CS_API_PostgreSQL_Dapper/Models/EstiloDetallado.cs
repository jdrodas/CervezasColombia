using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_PostgreSQL_Dapper.Models
{
    public class EstiloDetallado : Estilo
    {
        [JsonPropertyName("cervezas")]
        public List<Cerveza> Cervezas { get; set; } = new List<Cerveza>();
    }
}
