using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_Mongo.Models
{
    public class IngredienteCerveza
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string? Id { get; set; } = string.Empty;

        [JsonPropertyName("cerveceria")]
        [BsonElement("cerveceria")]
        [BsonRepresentation(BsonType.String)]
        public string Cerveceria { get; set; } = string.Empty;

        [JsonPropertyName("cerveza")]
        [BsonElement("cerveza")]
        [BsonRepresentation(BsonType.String)]
        public string Cerveza { get; set; } = string.Empty;

        [BsonElement("tipo_ingrediente")]
        [JsonPropertyName("tipo_ingrediente")]
        [BsonRepresentation(BsonType.String)]
        public string Tipo_Ingrediente { get; set; } = string.Empty;

        [BsonElement("ingrediente")]
        [JsonPropertyName("ingrediente")]
        [BsonRepresentation(BsonType.String)]
        public string Ingrediente { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otroIngredienteCerveza = (IngredienteCerveza)obj;

            return Id == otroIngredienteCerveza.Id
                && Cerveceria.Equals(otroIngredienteCerveza.Cerveceria)
                && Cerveza.Equals(otroIngredienteCerveza.Cerveza)
                && Tipo_Ingrediente.Equals(otroIngredienteCerveza.Tipo_Ingrediente)
                && Ingrediente.Equals(otroIngredienteCerveza.Ingrediente);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + (Id?.GetHashCode() ?? 0);
                hash = hash * 5 + (Cerveceria?.GetHashCode() ?? 0);
                hash = hash * 5 + (Cerveza?.GetHashCode() ?? 0);
                hash = hash * 5 + (Tipo_Ingrediente?.GetHashCode() ?? 0);
                hash = hash * 5 + (Ingrediente?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}
