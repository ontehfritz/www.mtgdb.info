using System;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class MongoDeckRepository : IDeckRepository
    {
        public Deck AddDeck(Deck deck)
        {
            return deck;
        }

        public Deck UpdateDeck(Deck deck)
        {
            return deck;
        }

        public void DeleteDeck(Guid Id)
        {

        }

        public Deck GetDeck(Guid id)
        {
            Deck deck = new Deck();

            return deck;

        }

        public Deck[] GetUserDecks(Guid userId)
        {
            List<Deck> decks = new List<Deck>();

            return decks.ToArray();
        }

        public MongoDeckRepository ()
        {
        }
    }
}

