namespace CervezasColombia_CS_API_SQLite_Dapper.Models
{
    public class EnvasadoCerveza
    {
        public int Id { get; set; } = 0;
        public string Nombre { get; set; } = string.Empty;
        public int Unidad_Volumen_Id { get; set; } = 0;
        public string Unidad_Volumen { get; set; } = string.Empty;
        public float Volumen { get; set; } = 0.0f;
    }
}
