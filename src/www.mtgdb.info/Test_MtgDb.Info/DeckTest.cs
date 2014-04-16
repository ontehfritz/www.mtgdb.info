using System;
using NUnit.Framework;
using MtgDb.Info;
using System.Collections.Generic;

namespace Test_MtgDb.Info
{
    [TestFixture ()]
    public class DeckTest
    {
        IDeckRepository deckRepo; 
        Guid DeckId;
        Guid UserId = Guid.NewGuid();

        [SetUp()]
        public void Init()
        {
            deckRepo = new MongoDeckRepository("mongodb://localhost");

            Deck deck = new Deck();
            List<DeckCard> cards = new List<DeckCard>();
            cards.Add(new DeckCard()
                {
                    MultiverseId = 1,
                    Amount = 2
                });

            List<DeckCard> sideBoard = new List<DeckCard>();
            sideBoard.Add(new DeckCard()
                {
                    MultiverseId = 34,
                    Amount = 3
                });

            deck.Cards = cards;
            deck.SideBar = sideBoard;
            deck.Description = "This is a test deck.";
            deck.IsPublic = true;
            deck.Name = "Test Deck";
            deck.CreatedAt = DateTime.Now;
            deck.UserId = UserId;

            deck = deckRepo.AddDeck(deck);
            DeckId = deck.Id;
        }

        [Test()]
        public void Add_deck()
        {
            Deck deck = new Deck();
            List<DeckCard> cards = new List<DeckCard>();
            cards.Add(new DeckCard()
                {
                    MultiverseId = 1,
                    Amount = 2
                });

            List<DeckCard> sideBoard = new List<DeckCard>();
            sideBoard.Add(new DeckCard()
                {
                    MultiverseId = 34,
                    Amount = 3
                });

            deck.Cards = cards;
            deck.SideBar = sideBoard;
            deck.Description = "This is a test deck 2";
            deck.IsPublic = true;
            deck.Name = "Test Deck 2";
            deck.CreatedAt = DateTime.Now;
            deck.UserId = this.UserId;

            deck = deckRepo.AddDeck(deck);

            Assert.NotNull(deck);
        }

        [Test()]
        public void Get_deck()
        {
            Deck deck = deckRepo.GetDeck(DeckId);
            Assert.IsNotNull(deck);
        }

        [Test()]
        public void Update_deck()
        {
            Deck deck = deckRepo.GetDeck(DeckId);
            deck.Description = "Changed the description";
            deckRepo.UpdateDeck(deck);

            deck = deckRepo.GetDeck(DeckId);

            Assert.AreEqual("Changed the description",deck.Description);
        }

        [Test()]
        public void Delete_deck()
        {
            deckRepo.DeleteDeck(DeckId);
            Deck deck = deckRepo.GetDeck(DeckId);
            Assert.IsNull(deck);
        }
    }
}

