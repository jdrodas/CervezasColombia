using CervezasColombia_CS_API_SQLite_Dapper.Ingredientes;
using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_SQLite_Dapper.Cervezas
{
    public class CervezaDetallada : Cerveza
    {

        [JsonPropertyName("ingredientes")]
        public List<Ingrediente> Ingredientes { get; set; } = [];
    }
}
