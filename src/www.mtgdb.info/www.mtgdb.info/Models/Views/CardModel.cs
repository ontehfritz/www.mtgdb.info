using System;

namespace MtgDb.Info
{
    public class CardModel : PageModel
    {
        public Card Card        { get; set; }
        public CardSet Set      { get; set; }
        public int Amount       { get; set; } 
        public int Page         { get; set; }
        public Card[] Prints    { get; set; }

        public Card NextCard    { get; set; }
        public Card PrevCard    { get; set; }

        public CardModel (){}
    }
}