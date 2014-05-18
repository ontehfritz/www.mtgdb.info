using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MtgDb.Info
{
    public class NewCard : PageModel
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
        /*Card fields*/
        [BsonElement]
        public int Mvid                     { get; set; }  
        [BsonElement]
        public int RelatedCardId            { get; set; }  
        [BsonElement]
        public int SetNumber                { get; set; }
        [BsonElement]
        public string Name                  { get; set; }
        [BsonElement]
        public string Description           { get; set; }   
        [BsonElement]
        public string Flavor                { get; set; }   
        [BsonElement]
        public string[] Colors              { get; set; }   
        [BsonElement]
        public string ManaCost              { get; set; }    
        [BsonElement]
        public int ConvertedManaCost        { get; set; }   
        [BsonElement]
        public string Type                  { get; set; }   
        [BsonElement]
        public string SubType               { get; set; }       
        [BsonElement]
        public bool Token                   { get; set; }       
        [BsonElement]
        public int Power                    { get; set; }      
        [BsonElement]
        public int Toughness                { get; set; }  
        [BsonElement]
        public int Loyalty                  { get; set; }    
        [BsonElement]
        public string Rarity                { get; set; }  
        [BsonElement]
        public string Artist                { get; set; } 
        [BsonElement]
        public string CardSetId             { get; set; }     
        [BsonElement]
        public string ReleasedAt            { get; set; } 
        /*end of card fields*/
    }
}

