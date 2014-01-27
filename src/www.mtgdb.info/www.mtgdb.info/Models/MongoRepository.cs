using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using SuperSimple.Auth;
using MongoDB.Bson;
using MtgDb.Info.Driver;

namespace MtgDb.Info
{
    public class MongoRepository : IRepository
    {
        private string Connection { get; set; }
        private MongoDatabase database;
        private MongoClient client;
        private MongoServer server;
        private SuperSimpleAuth ssa = new SuperSimpleAuth ("mtgdb.info", 
            "4e5844a9-6444-415d-b06d-6f29f52fbd0e"); 

        public Db magicdb = new Db ();

        public MongoRepository (string connection)
        {
            Connection = connection;
            client = new MongoClient(connection);
            server = client.GetServer();
            database = server.GetDatabase("mtgdb_info");
            CreateUserCardIndexes ();
        }


        public Dictionary<string, int> GetSetCardCounts(Guid walkerId)
        {
            Dictionary<string, int> userSets = new Dictionary<string, int> ();

            CardSet[] sets = magicdb.GetSets();
            UserCard[] users = null;

            foreach(CardSet s in sets)
            {
                users = GetUserCards (walkerId, s.CardIds);

                if(users != null && users.Length > 0)
                {
                    userSets.Add (s.Id, users.Length);
                }
            }

            return userSets;
        }

        public UserCard[] GetUserCards(Guid walkerId)
        {
            var collection = database.GetCollection<UserCard>("user_cards");
            List<UserCard> userCards = new List<UserCard> ();

            var query =
                Query<UserCard>.EQ(c => c.PlaneswalkerId, walkerId);

            var cards = collection.Find (query);

            foreach(UserCard c in cards)
            {
                userCards.Add (c);
            }

            return userCards.ToArray ();
        }

        public UserCard[] GetUserCards(Guid walkerId, int[] multiverseIds)
        {
            var collection = database.GetCollection<UserCard>("user_cards");
            List<UserCard> userCards = new List<UserCard> ();

            var query = Query.And(Query.In ("MultiverseId", new BsonArray(multiverseIds)),
                Query<UserCard>.EQ(c => c.PlaneswalkerId, walkerId));

            var cards = collection.Find (query);

            foreach(UserCard c in cards)
            {
                userCards.Add (c);
            }

            return userCards.ToArray ();
        }

        public UserCard AddUserCard(Guid walkerId, int multiverseId, int amount)
        {
            MongoCollection<UserCard> cards = database.GetCollection<UserCard> ("user_cards");
            var query = Query.And(Query<UserCard>.EQ (e => e.MultiverseId, multiverseId),
                Query<UserCard>.EQ (e => e.PlaneswalkerId, walkerId));

            UserCard card = cards.FindOne(query);

            if(card == null)
            {
                UserCard newCard = new UserCard ();
                newCard.Id = Guid.NewGuid ();
                newCard.Amount = amount;
                newCard.MultiverseId = multiverseId;
                newCard.PlaneswalkerId = walkerId;
                newCard.SetId = magicdb.GetCard (multiverseId).CardSetId;

                cards.Insert (newCard);
                card = newCard;
            }
            else
            {
                card.Amount = amount;
                cards.Save (card);
            }

            return card;
        }


        public Guid AddPlaneswalker(string userName, string password, string email)
        {
            var collection = database.GetCollection<Profile>("profiles");
          
            User user = ssa.CreateUser (userName, password, email);

            Profile profile = new Profile ();
            profile.Id = user.Id;
            profile.CreatedAt = DateTime.Now;
            profile.Email = user.Email;
            profile.Name = user.UserName;

            WriteConcernResult result = collection.Insert(profile);

            return user.Id;
        }

        public void RemovePlaneswalker(Guid id)
        {
            var collection = database.GetCollection<Profile> ("profiles");

            Dictionary<string, object> query = new Dictionary<string, object> ();
            query.Add ("_id", id);
           
            collection.Remove(new QueryDocument(query));
        }

        public Profile GetProfile(Guid id)
        {
            var collection = database.GetCollection<Profile> ("profiles");
            var query = Query<Profile>.EQ (e => e.Id, id);
            Profile p = collection.FindOne (query);

            if (p != null)
                return p;
      
            return null;
        }

        public Profile GetProfile(string name)
        {
            var collection = database.GetCollection<Profile> ("profiles");
            var query = Query<Profile>.EQ (e => e.Name, name);
            Profile p = collection.FindOne (query);

            if (p != null)
                return p;

            return null;
        }

        public Planeswalker UpdatePlaneswalker(Planeswalker planeswalker)
        {
            MongoCollection<Profile> walkers = database.GetCollection<Profile> ("profiles");
            var query = Query<Profile>.EQ (e => e.Id, planeswalker.Id);

            Profile updateWalker = walkers.FindOne(query);
            Planeswalker user = null;

            if (updateWalker != null) 
            {
                if(updateWalker.Email.ToLower() != planeswalker.Profile.Email.ToLower())
                {
                    ssa.ChangeEmail (planeswalker.AuthToken, planeswalker.Profile.Email);
                }

                if(updateWalker.Name.ToLower() != planeswalker.Profile.Name.ToLower())
                {
                    ssa.ChangeUserName (planeswalker.AuthToken, planeswalker.Profile.Name);
                }

                updateWalker.Email = planeswalker.Profile.Email;
                updateWalker.ModifiedAt = DateTime.Now;
                updateWalker.Name = planeswalker.Profile.Name;

                walkers.Save(updateWalker);

                User ssaUser = ssa.Validate (planeswalker.AuthToken);

                user = new Planeswalker 
                {
                    UserName = ssaUser.UserName,
                    AuthToken = ssaUser.AuthToken,
                    Email = ssaUser.Email,
                    Id = ssaUser.Id,
                    Claims = ssaUser.Claims,
                    Roles = ssaUser.Roles,
                    Profile = GetProfile(ssaUser.Id)
                };
            }
           
            return user;
        }

        private void CreateUserCardIndexes()
        {
            var keys = new IndexKeysBuilder();

            keys.Ascending("PlaneswalkerId","MultiverseId");

            var options = new IndexOptionsBuilder();
            options.SetSparse(true);
            options.SetUnique(true);

            var collection = database.GetCollection<UserCard>("user_cards");

            collection.EnsureIndex(keys, options);
        }
    }
}

