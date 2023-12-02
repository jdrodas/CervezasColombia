using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_SQLite_Dapper.Models
{
    public class Cerveceria
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("instagram")]
        public string Instagram { get; set; } = string.Empty;

        [JsonPropertyName("ubicacion")]
        public Ubicacion Ubicacion { get; set; } = new();

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraCerveceria = (Cerveceria)obj;

            return Id == otraCerveceria.Id
                   && Nombre.Equals(otraCerveceria.Nombre)
                   && Instagram.Equals(otraCerveceria.Instagram)
                   && Ubicacion.Equals(otraCerveceria.Ubicacion);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + (Instagram?.GetHashCode() ?? 0);
                hash = hash * 5 + Ubicacion.GetHashCode();

                return hash;
            }
        }
    }
}