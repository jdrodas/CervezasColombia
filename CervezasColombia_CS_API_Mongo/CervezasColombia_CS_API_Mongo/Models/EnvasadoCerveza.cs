using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CervezasColombia_CS_API_Mongo.Models
{
    public class EnvasadoCerveza
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

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otroEnvasadoCerveza = (EnvasadoCerveza)obj;

            return Id == otroEnvasadoCerveza.Id
                && Cerveceria.Equals(otroEnvasadoCerveza.Cerveceria)
                && Cerveza.Equals(otroEnvasadoCerveza.Cerveza)
                && Envasado.Equals(otroEnvasadoCerveza.Envasado)
                && Unidad_Volumen.Equals(otroEnvasadoCerveza.Unidad_Volumen)
                && Volumen.Equals(otroEnvasadoCerveza.Volumen);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + (Id?.GetHashCode() ?? 0);
                hash = hash * 5 + (Cerveceria?.GetHashCode() ?? 0);
                hash = hash * 5 + (Cerveza?.GetHashCode() ?? 0);
                hash = hash * 5 + (Envasado?.GetHashCode() ?? 0);
                hash = hash * 5 + (Unidad_Volumen?.GetHashCode() ?? 0);
                hash = hash * 5 + Volumen.GetHashCode();

                return hash;
            }
        }
    }
}
