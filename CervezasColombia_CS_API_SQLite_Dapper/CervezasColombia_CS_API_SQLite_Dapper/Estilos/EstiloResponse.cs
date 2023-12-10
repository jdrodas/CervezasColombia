using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_SQLite_Dapper.Estilos
{
    public class EstiloResponse : BaseResponse
    {
        [JsonPropertyName("data")]
        public List<Estilo> Data { get; set; } = [];
    }
}
