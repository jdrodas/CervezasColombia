using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_SQLite_Dapper.Models
{
    public class EnvasadoCerveza
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("cerveceria")]
        public string Cerveceria { get; set; } = string.Empty;

        [JsonPropertyName("cerveza")]
        public string Cerveza { get; set; } = string.Empty;

        [JsonPropertyName("envasado")]
        public string Envasado { get; set; } = string.Empty;

        [JsonPropertyName("unidad_volumen")]
        public string Unidad_Volumen { get; set; } = string.Empty;

        public int Unidad_Volumen_Id { get; set; } = 0;

        [JsonPropertyName("volumen")]
        public double Volumen { get; set; } = 0.0d;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otroEnvasadoCerveza = (EnvasadoCerveza)obj;

            return Id == otroEnvasadoCerveza.Id
                && Cerveceria.Equals(otroEnvasadoCerveza.Cerveceria)
                && Cerveza.Equals(otroEnvasadoCerveza.Cerveza)
                && Envasado.Equals(otroEnvasadoCerveza.Envasado)
                && Unidad_Volumen.Equals(otroEnvasadoCerveza.Unidad_Volumen)
                && Volumen.Equals(otroEnvasadoCerveza.Volumen);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Cerveceria?.GetHashCode() ?? 0);
                hash = hash * 5 + (Cerveza?.GetHashCode() ?? 0);
                hash = hash * 5 + (Envasado?.GetHashCode() ?? 0);
                hash = hash * 5 + (Unidad_Volumen?.GetHashCode() ?? 0);
                hash = hash * 5 + Volumen.GetHashCode();

                return hash;
            }
        }
    }
}
