using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_SQLite_Dapper.Helpers
{
    public class BaseResponse
    {
        [JsonPropertyName("tipo")]
        public string? Tipo { get; set; } = string.Empty;

        [JsonPropertyName("totalElementos")]
        public int TotalElementos { get; set; }

        [JsonPropertyName("PaginaActual")]
        public int PaginaActual { get; set; }

        [JsonPropertyName("elementosPorPagina")]
        public int ElementosPorPagina { get; set; } // PageSize

        [JsonPropertyName("totalPaginas")]
        public int TotalPaginas { get; set; }
    }
}
