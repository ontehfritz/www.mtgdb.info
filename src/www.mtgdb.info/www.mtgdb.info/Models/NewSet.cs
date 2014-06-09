using System;
using MongoDB.Bson.Serialization.Attributes;
using FluentValidation;

namespace MtgDb.Info
{
    public class NewSetValidator : AbstractValidator<NewSet>
    {
        public NewSetValidator()
        {
            RuleFor(set => set.SetId).NotEmpty();
            RuleFor(set => set.ReleasedAt).NotEmpty();

            RuleFor(set => set.ReleasedAt)
                .Matches("^(19|20)\\d\\d[- /.](0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])$")
                .WithMessage("Set release date must be in yyyy-mm-dd fomat");

            RuleFor(set => set.BasicLand)
                .GreaterThanOrEqualTo(0);
            RuleFor(set => set.Rare)
                .GreaterThanOrEqualTo(0);
            RuleFor(set => set.Uncommon)
                .GreaterThanOrEqualTo(0);
            RuleFor(set => set.Common)
                .GreaterThanOrEqualTo(0);
            RuleFor(set => set.MythicRare)
                .GreaterThanOrEqualTo(0);

            RuleFor(set => set.Name).NotEmpty();
            RuleFor(set => set.Description).NotEmpty();
            RuleFor(set => set.Type).NotEmpty();
            RuleFor(set => set.Comment).NotEmpty();
        }
    }

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

        [BsonIgnore]
        public Profile User                 { get; set; }
    }
}

