using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CervezasColombia_CS_PoC_Consola
{
    public class Estilo
    {
        [BsonId]
        public string? ObjectId { get; set; }

        [BsonElement("Id")]
        public int Id { get; set; } = 0;

        [BsonElement("nombre")]
        [BsonRepresentation(BsonType.String)]
        public string? Nombre { get; set; } = String.Empty;
    }
}
