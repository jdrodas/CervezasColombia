using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;


namespace CervezasColombia_CS_API_Mongo.Models
{
    public class Ingrediente
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string? Id { get; set; } = string.Empty;

        [BsonElement("nombre")]
        [JsonPropertyName("nombre")]
        [BsonRepresentation(BsonType.String)]
        public string Nombre { get; set; } = string.Empty;

        [BsonElement("tipo_ingrediente_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("tipo_ingrediente_id")]
        public string? Tipo_Ingrediente_Id { get; set; } = string.Empty;

        [BsonElement("tipo_ingrediente")]
        [JsonPropertyName("tipo_ingrediente")]
        [BsonRepresentation(BsonType.String)]
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
                hash = hash * 5 + (Id?.GetHashCode() ?? 0);
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + (Tipo_Ingrediente?.GetHashCode() ?? 0);
                hash = hash * 5 + (Tipo_Ingrediente_Id?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}
