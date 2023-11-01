using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CervezasColombia_CS_PoC_Consola
{
    public class Estilo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string? ObjectId { get; set; }

        [BsonElement("Id")]
        public int Id { get; set; } = 0;

        [BsonElement("nombre")]
        [JsonPropertyName("nombre")]
        [BsonRepresentation(BsonType.String)]
        public string? Nombre { get; set; } = String.Empty;
    }
}
