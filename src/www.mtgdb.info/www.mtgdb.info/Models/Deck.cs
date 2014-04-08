using System;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class Deck
    {
        public Guid UserId          { get; set; }
        public string Name          { get; set; }
        public string Description   { get; set; }
        public bool IsPublic        { get; set; }
        public List<DeckCard> Cards { get; set; }
        public DateTime CreatedAt   { get; set; }
        public DateTime ModifiedAt  { get; set; }

        public Deck ()
        {
            Cards = new List<DeckCard>();
        }
    }

    public class DeckCard
    {
        public int MultiverseId { get; set; }
        public int Amount       { get; set; }
    }
}

