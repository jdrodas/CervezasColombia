namespace CervezasColombia_CS_API_SQLite_Dapper.Data.Entities
{
    public class Cerveza
    {
        public int Id { get; set; } = 0;
        public string Nombre { get; set; } = string.Empty;
        public string Cerveceria { get; set; } = string.Empty;
        public string Estilo { get; set; } = string.Empty;
        public float Ibu { get; set; } = 0f;
        public float Abv { get; set; } = 0f;
    }
}