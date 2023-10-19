using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_Mongo.Models
{
    public class Cerveceria
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string? Id { get; set; } = string.Empty;

        [BsonElement("nombre")]
        [JsonPropertyName("nombre")]
        [BsonRepresentation(BsonType.String)]
        public string Nombre { get; set; } = string.Empty;

        [BsonElement("sitio_web")]
        [JsonPropertyName("sitio_web")]
        [BsonRepresentation(BsonType.String)]
        public string Sitio_Web { get; set; } = string.Empty;

        [BsonElement("instagram")]
        [JsonPropertyName("instagram")]
        [BsonRepresentation(BsonType.String)]
        public string Instagram { get; set; } = string.Empty;

        [BsonElement("ubicacion")]
        [JsonPropertyName("ubicacion")]
        [BsonRepresentation(BsonType.String)]
        public string Ubicacion { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraCerveceria = (Cerveceria)obj;

            return Id == otraCerveceria.Id
                   && Nombre.Equals(otraCerveceria.Nombre)
                   && Sitio_Web.Equals(otraCerveceria.Sitio_Web)
                   && Instagram.Equals(otraCerveceria.Instagram)
                   && Ubicacion.Equals(otraCerveceria.Ubicacion);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + (Id?.GetHashCode() ?? 0);
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + (Sitio_Web?.GetHashCode() ?? 0);
                hash = hash * 5 + (Instagram?.GetHashCode() ?? 0);
                hash = hash * 5 + (Ubicacion?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}