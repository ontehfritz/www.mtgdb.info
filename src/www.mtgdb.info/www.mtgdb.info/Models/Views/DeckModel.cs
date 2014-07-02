using System;
using FluentValidation;

namespace MtgDb.Info
{
    public class DeckModelValidator : AbstractValidator<DeckModel>
    {
        public DeckModelValidator()
        {
            RuleFor(deck => deck.DeckFile).NotEmpty()
                .WithMessage("No .dec file to render or save.");
        }
    }

    public class DeckModel : PageModel
    {
        public Deck Deck            { get; set; }
        public string DeckFile      { get; set; }
        public string Description   { get; set; }
        public string Name          { get; set; }
        public string Email         { get; set; }
      
        public DeckModel () : base(){}
    }
}

