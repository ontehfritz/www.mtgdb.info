using System;
using MongoDB.Driver;

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
            database = server.GetDatabase("mtgdb.info");
        }

        public void AddPlaneswalker(Planeswalker planeswalker)
        {
            var collection = database.GetCollection<Planeswalker>("planeswalkers");


            //return role;

        }


    }
}

