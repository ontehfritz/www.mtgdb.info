using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MtgDb.Info
{
    public class Profile
    {
        [BsonId]
        public Guid Id              { get; set; }
        [BsonElement]
        public string Email         { get; set; }
        [BsonElement]
        public string Name          { get; set; }
        [BsonElement]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt   { get; set; }
        [BsonElement]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ModifiedAt  { get; set; }
    }
}

