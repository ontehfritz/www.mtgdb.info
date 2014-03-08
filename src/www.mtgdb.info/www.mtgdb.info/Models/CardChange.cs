using System;

namespace MtgDb.Info
{
    public class CardChange : PageModel
    {
        public Guid Id                  { get; set; }
        public Guid UserId              { get; set; }
        public int Number               { get; set; } //0 - is the original 
        public DateTime ModifiedAt      { get; set; }
        public DateTime CreatedAt       { get; set; }
        public string Comment           { get; set; }
        public string Image             { get; set; }
        public string[] FieldsUpdated   { get; set; }

        /*Card fields*/
        public int Mvid                 { get; set; }             
        public int SetNumber            { get; set; }
        public string Name              { get; set; }
        public string SearchName        { get; set; }             
        public string Description       { get; set; }          
        public string Flavor            { get; set; }              
        public string[] Colors          { get; set; }               
        public string ManaCost          { get; set; }            
        public int ConvertedManaCost    { get; set; }    
        public string CardSetName       { get; set; }          
        public string Type              { get; set; }                
        public string SubType           { get; set; }              
        public int Power                { get; set; }              
        public int Toughness            { get; set; }            
        public int Loyalty              { get; set; }             
        public string Rarity            { get; set; }               
        public string Artist            { get; set; }              
        public string CardSetId         { get; set; }            
        public Ruling[] Rulings         { get; set; }              
        public string[] Formats         { get; set; }         
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
    }
}

