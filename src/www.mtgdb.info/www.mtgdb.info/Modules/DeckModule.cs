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
            Get["/mydecks"] = parameters => {
                DecksModel model =      new DecksModel();
                model.ActiveMenu =      "mydecks";
                model.Planeswalker =    (Planeswalker)this.Context.CurrentUser;
                model.Decks =           deckbuilder.GetUserDecks(model.Planeswalker.Id).ToList();
                model.Title =           "My Decks";
             
                return View["Deck/MyDecks", model];
            };

            //for deck link sharing and viewing single deck
            Get["/pw/{planeswalker}/decks/{name}"] = parameters => {
                DeckModel model =       new DeckModel();
                model.ActiveMenu =      "mydecks";
                model.Planeswalker =    (Planeswalker)this.Context.CurrentUser;
                model.Deck =            
                    deckbuilder.GetDeck(model.Planeswalker.Id, 
                        (string)parameters.name);

                return View["Deck/Deck", model];
            };

            Get["/decks"] = parameters => {
                Planeswalker planeswalker = (Planeswalker)this.Context.CurrentUser;
                Deck [] decks = deckbuilder.GetUserDecks(planeswalker.Id);

                return View["Deck/Decks", decks.ToArray()];
            };

            //create a deck SPA
            Post["/decks"] = parameters => {
                Planeswalker planeswalker =  
                    (Planeswalker)this.Context.CurrentUser;

                int [] cards = null;
                int [] sideBar = null;

                try
                {
                    cards = ((string)Request.Form.Cards).Split(',')
                        .Select(n => Convert.ToInt32(n))
                        .ToArray();

                    sideBar = ((string)Request.Form.SideBar).Split(',')
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
                deck.SetSideBar(sideBar);
                deck.UserId = planeswalker.Id;

                deck = deckbuilder.AddDeck(deck);

                return Response.AsJson(deck);
            };

            //update a deck SPA
            Post["/decks/{name}"] = parameters => {
                Planeswalker planeswalker =     (Planeswalker)this.Context.CurrentUser;
                string deckName =               (string)parameters.name;

                int [] cards = null;
                int [] sideBar = null;

                try
                {
                    cards = ((string)Request.Form.Cards).Split(',')
                        .Select(n => Convert.ToInt32(n))
                        .ToArray();

                    sideBar = ((string)Request.Form.SideBar).Split(',')
                        .Select(n => Convert.ToInt32(n))
                        .ToArray();

                }
                catch(Exception e)
                {
                    throw e;
                }

                string name = (string)this.Request.Form.Name; 
                string description = (string)this.Request.Form.Description; 

                Deck deck = deckbuilder.GetDeck(planeswalker.Id, deckName);
                deck.Name = name; 
                deck.Description = description;
                deck.SetCards(cards);
                deck.SetSideBar(sideBar);
                deck.UserId = planeswalker.Id;

                deck = deckbuilder.UpdateDeck(deck);

                return Response.AsJson(deck);
            };

            //Delete a deck SPA
            Post["/decks/delete/{name}"] = parameters => {
                Planeswalker planeswalker =     (Planeswalker)this.Context.CurrentUser;
                Deck deck = deckbuilder.GetDeck(planeswalker.Id, 
                    (string)parameters.name);

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
                Deck deck =   deckbuilder.GetDeck(planeswalker.Id, 
                    (string)parameters.name);

                return Response.AsJson(deck);
            };

            //Get cards for a deck SPA
            Get["/decks/{name}/cards"] = parameters => {
                Planeswalker planeswalker =     (Planeswalker)this.Context.CurrentUser;
                Deck deck =   deckbuilder.GetDeck(planeswalker.Id, 
                    (string)parameters.name);

                return Response.AsJson(deck.GetCards());
            };

            //Get cards for a deck SPA
            Get["/decks/{name}/sidebar"] = parameters => {
                Planeswalker planeswalker =     (Planeswalker)this.Context.CurrentUser;
                Deck deck =  deckbuilder.GetDeck(planeswalker.Id, 
                    (string)parameters.name);

                return Response.AsJson(deck.GetSideBarCards());
            };

            //gets the latest version of card at the time of import
            Post["/decks/import"] = parameters => {
                Planeswalker planeswalker =     (Planeswalker)this.Context.CurrentUser;
                string deckName =               (string)this.Request.Form.Name;
                string description =            (string)this.Request.Form.Description;
                var file =                      this.Request.Files.FirstOrDefault();
                Deck deck =                     null;

                if(file != null)
                {
                    deck = MtgFile.ImportDec(file.Value);
                    deck.Name = deckName;
                    deck.Description = description;
                    deck.UserId = planeswalker.Id;
                }
                    
                return Response.AsJson(deck);
            };

            //testing method
            Post["/col/import"] = parameters => {
                Planeswalker planeswalker =     (Planeswalker)this.Context.CurrentUser;
                var file =                      this.Request.Files.FirstOrDefault();
               
                MtgDb.Info.MtgFile.Item[] items = null;

                if(file != null)
                {
                    items = MtgFile.ImportColl2(file.Value);
                }

                return Response.AsJson(items);
            };
        }
    }
}

