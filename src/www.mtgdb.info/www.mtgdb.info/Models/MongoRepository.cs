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
    }
}

