using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace MtgDb.Info
{
    public class Deck
    {
        [BsonId]
        public Guid Id                  { get; set; }
        [BsonElement]
        public Guid UserId              { get; set; }
        [BsonElement]
        public string Name              { get; set; }
        [BsonElement]
        public string Description       { get; set; }
        [BsonElement]
        public bool IsPublic            { get; set; }
        [BsonElement]
        public List<DeckCard> Cards     { get; set; }
        [BsonElement]
        public List<DeckCard> SideBar   { get; set; }
        [BsonElement]
        public DateTime CreatedAt       { get; set; }
        [BsonElement]
        public DateTime ModifiedAt      { get; set; }

        public Deck ()
        {
            Cards =     new List<DeckCard>();
            SideBar =   new List<DeckCard>();
        }
    }

    public class DeckCard
    {
        [BsonElement]
        public int MultiverseId { get; set; }
        [BsonElement]
        public int Amount       { get; set; }
    }
}

