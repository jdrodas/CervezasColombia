using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CervezasColombia_CS_PoC_Consola
{
    public class Estilo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ObjectId { get; set; }
        
        [BsonElement("Id")]
        public int Id { get; set; } = 0;
        
        [BsonElement("Nombre")]
        public string? Nombre { get; set; } = String.Empty;
    }
}
