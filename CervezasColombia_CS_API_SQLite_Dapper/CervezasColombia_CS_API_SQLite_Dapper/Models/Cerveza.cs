namespace CervezasColombia_CS_API_SQLite_Dapper.Models
{
    public class Cerveza
    {
        public int Id { get; set; } = 0;
        public string Nombre { get; set; } = string.Empty;
        public int Cerveceria_id { get; set; } = 0;
        public string Cerveceria { get; set; } = string.Empty;
        public int Estilo_id { get; set; } = 0;
        public string Estilo { get; set; } = string.Empty;
        public string Rango_Ibu { get; set; } = string.Empty;
        public float Ibu { get; set; } = 0f;
        public string Rango_Abv { get; set; } = string.Empty;
        public float Abv { get; set; } = 0f;
    }
}