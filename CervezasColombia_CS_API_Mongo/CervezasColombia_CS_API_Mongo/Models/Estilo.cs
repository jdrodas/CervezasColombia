using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_Mongo.Models
{
    public class Estilo
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
