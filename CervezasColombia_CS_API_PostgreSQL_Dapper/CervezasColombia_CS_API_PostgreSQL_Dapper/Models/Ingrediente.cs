using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_PostgreSQL_Dapper.Models
{
    public class Ingrediente
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("tipo_ingrediente_id")]
        public int Tipo_Ingrediente_Id { get; set; } = 0;

        [JsonPropertyName("tipo_ingrediente")]
        public string Tipo_Ingrediente { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otroIngrediente = (Ingrediente)obj;

            return Id == otroIngrediente.Id
                   && Nombre.Equals(otroIngrediente.Nombre)
                   && Tipo_Ingrediente_Id == otroIngrediente.Tipo_Ingrediente_Id
                   && Tipo_Ingrediente.Equals(otroIngrediente.Tipo_Ingrediente);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + (Tipo_Ingrediente?.GetHashCode() ?? 0);
                hash = hash * 5 + Tipo_Ingrediente_Id.GetHashCode();

                return hash;
            }
        }
    }
}
