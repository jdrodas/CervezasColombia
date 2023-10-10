using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_PostgreSQL_Dapper.Models
{
    public class Cerveza
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("cerveceria_id")]
        public int Cerveceria_id { get; set; } = 0;

        [JsonPropertyName("cerveceria")]
        public string Cerveceria { get; set; } = string.Empty;

        [JsonPropertyName("estilo_id")]
        public int Estilo_id { get; set; } = 0;

        [JsonPropertyName("estilo")]
        public string Estilo { get; set; } = string.Empty;

        [JsonPropertyName("rango_ibu")]
        public string Rango_Ibu { get; set; } = string.Empty;

        [JsonPropertyName("ibu")]
        public float Ibu { get; set; } = 0f;

        [JsonPropertyName("rango_abv")]
        public string Rango_Abv { get; set; } = string.Empty;

        [JsonPropertyName("abv")]
        public float Abv { get; set; } = 0f;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraCerveza = (Cerveza)obj;

            return Id == otraCerveza.Id
                && Nombre.Equals(otraCerveza.Nombre)
                && Cerveceria_id == otraCerveza.Cerveceria_id
                && Cerveceria.Equals(otraCerveza.Cerveceria)
                && Estilo_id == otraCerveza.Estilo_id
                && Estilo.Equals(otraCerveza.Estilo)
                && Ibu.Equals(otraCerveza.Ibu)
                && Abv.Equals(otraCerveza.Abv);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + (Cerveceria?.GetHashCode() ?? 0);
                hash = hash * 5 + (Estilo?.GetHashCode() ?? 0);
                hash = hash * 5 + Cerveceria_id.GetHashCode();
                hash = hash * 5 + Estilo_id.GetHashCode();
                hash = hash * 5 + Ibu.GetHashCode();
                hash = hash * 5 + Abv.GetHashCode();

                return hash;
            }
        }
    }
}