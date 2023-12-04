using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_SQLite_Dapper.Unidades
{
    public class Unidad
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("abreviatura")]
        public string Abreviatura { get; set; } = string.Empty;
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraUnidad = (Unidad)obj;

            return Id == otraUnidad.Id
                   && Nombre.Equals(otraUnidad.Nombre)
                   && Abreviatura.Equals(otraUnidad.Abreviatura);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + (Abreviatura?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}
