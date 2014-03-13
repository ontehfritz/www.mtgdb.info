using System;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using MtgDb.Info.Driver;
using System.Configuration;

namespace MtgDb.Info
{
    public class CardChange : PageModel
    {
        private IRepository _repository; 

        [BsonId]
        public Guid Id                  { get; set; }
        [BsonElement]
        public Guid UserId              { get; set; }
        [BsonElement]
        public int Version              { get; set; } //0 - is the original 
        [BsonElement]
        public DateTime ModifiedAt      { get; set; }
        [BsonElement]
        public DateTime CreatedAt       { get; set; }
        [BsonElement]
        public string Comment           { get; set; }
        [BsonElement]
        public string Image             { get; set; }
        [BsonElement]
        public string[] FieldsUpdated   { get; set; }

        /*Card fields*/
        [BsonElement]
        public int Mvid                 { get; set; }  
        [BsonElement]
        public int SetNumber            { get; set; }
        [BsonElement]
        public string Name              { get; set; }
        [BsonElement]
        public string SearchName        { get; set; }  
        [BsonElement]
        public string Description       { get; set; }   
        [BsonElement]
        public string Flavor            { get; set; }   
        [BsonElement]
        public string[] Colors          { get; set; }   
        [BsonElement]
        public string ManaCost          { get; set; }    
        [BsonElement]
        public int ConvertedManaCost    { get; set; }   
        [BsonElement]
        public string CardSetName       { get; set; }  
        [BsonElement]
        public string Type              { get; set; }   
        [BsonElement]
        public string SubType           { get; set; }       
        [BsonElement]
        public int Power                { get; set; }      
        [BsonElement]
        public int Toughness            { get; set; }  
        [BsonElement]
        public int Loyalty              { get; set; }    
        [BsonElement]
        public string Rarity            { get; set; }  
        [BsonElement]
        public string Artist            { get; set; } 
        [BsonElement]
        public string CardSetId         { get; set; }     
        [BsonElement]
        public Ruling[] Rulings         { get; set; }    
        [BsonElement]
        public Format[] Formats         { get; set; }   
        [BsonElement]
        public DateTime ReleasedAt      { get; set; } 
        /*end of card fields*/

        public CardChange() : base(){}

        public static CardChange MapCard(Card card)
        {
            CardChange change = new CardChange ();
            change.Artist = card.Artist;
            change.CardSetId = card.CardSetId;
            change.CardSetName = card.CardSetName;
            change.Colors = card.Colors;
            change.ConvertedManaCost = card.ConvertedManaCost;
            change.Description = card.Description;
            change.Flavor = card.Flavor;
            change.Formats = card.Formats;
            change.Loyalty = card.Loyalty;
            change.ManaCost = card.ManaCost;
            change.Mvid = card.Id;
            change.Name = card.Name;
            change.Power = card.Power;
            change.Rarity = card.Rarity;
            change.ReleasedAt = card.ReleasedAt;
            change.Rulings = card.Rulings;
            change.SetNumber = card.SetNumber;
            change.SubType = card.SubType;
            change.Toughness = card.Toughness;
            change.Type = card.Type;
            change.Image = card.CardImage;

            return change;
        }

        public static string[] FieldsChanged(Card card, CardChange change)
        {
            List<string> fields = new List<string> ();

            if(change.Artist != card.Artist){ fields.Add("artist");}
            if(change.CardSetId != card.CardSetId){ fields.Add("cardSetId");}
            if(change.CardSetName != card.CardSetName){ fields.Add("cardSetName");}
            //if(change.Colors != card.Colors){ fields.Add("colors");}
            //if(change.ConvertedManaCost != card.ConvertedManaCost){ fields.Add("convertedManaCost");}
            if(change.Description != card.Description){ fields.Add("description");}
            if(change.Flavor != card.Flavor){ fields.Add("flavor");}
//          if(change.Formats != card.Formats){ fields.Add("formats");}
            if(change.Loyalty != card.Loyalty){ fields.Add("loyalty");}
            if(change.ManaCost!= card.ManaCost){ fields.Add("manaCost");}
            if(change.Name!= card.Name){ fields.Add("name");}
            if(change.Power != card.Power){ fields.Add("power");}
            if(change.Rarity != card.Rarity){ fields.Add("rarity");}
            if(change.ReleasedAt != card.ReleasedAt){ fields.Add("releasedAt");}
//           if(change.Rulings != card.Rulings){ fields.Add("rulings");}
            if(change.SetNumber != card.SetNumber){ fields.Add("setNumber");}
            if(change.SubType != card.SubType){ fields.Add("subType");}
            if(change.Toughness != card.Toughness){ fields.Add("toughness");}
            if(change.Type != card.Type){ fields.Add("type");}
//          change.Image = card.CardImage;

            return fields.ToArray();
        }
    }
}

