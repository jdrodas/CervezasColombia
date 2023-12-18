using CervezasColombia_CS_API_SQLite_Dapper.Helpers;

namespace CervezasColombia_CS_API_SQLite_Dapper.Cervezas
{
    public class CervezaQueryParameters : BaseQueryParameters
    {
        private static new readonly List<string> criteriosValidos =
            ["nombre", "cerveceria"];

        public string? Cerveceria { get; set; }
    }
}
