using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace MtgDb.Info
{
    public class MongoRepository : IRepository
    {
        private string Connection { get; set; }
        private MongoDatabase database;
        private MongoClient client;
        private MongoServer server;
     
        public MongoRepository (string connection)
        {
            Connection = connection;
            client = new MongoClient(connection);
            server = client.GetServer();
            database = server.GetDatabase("mtgdb_info");
        }

        public Planeswalker AddPlaneswalker(Planeswalker planeswalker)
        {
            var collection = database.GetCollection<Planeswalker>("planeswalkers");
            planeswalker.CreatedAt = DateTime.Now;

            WriteConcernResult result = collection.Insert(planeswalker);

            return planeswalker;
        }

        public void RemovePlaneswalker(Guid id)
        {
            var collection = database.GetCollection<Planeswalker> ("planeswalkers");

            Dictionary<string, object> query = new Dictionary<string, object> ();
            query.Add ("_id", id);

            collection.Remove(new QueryDocument(query));
        }

        public Planeswalker GetPlaneswalker(Guid id)
        {
            var collection = database.GetCollection<Planeswalker> ("planeswalkers");
            var query = Query<Planeswalker>.EQ (e => e.Id, id);
            Planeswalker p = collection.FindOne (query);

            if (p != null)
                return p;
      
            return null;
        }
    }
}

