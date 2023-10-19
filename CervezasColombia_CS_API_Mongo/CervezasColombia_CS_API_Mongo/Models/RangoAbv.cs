using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_Mongo.Models
{
    public class RangoAbv
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string? Id { get; set; } = string.Empty;

        [BsonElement("nombre")]
        [JsonPropertyName("nombre")]
        [BsonRepresentation(BsonType.String)]
        public string Nombre { get; set; } = string.Empty;

        [BsonElement("valor_inicial")]
        [JsonPropertyName("valor_inicial")]
        [BsonRepresentation(BsonType.Double)]
        public double ValorInicial { get; set; } = 0d;

        [BsonElement("valor_final")]
        [JsonPropertyName("valor_final")]
        [BsonRepresentation(BsonType.Double)]
        public double ValorFinal { get; set; } = 0d;
    }
}
