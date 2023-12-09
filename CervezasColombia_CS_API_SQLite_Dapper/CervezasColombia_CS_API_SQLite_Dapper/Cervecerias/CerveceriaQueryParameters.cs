using CervezasColombia_CS_API_SQLite_Dapper.Helpers;

namespace CervezasColombia_CS_API_SQLite_Dapper.Cervecerias
{
    public class CerveceriaQueryParameters: QueryParameters
    {
        static readonly List<string> criteriosValidos =
            ["nombre", "instagram"];

        public string? Instagram { get; set; }
    }
}
