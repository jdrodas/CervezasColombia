namespace CervezasColombia_CS_API_PostgreSQL_Dapper.Models
{
    public class Ubicacion
    {
        public int Id { get; set; } = 0;
        public string Municipio { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public float Latitud { get; set; } = 0.0f;
        public float Longitud { get; set; } = 0.0f;
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraUbicacion = (Ubicacion)obj;

            return Id == otraUbicacion.Id
                   && Municipio.Equals(otraUbicacion.Municipio)
                   && Departamento.Equals(otraUbicacion.Departamento)
                   && Latitud.Equals(otraUbicacion.Latitud)
                   && Longitud.Equals(otraUbicacion.Longitud);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Municipio?.GetHashCode() ?? 0);
                hash = hash * 5 + (Departamento?.GetHashCode() ?? 0);
                hash = hash * 5 + Latitud.GetHashCode();
                hash = hash * 5 + Longitud.GetHashCode();

                return hash;
            }
        }
    }
}
