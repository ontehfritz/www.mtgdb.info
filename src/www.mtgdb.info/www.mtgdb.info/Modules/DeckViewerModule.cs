using System;
using MtgDb.Info.Driver;
using System.Configuration;
using Nancy;
using Nancy.Security;
using System.Linq;
using Nancy.ModelBinding;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MtgDb.Info
{
    public class DeckViewerModule : NancyModule
    {
        public IDeckRepository deckbuilder = 
            new MongoDeckRepository (ConfigurationManager.AppSettings.Get("db"));


        public DeckViewerModule () 
        {
            Get["/decks/viewer/{deckId?}"] = parameters => {
                DeckModel model =      new DeckModel();
                model.ActiveMenu =      "mydecks";
                model.Planeswalker =    (Planeswalker)this.Context.CurrentUser;
                model.Title =           "M:tgDb - Simple Deck Viewer";

                if(parameters.deckId != null)
                {
                    Guid id; 

                    if(Guid.TryParse((string)parameters.deckId, out id))
                    {
                        model.Deck = deckbuilder.GetDeck(id);

                        if(model.Deck != null)
                        {
                            model.Name = model.Deck.Name;
                            model.DeckFile = MtgFile.ExportDec(model.Deck);
                        }
                    }
                }

                return View["Deck/Deck", model];
            };

            Post["/decks/viewer/"] = parameters => {
                DeckModel model = this.Bind<DeckModel>();

                model.ActiveMenu =      "mydecks";
                model.Planeswalker =    (Planeswalker)this.Context.CurrentUser;
                model.Title =           "M:tgDb - Simple Deck Viewer";
               
                byte[] byteArray = Encoding.ASCII.GetBytes(model.DeckFile);
                MemoryStream stream = new MemoryStream(byteArray);

                model.Deck = MtgFile.ImportDec(stream);

                if(Request.Form.Save != null)
                {
                    model.Deck.UserId = model.Planeswalker.Id;
                    model.Deck.Name = model.Name;
                    deckbuilder.AddDeck(model.Deck);
                    return Response.AsRedirect(string.Format("/decks/viewer/{0}", 
                        model.Deck.Id.ToString()));
                }

                return View["Deck/Deck", model];
            };
        }
    }
}

