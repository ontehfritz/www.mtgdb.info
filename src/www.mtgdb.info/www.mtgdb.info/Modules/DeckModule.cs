using System;
using MtgDb.Info.Driver;
using System.Configuration;
using Nancy;
using Nancy.Security;
using System.Linq;
using Nancy.ModelBinding;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class DeckModule :  NancyModule
    {
        public IRepository repository = 
            new MongoRepository (ConfigurationManager.AppSettings.Get("db"));

        public IDeckRepository deckbuilder = 
            new MongoDeckRepository (ConfigurationManager.AppSettings.Get("db"));

        public Db magicdb = 
            new Db (ConfigurationManager.AppSettings.Get("api"));

        public DeckModule ()
        {
            this.RequiresAuthentication ();
           
            //viewing public decks of a planeswalker
            Get["/pw/{planeswalker}/decks"] = parameters => {
                DecksModel model =      new DecksModel();
                model.ActiveMenu =      "mydecks";
                model.Planeswalker =    (Planeswalker)this.Context.CurrentUser;
                model.Decks =           deckbuilder.GetUserDecks(model.Planeswalker.Id).ToList();
                model.Title =           "My Decks";
             
                return "decks";
            };

            //for deck link sharing and viewing single deck
            Get["/pw/{planeswalker}/decks/{name}"] = parameters => {
                DeckModel model =       new DeckModel();
                model.ActiveMenu =      "mydecks";
                model.Planeswalker =    (Planeswalker)this.Context.CurrentUser;
                model.Deck =            
                    deckbuilder.GetDeck(model.Planeswalker.Id, (string)parameters.name);

                return View["Deck/Deck", model];
            };

            //create a deck SPA
            Post["/decks"] = parameters => {
                Planeswalker planeswalker =     (Planeswalker)this.Context.CurrentUser;


                int [] cards = null;
                try
                {
                    cards = ((string)Request.Form.Cards).Split(',')
                    .Select(n => Convert.ToInt32(n))
                    .ToArray();

                }
                catch(Exception e)
                {
                    throw e;
                }

                string name = (string)this.Request.Form.Name; 
                string description = (string)this.Request.Form.Description; 

                Deck deck = new Deck();
                deck.Name = name; 
                deck.Description = description;
                deck.SetCards(cards);
                deck.UserId = planeswalker.Id;

                return Response.AsJson(deck);
            };

            //update a deck SPA
            Put["/decks/"] = parameters => {
                Deck deck =                     this.Bind<Deck>();
                Planeswalker planeswalker =     (Planeswalker)this.Context.CurrentUser;
                deck.UserId =                   planeswalker.Id;
                deck =                          deckbuilder.UpdateDeck(deck);

                return Response.AsJson(deck);
            };

            //Delete a deck SPA
            Delete["/decks/{name}"] = parameters => {
                Planeswalker planeswalker =     (Planeswalker)this.Context.CurrentUser;
                Deck deck = deckbuilder.GetDeck(planeswalker.Id, (string)parameters.name);

                try
                {
                    deckbuilder.DeleteDeck(deck.Id);
                }
                catch(Exception e)
                {
                    return Response.AsJson(e.Message, 
                        HttpStatusCode.NotAcceptable);
                }

                return Response.AsJson("true", HttpStatusCode.Accepted);
            };

            //Get a deck SPA
            Get["/decks/{name}"] = parameters => {
                Planeswalker planeswalker =     (Planeswalker)this.Context.CurrentUser;
                Deck deck =   deckbuilder.GetDeck(planeswalker.Id, (string)parameters.name);

                return Response.AsJson(deck);
            };


        }
    }
}

