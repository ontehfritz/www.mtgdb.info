using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace MtgDb.Info
{
    public class UserCard
    {
        [BsonId]
        public Guid Id              { get; set; }
        [BsonElement]
        public int MultiverseId     { get; set; }
        [BsonElement]
        public string SetId         { get; set; }
        [BsonElement]
        public Guid PlaneswalkerId  { get; set; }
        [BsonElement]
        public int Amount           { get; set; }
    }
}

