using System;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using MtgDb.Info.Driver;
using System.Configuration;
using System.Reflection;
using FluentValidation;

namespace MtgDb.Info
{
    public class CardChangeValidator : AbstractValidator<CardChange>
    {
        public CardChangeValidator()
        {
            RuleFor(change => change.Artist).NotEmpty();
            RuleFor(change => change.CardSetId).NotEmpty();
            RuleFor(change => change.CardSetName).NotEmpty();
            RuleFor(change => change.Colors).NotEmpty();

            RuleFor(change => change.ConvertedManaCost)
                .GreaterThanOrEqualTo(0);
            RuleFor(change => change.Power)
                .GreaterThanOrEqualTo(0);


            RuleFor(change => change.Toughness)
                .GreaterThanOrEqualTo(0);
                
            RuleFor(change => change.Name).NotEmpty();
            RuleFor(change => change.Type).NotEmpty();
            RuleFor(change => change.Comment).NotEmpty();
            RuleFor(change => change.RelatedCardId)
                .GreaterThanOrEqualTo(0);
        }
    }

    public class CardChange : PageModel
    {
        [BsonId]
        public Guid Id                      { get; set; }
        [BsonElement]
        public Guid UserId                  { get; set; }
        [BsonElement]
        public int Version                  { get; set; } //0 - is the original 
        [BsonElement]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ModifiedAt          { get; set; }
        [BsonElement]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt           { get; set; }
        [BsonElement]
        public string Comment               { get; set; }
        [BsonElement]
        public string[] FieldsUpdated       { get; set; }
        [BsonElement]
        public string[] ChangesCommitted    { get; set; }
        //declined
        //approved
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
        public string SearchName            { get; set; }  
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
        public string CardSetName           { get; set; }  
        [BsonElement]
        public string Type                  { get; set; }   
        [BsonElement]
        public string SubType               { get; set; }       
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
        public Ruling[] Rulings             { get; set; }    
        [BsonElement]
        public Format[] Formats             { get; set; }   
        [BsonElement]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ReleasedAt      { get; set; } 
        /*end of card fields*/

        [BsonIgnore]
        public Profile User { get; set; }

        public CardChange() : base(){}

        public static CardChange MapCard(Card card)
        {
            CardChange change =         new CardChange ();
            change.Artist =             card.Artist;
            change.CardSetId =          card.CardSetId;
            change.CardSetName =        card.CardSetName;
            change.Colors =             card.Colors;
            change.ConvertedManaCost =  card.ConvertedManaCost;
            change.Description =        card.Description;
            change.Flavor =             card.Flavor;
            change.Formats =            card.Formats;
            change.Loyalty =            card.Loyalty;
            change.ManaCost =           card.ManaCost;
            change.Mvid =               card.Id;
            change.Name =               card.Name;
            change.Power =              card.Power;
            change.Rarity =             card.Rarity;
            change.ReleasedAt =         card.ReleasedAt;
            change.Rulings =            card.Rulings;
            change.SetNumber =          card.SetNumber;
            change.SubType =            card.SubType;
            change.Toughness =          card.Toughness;
            change.Type =               card.Type;
            change.RelatedCardId =      card.RelatedCardId;
            //change.Image = card.CardImage;

            return change;
        }

        public bool IsAccepted(string field)
        {
            if(ChangesCommitted == null)
            {
                return false;
            }

            foreach(string change in ChangesCommitted)
            {
                if(field.ToLower() == change.ToLower())
                {
                    return true;
                }
            }

            return false; 
        }

        /// <summary>
        /// Field must be defined for cards object in api.mtgdb.info project
        /// and also in Helper.cs
        /// </summary>
        /// <returns>The changed.</returns>
        /// <param name="card">Card.</param>
        /// <param name="change">Change.</param>
        public static string[] FieldsChanged(Card card, CardChange change)
        {
            List<string> fields = new List<string> ();

            if(change.Artist != card.Artist){ fields.Add("artist");}
            //if(change.CardSetId != card.CardSetId){ fields.Add("cardSetId");}
            //if(change.CardSetName != card.CardSetName){ fields.Add("cardSetName");}
            if(IsColorChanged(change.Colors, card.Colors)){ fields.Add("colors");}
            if(change.ConvertedManaCost != card.ConvertedManaCost){ fields.Add("convertedManaCost");}
            if(change.RelatedCardId != card.RelatedCardId){ fields.Add("relatedCardId"); }

            change.Description = change.Description ?? "";
            card.Description = card.Description ?? "";

            if(change.Description.Replace("\r",string.Empty) != 
                card.Description.Replace("\r",string.Empty))
            { fields.Add("description");}


            change.Flavor = change.Flavor ?? "";
            card.Flavor = card.Flavor ?? "";

            if(change.Flavor.Replace("\r",string.Empty) != 
                card.Flavor.Replace("\r",string.Empty)){ fields.Add("flavor"); }


            if(IsFormatChange(card.Formats,change.Formats)){ fields.Add("formats");}
            if(change.Loyalty != card.Loyalty){ fields.Add("loyalty");}
            if(change.ManaCost!= card.ManaCost){ fields.Add("manaCost");}
            //if(change.Name!= card.Name){ fields.Add("name");}
            if(change.Power != card.Power){ fields.Add("power");}
            //if(change.Rarity != card.Rarity){ fields.Add("rarity");}
            if(change.ReleasedAt.ToShortDateString() != 
                card.ReleasedAt.ToShortDateString()){ fields.Add("releasedAt"); }

            if(IsRuleChange(card.Rulings, change.Rulings)){ fields.Add("rulings"); }

            if(change.SetNumber != card.SetNumber){ fields.Add("setNumber");}
            if(change.SubType != card.SubType){ fields.Add("subType");}
            if(change.Toughness != card.Toughness){ fields.Add("toughness");}
            if(change.Type != card.Type){ fields.Add("type");}

            return fields.ToArray();
        }

        private static bool IsColorChanged(string[] colors1, 
            string[] colors2)
        {
            bool changed = false;

            if(colors1.Length != colors2.Length)
            {
                return true;
            }

            foreach(string color in colors1)
            {
                foreach(string color2 in colors2)
                {
                    if(color2 == color)
                    {
                        changed = false;
                        break;
                    }
                    else
                    {
                        changed = true;
                    }
                }

                if(changed)
                {
                    return true;
                }
            }

            return changed;
        }


        private static bool IsRuleChange(Ruling[] rulings1, 
            Ruling[] rulings2)
        {
            bool changed = false;

            if(rulings1.Length != rulings2.Length)
            {
                return true;
            }

            foreach(Ruling ruling in rulings1)
            {
                foreach(Ruling ruling2 in rulings2)
                {
                    if(ruling2.Rule == ruling.Rule
                        && ruling2.ReleasedAt.ToShortDateString() == ruling.ReleasedAt.ToShortDateString())
                    {
                        changed = false;
                        break;
                    }
                    else
                    {
                        changed = true;
                    }
                }

                if(changed)
                {
                    return true;
                }
            }

            return changed;
        }

        private static bool IsFormatChange(Format[] formats1, 
            Format[] formats2)
        {
            bool changed = false;

            if(formats1.Length != formats2.Length)
            {
                return true;
            }

            foreach(Format format in formats1)
            {
                foreach(Format format2 in formats2)
                {
                    if(format2.Name == format.Name
                        && format2.Legality == format.Legality)
                    {
                        changed = false;
                        break;
                    }
                    else
                    {
                        changed = true;
                    }
                }

                if(changed)
                {
                    return true;
                }
            }

            return changed;
        }

        public bool IsFieldChanged(string name)
        {
            name = name.ToLower();
            foreach(string field in FieldsUpdated)
            {
                if(field.ToLower() == name)
                {
                    return true;
                }
            }
            return false;
        }

        public string GetFieldValue(string field)
        {
            Type type = this.GetType();
            dynamic value = type.GetProperty(field, BindingFlags.IgnoreCase |  BindingFlags.Public |
                BindingFlags.Instance).GetValue(this, null);

            if(value == null)
            {
                return null;
            }

            if(field.ToLower() == "colors")
            {
                return string.Join(",",(string[])value);
            }

            return value.ToString();
        }

        public string FieldState(string field)
        {
            string state = "";

            if(Status == "Closed")
            {
                state = "closed";
            }
            else if(this.IsAccepted(field))
            {
                state = "accepted";
            }
            else if(Version != 0 && this.IsFieldChanged(field))
            {
                state = "changed";
            }

            return state;
        }
    }
}
