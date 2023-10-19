using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_Mongo.Models
{
    public class CervezaEnvasada : Cerveza
    {
        [BsonElement("envasado")]
        [JsonPropertyName("envasado")]
        [BsonRepresentation(BsonType.String)]
        public string Envasado { get; set; } = string.Empty;

        [BsonElement("unidad_volumen")]
        [JsonPropertyName("unidad_volumen")]
        [BsonRepresentation(BsonType.String)]
        public string Unidad_Volumen { get; set; } = string.Empty;

        [BsonElement("volumen")]
        [JsonPropertyName("volumen")]
        [BsonRepresentation(BsonType.Double)]
        public double Volumen { get; set; } = 0.0d;
    }
}
