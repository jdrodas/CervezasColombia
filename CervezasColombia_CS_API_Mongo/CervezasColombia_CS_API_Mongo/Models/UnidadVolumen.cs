using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_Mongo.Models
{
    public class UnidadVolumen
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string? Id { get; set; } = string.Empty;

        [BsonElement("nombre")]
        [JsonPropertyName("nombre")]
        [BsonRepresentation(BsonType.String)]
        public string Nombre { get; set; } = string.Empty;

        [BsonElement("abreviatura")]
        [JsonPropertyName("abreviatura")]
        [BsonRepresentation(BsonType.String)]
        public string Abreviatura { get; set; } = string.Empty;
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraUnidadVolumen = (UnidadVolumen)obj;

            return Id == otraUnidadVolumen.Id
                   && Nombre.Equals(otraUnidadVolumen.Nombre)
                   && Abreviatura.Equals(otraUnidadVolumen.Abreviatura);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + (Id?.GetHashCode() ?? 0);
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + (Abreviatura?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}
