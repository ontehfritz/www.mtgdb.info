using System;
using MtgDb.Info.Driver;
using System.Configuration;
using Nancy;
using Nancy.Security;

namespace MtgDb.Info
{
    public class DeckModule :  NancyModule
    {
        public IRepository repository = 
            new MongoRepository (ConfigurationManager.AppSettings.Get("db"));

        public Db magicdb = 
            new Db (ConfigurationManager.AppSettings.Get("api"));

        public DeckModule ()
        {
            this.RequiresAuthentication ();
           
            Get["/pw/{planeswalker}/decks"] = parameters => {

                return "decks";
            };

            Get["/pw/{planeswalker}/decks/new"] = parameters => {

                return "new deck";
            };

            Post["/pw/{planeswalker}/decks/new"] = parameters => {

                return "create deck";
            };

            Get["/pw/{planeswalker}/decks/{name}"] = parameters => {

                return string.Format("{0} deck",(string)parameters.name);
            };
        }
    }
}

