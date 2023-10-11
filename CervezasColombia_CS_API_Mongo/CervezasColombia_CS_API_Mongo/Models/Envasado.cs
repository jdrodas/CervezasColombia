using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_Mongo.Models
{
    public class Envasado
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string? Id { get; set; } = string.Empty;

        [BsonElement("nombre")]
        [JsonPropertyName("nombre")]
        [BsonRepresentation(BsonType.String)]
        public string Nombre { get; set; } = string.Empty;
    }
}
