namespace CervezasColombia_CS_API_PostgreSQL_Dapper.Models
{
    public class EnvasadoCerveza
    {
        public int Id { get; set; } = 0;
        public string Nombre { get; set; } = string.Empty;
        public int Unidad_Volumen_id { get; set; } = 0;
        public string Unidad_Volumen { get; set; } = string.Empty;
        public int Volumen { get; set; } = 0;
    }
}
