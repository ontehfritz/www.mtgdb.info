using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Linq;

namespace MtgDb.Info
{
    public class MongoDeckRepository : IDeckRepository
    {
        private string Connection { get; set; }
        private MongoDatabase database;
        private MongoClient client;
        private MongoServer server;

        public MongoDeckRepository(string connection)
        {
            Connection =    connection;
            client =        new MongoClient(connection);
            server =        client.GetServer();
            database =      server.GetDatabase("mtgdb_info");
        }

        public Deck AddDeck(Deck deck)
        {
            MongoCollection<CardChange> collection = 
                database.GetCollection<CardChange> ("decks");

            deck.Id = Guid.NewGuid();
            deck.CreatedAt = DateTime.Now;

            try
            {
                collection.Save(deck);
            }
            catch(Exception e)
            {
                throw e;
            }

            return deck;
        }

        public Deck UpdateDeck(Deck deck)
        {
            deck.ModifiedAt = DateTime.Now;

            MongoCollection<CardChange> collection = 
                database.GetCollection<CardChange> ("decks");

            var updateResult = collection.Update(
                Query<Deck>.EQ(d => d.Id, deck.Id),
                Update<Deck>
                .Set(d => d.Name,  deck.Name)
                .Set(d => d.Cards, deck.Cards)
                .Set(d => d.Description, deck.Description)
                .Set(d => d.IsPublic, deck.IsPublic)
                .Set(d => d.SideBar,  deck.SideBar)
                .Set(d => d.ModifiedAt, deck.ModifiedAt),
                new MongoUpdateOptions
                {
                    WriteConcern = WriteConcern.Acknowledged
                });

            return deck;
        }

        public void DeleteDeck(Guid id)
        {
            MongoCollection<Deck> collection = 
                database.GetCollection<Deck> ("decks");

            Dictionary<string, object> query = new Dictionary<string, object> ();
            query.Add ("_id", id);

            collection.Remove(new QueryDocument(query));
        }

        public Deck GetDeck(Guid userId, string name)
        {
            MongoCollection<Deck> collection = 
                database.GetCollection<Deck> ("decks");

            var query = Query.And(Query<Deck>.EQ (e => e.UserId, userId),
                Query<Deck>.EQ(e => e.Name, name));

            Deck deck =  collection.FindOne(query);

            return deck;
        }

        public Deck GetDeck(Guid deckId)
        {
            MongoCollection<Deck> collection = 
                database.GetCollection<Deck> ("decks");

            var query = Query.And(Query<Deck>.EQ (e => e.Id, deckId));

            Deck deck =  collection.FindOne(query);

            return deck;
        }

        public Deck[] GetUserDecks(Guid userId)
        {
            List<Deck> decks = new List<Deck>();

            MongoCollection<Deck> collection = 
                database.GetCollection<Deck> ("decks");
                
            var query = Query.And(Query<Deck>.EQ (e => e.UserId, userId ));

            var d = collection.Find(query);
          
            foreach(Deck deck in d)
            {
                decks.Add(deck);
            }

            return decks
                   .OrderBy(x => x.CreatedAt)
                   .ToArray();
        }
    }
}

