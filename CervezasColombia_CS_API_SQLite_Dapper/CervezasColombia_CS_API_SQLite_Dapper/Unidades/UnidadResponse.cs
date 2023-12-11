using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_SQLite_Dapper.Unidades
{
    public class UnidadResponse : BaseResponse
    {
        [JsonPropertyName("data")]
        public List<Unidad> Data { get; set; } = [];
    }
}
