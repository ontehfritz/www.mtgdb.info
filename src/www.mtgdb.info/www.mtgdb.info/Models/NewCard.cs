using System;
using MongoDB.Bson.Serialization.Attributes;
using FluentValidation;

namespace MtgDb.Info
{

    public class NewCardValidator : AbstractValidator<NewCard>
    {
        public NewCardValidator()
        {
            RuleFor(card => card.Artist).NotEmpty();
            RuleFor(card => card.CardSetId).NotEmpty();
            RuleFor(card => card.Colors).NotEmpty();
//            RuleFor(change => change.ReleasedAt)
//                .Matches("^(19|20)\\d\\d[- /.](0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])$")
//                .WithMessage("Card release date must be in yyyy-mm-dd fomat");

            RuleFor(card => card.ConvertedManaCost)
                .GreaterThanOrEqualTo(0);
            RuleFor(card => card.Power)
                .GreaterThanOrEqualTo(0);

            RuleFor(card => card.Toughness)
                .GreaterThanOrEqualTo(0);

            RuleFor(card => card.Name).NotEmpty();
            RuleFor(card => card.Rarity).NotEmpty();
            RuleFor(card => card.Type).NotEmpty();
            RuleFor(card => card.Comment).NotEmpty();
            RuleFor(card => card.RelatedCardId)
                .GreaterThanOrEqualTo(0);
        }
    }


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


        [BsonIgnore]
        public Profile User { get; set; }
    }
}

