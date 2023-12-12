using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_SQLite_Dapper.Ubicaciones
{
    public class UbicacionResponse : BaseResponse
    {
        [JsonPropertyName("data")]
        public List<Ubicacion> Data { get; set; } = [];
    }
}
