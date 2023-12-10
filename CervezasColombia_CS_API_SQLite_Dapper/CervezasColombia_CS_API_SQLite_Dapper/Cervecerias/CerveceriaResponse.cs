using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_SQLite_Dapper.Cervecerias
{
    public class CerveceriaResponse : BaseResponse
    {
        [JsonPropertyName("data")]
        public List<Cerveceria> Data { get; set; } = [];
    }
}
