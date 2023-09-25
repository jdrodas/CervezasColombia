namespace CervezasColombia_CS_API_PostgreSQL_Dapper.Models
{
    public class EnvasadoCerveza
    {
        public int Id { get; set; } = 0;
        public string Nombre { get; set; } = string.Empty;
        public int Unidad_Volumen_Id { get; set; } = 0;
        public string Unidad_Volumen { get; set; } = string.Empty;
        public int Volumen { get; set; } = 0;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otroEnvasadoCerveza = (EnvasadoCerveza)obj;

            return Id == otroEnvasadoCerveza.Id
                && Nombre.Equals(otroEnvasadoCerveza.Nombre)
                && Unidad_Volumen_Id == otroEnvasadoCerveza.Unidad_Volumen_Id
                && Unidad_Volumen.Equals(otroEnvasadoCerveza.Unidad_Volumen)
                && Volumen.Equals(otroEnvasadoCerveza.Volumen);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + (Unidad_Volumen?.GetHashCode() ?? 0);
                hash = hash * 5 + Unidad_Volumen_Id.GetHashCode();
                hash = hash * 5 + Volumen.GetHashCode();

                return hash;
            }
        }
    }
}
