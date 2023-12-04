using CervezasColombia_CS_API_SQLite_Dapper.Cervezas;
using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_SQLite_Dapper.Estilos
{
    public class EstiloDetallado : Estilo
    {
        [JsonPropertyName("cervezas")]
        public List<Cerveza> Cervezas { get; set; } = [];
    }
}
