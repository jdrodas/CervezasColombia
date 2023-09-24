namespace CervezasColombia_CS_API_PostgreSQL_Dapper.Models
{
    public class UnidadVolumen
    {
        public int Id { get; set; } = 0;
        public string Nombre { get; set; } = string.Empty;
        public string Abreviatura { get; set; } = string.Empty;
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraUnidadVolumen = (UnidadVolumen)obj;

            return Id == otraUnidadVolumen.Id
                   && Nombre.Equals(otraUnidadVolumen.Nombre)
                   && Abreviatura.Equals(otraUnidadVolumen.Abreviatura);
        }
    }
}
