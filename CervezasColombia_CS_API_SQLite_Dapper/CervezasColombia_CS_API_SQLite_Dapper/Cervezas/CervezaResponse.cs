using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_SQLite_Dapper.Cervezas
{
    public class CervezaResponse : BaseResponse
    {
        [JsonPropertyName("data")]
        public List<Cerveza> Data { get; set; } = [];
    }
}
