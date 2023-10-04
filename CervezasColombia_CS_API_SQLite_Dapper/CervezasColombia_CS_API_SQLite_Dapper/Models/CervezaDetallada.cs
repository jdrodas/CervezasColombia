namespace CervezasColombia_CS_API_SQLite_Dapper.Models
{
    public class CervezaDetallada : Cerveza
    {
        public List<EnvasadoCerveza> Envasados { get; set; } = new List<EnvasadoCerveza>();
        public List<Ingrediente> Ingredientes { get; set; } = new List<Ingrediente>();
    }
}
