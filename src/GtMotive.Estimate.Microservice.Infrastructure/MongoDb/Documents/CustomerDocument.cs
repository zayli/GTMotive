using MongoDB.Bson.Serialization.Attributes;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Documents
{
    public sealed class CustomerDocument
    {
        [BsonId]
        public string Id { get; set; }

        public string Name { get; set; }

        public string DocumentId { get; set; }
    }
}
