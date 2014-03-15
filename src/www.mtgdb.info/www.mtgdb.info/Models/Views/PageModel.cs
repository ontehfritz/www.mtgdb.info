using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace MtgDb.Info
{
    public class PageModel
    {
        [BsonIgnore]
        public List<string> Messages        { get; set; }
        [BsonIgnore]
        public List<string> Errors          { get; set; }
        [BsonIgnore]
        public List<string> Information     { get; set; }
        [BsonIgnore]
        public List<string> Warnings        { get; set; }
        [BsonIgnore]
        public string Title                 { get; set; }
        [BsonIgnore]
        public Planeswalker Planeswalker    { get; set; }
        [BsonIgnore]
        public string ActiveMenu            { get; set; }

        public PageModel ()
        {
            Messages = new List<string> ();
            Errors = new List<string> ();
            Information = new List<string> ();
        }
    }
}

