using System;

namespace MtgDb.Info
{
    public interface IDeckRepository
    {
        Deck AddDeck(Deck deck);
        Deck UpdateDeck(Deck deck);
        void DeleteDeck(Guid Id);
        Deck GetDeck(Guid userId, string name);
        Deck GetDeck(Guid deckId);
        Deck[] GetUserDecks(Guid userId);
    }
}

