using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_SQLite_Dapper.Models
{
    public class IngredienteCerveza
    {
        [JsonPropertyName("cerveceria")]
        public string Cerveceria { get; set; } = string.Empty;

        [JsonPropertyName("cerveza")]
        public string Cerveza { get; set; } = string.Empty;

        [JsonPropertyName("tipo_ingrediente")]
        public string Tipo_Ingrediente { get; set; } = string.Empty;

        [JsonPropertyName("ingrediente")]
        public string Ingrediente { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otroIngredienteCerveza = (IngredienteCerveza)obj;

            return Cerveceria.Equals(otroIngredienteCerveza.Cerveceria)
                && Cerveza.Equals(otroIngredienteCerveza.Cerveza)
                && Tipo_Ingrediente.Equals(otroIngredienteCerveza.Tipo_Ingrediente)
                && Ingrediente.Equals(otroIngredienteCerveza.Ingrediente);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + (Cerveceria?.GetHashCode() ?? 0);
                hash = hash * 5 + (Cerveza?.GetHashCode() ?? 0);
                hash = hash * 5 + (Tipo_Ingrediente?.GetHashCode() ?? 0);
                hash = hash * 5 + (Ingrediente?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}
