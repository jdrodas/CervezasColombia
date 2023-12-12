using CervezasColombia_CS_API_SQLite_Dapper.Cervecerias;
using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_SQLite_Dapper.Ubicaciones
{
    public class UbicacionDetallada : Ubicacion
    {
        [JsonPropertyName("cervecerias")]
        public List<Cerveceria> Cervecerias { get; set; } = [];
    }
}