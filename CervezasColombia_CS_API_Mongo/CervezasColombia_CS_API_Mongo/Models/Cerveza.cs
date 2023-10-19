using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CervezasColombia_CS_API_Mongo.Models
{
    public class Cerveza
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string? Id { get; set; } = string.Empty;

        [BsonElement("nombre")]
        [JsonPropertyName("nombre")]
        [BsonRepresentation(BsonType.String)]
        public string Nombre { get; set; } = string.Empty;

        [BsonElement("cerveceria_id")]
        [JsonPropertyName("cerveceria_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Cerveceria_id { get; set; } = null;

        [BsonElement("cerveceria")]
        [JsonPropertyName("cerveceria")]
        [BsonRepresentation(BsonType.String)]
        public string Cerveceria { get; set; } = string.Empty;

        [BsonElement("estilo_id")]
        [JsonPropertyName("estilo_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Estilo_id { get; set; } = null;

        [BsonElement("estilo")]
        [JsonPropertyName("estilo")]
        [BsonRepresentation(BsonType.String)]
        public string Estilo { get; set; } = string.Empty;

        [BsonElement("rango_ibu")]
        [JsonPropertyName("rango_ibu")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [BsonRepresentation(BsonType.String)]
        public string? Rango_Ibu { get; set; } = null;

        [BsonElement("ibu")]
        [JsonPropertyName("ibu")]
        [BsonRepresentation(BsonType.Double)]
        public double Ibu { get; set; } = 0d;

        [BsonElement("rango_abv")]
        [JsonPropertyName("rango_abv")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [BsonRepresentation(BsonType.String)]
        public string? Rango_Abv { get; set; } = null;

        [BsonElement("abv")]
        [JsonPropertyName("abv")]
        [BsonRepresentation(BsonType.Double)]
        public double Abv { get; set; } = 0d;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraCerveza = (Cerveza)obj;

            return Id == otraCerveza.Id
                && Nombre.Equals(otraCerveza.Nombre)
                && Cerveceria_id == otraCerveza.Cerveceria_id
                && Cerveceria.Equals(otraCerveza.Cerveceria)
                && Estilo_id == otraCerveza.Estilo_id
                && Estilo.Equals(otraCerveza.Estilo)
                && Ibu.Equals(otraCerveza.Ibu)
                && Abv.Equals(otraCerveza.Abv);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + (Id?.GetHashCode() ?? 0);
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + (Cerveceria?.GetHashCode() ?? 0);
                hash = hash * 5 + (Estilo?.GetHashCode() ?? 0);
                hash = hash * 5 + (Cerveceria_id?.GetHashCode() ?? 0);
                hash = hash * 5 + (Estilo_id?.GetHashCode() ?? 0);
                hash = hash * 5 + Ibu.GetHashCode();
                hash = hash * 5 + Abv.GetHashCode();

                return hash;
            }
        }
    }
}