using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using SuperSimple.Auth;

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

     
        public MongoRepository (string connection)
        {
            Connection = connection;
            client = new MongoClient(connection);
            server = client.GetServer();
            database = server.GetDatabase("mtgdb_info");
        }

        public void AddPlaneswalker(string userName, string password, string email)
        {
            var collection = database.GetCollection<Profile>("planeswalkers");
          
            User user = ssa.CreateUser (userName, password, email);

            Profile profile = new Profile ();
            profile.Id = user.Id;
            profile.CreatedAt = DateTime.Now;
            profile.Email = user.Email;
            profile.Name = user.UserName;

            WriteConcernResult result = collection.Insert(profile);
        }

        public void RemovePlanswalker(Guid id)
        {
            var collection = database.GetCollection<Profile> ("planeswalkers");

            Dictionary<string, object> query = new Dictionary<string, object> ();
            query.Add ("_id", id);

            collection.Remove(new QueryDocument(query));
        }

        public Profile GetProfile(Guid id)
        {
            var collection = database.GetCollection<Profile> ("planeswalkers");
            var query = Query<Profile>.EQ (e => e.Id, id);
            Profile p = collection.FindOne (query);

            if (p != null)
                return p;
      
            return null;
        }

        public Planeswalker UpdatePlaneswalker(Planeswalker planeswalker)
        {
            MongoCollection<Profile> walkers = database.GetCollection<Profile> ("planeswalker");
            var query = Query<Profile>.EQ (e => e.Id, planeswalker.Id);

            Profile updateWalker = walkers.FindOne(query);

            if (updateWalker != null) 
            {
                if(updateWalker.Email.ToLower() != planeswalker.Email.ToLower())
                {
                    ssa.ChangeEmail (planeswalker.AuthToken, planeswalker.Email);
                }

                if(updateWalker.Name.ToLower() != planeswalker.UserName.ToLower())
                {
                    ssa.ChangeUserName (planeswalker.AuthToken, planeswalker.UserName);
                }


                updateWalker.Email = planeswalker.Email;
                updateWalker.ModifiedAt = DateTime.Now;
                updateWalker.Name = pla.Name;

                walkers.Save(updateWalker);
            }

            return GetPlaneswalker(planeswalker.Id);

        }
    }
}

