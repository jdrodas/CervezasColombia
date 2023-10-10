using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;


namespace CervezasColombia_CS_API_Mongo.Models
{
    public class Ubicacion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string? Id { get; set; } = string.Empty;

        [BsonElement("municipio")]
        [JsonPropertyName("municipio")]
        [BsonRepresentation(BsonType.String)]
        public string Municipio { get; set; } = string.Empty;

        [BsonElement("departamento")]
        [JsonPropertyName("departamento")]
        [BsonRepresentation(BsonType.String)]
        public string Departamento { get; set; } = string.Empty;

        [BsonElement("latitud")]
        [JsonPropertyName("latitud")]
        [BsonRepresentation(BsonType.Double)]
        public double Latitud { get; set; } = 0.0d;

        [BsonElement("longitud")]
        [JsonPropertyName("longitud")]
        [BsonRepresentation(BsonType.Double)] 
        public double Longitud { get; set; } = 0.0d;
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraUbicacion = (Ubicacion)obj;

            return Id == otraUbicacion.Id
                   && Municipio.Equals(otraUbicacion.Municipio)
                   && Departamento.Equals(otraUbicacion.Departamento)
                   && Latitud.Equals(otraUbicacion.Latitud)
                   && Longitud.Equals(otraUbicacion.Longitud);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Municipio?.GetHashCode() ?? 0);
                hash = hash * 5 + (Departamento?.GetHashCode() ?? 0);
                hash = hash * 5 + Latitud.GetHashCode();
                hash = hash * 5 + Longitud.GetHashCode();

                return hash;
            }
        }
    }
}
