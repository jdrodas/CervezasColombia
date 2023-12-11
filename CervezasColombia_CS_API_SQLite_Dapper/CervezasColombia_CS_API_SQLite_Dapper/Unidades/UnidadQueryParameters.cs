using CervezasColombia_CS_API_SQLite_Dapper.Helpers;

namespace CervezasColombia_CS_API_SQLite_Dapper.Unidades
{
    public class UnidadQueryParameters : BaseQueryParameters
    {
        private static new readonly List<string> criteriosValidos = ["nombre", "abreviatura"];

        public string? Abreviatura { get; set; }
    }
}