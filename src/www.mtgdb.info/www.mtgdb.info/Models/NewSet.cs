using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MtgDb.Info
{
    public class NewSet : PageModel
    {
        [BsonId]
        public Guid Id                      { get; set; }
        [BsonElement]
        public Guid UserId                  { get; set; }
        [BsonElement]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ModifiedAt          { get; set; }
        [BsonElement]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt           { get; set; }
        [BsonElement]
        public string Comment               { get; set; }
        //Approved
        //Pending
        //Closed
        [BsonElement]
        public string Status                { get; set; }
        [BsonElement]
        public string SetId                 { get; set; }
        [BsonElement]
        public string Name                  { get; set; }
        [BsonElement]
        public string Block                 { get; set; }
        [BsonElement]
        public string Type                  { get; set; }
        [BsonElement]
        public string Description           { get; set; }
        [BsonElement]
        public int Common                   { get; set; }
        [BsonElement]
        public int Uncommon                 { get; set; }
        [BsonElement]
        public int Rare                     { get; set; }
        [BsonElement]
        public int MythicRare               { get; set; }
        [BsonElement]
        public int BasicLand                { get; set; }
        [BsonElement]
        public string ReleasedAt            { get; set; }
    }
}

