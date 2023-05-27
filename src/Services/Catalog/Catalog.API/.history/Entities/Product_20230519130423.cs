using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Entities
{
    public class Product
    {
        [BsonId]
        public int Id { get; set; }
    }
}