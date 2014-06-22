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
        public DeckViewerModule () 
        {
            Get["/decks/viewer"] = parameters => {
                DeckModel model =      new DeckModel();
                model.ActiveMenu =      "mydecks";
                model.Planeswalker =    (Planeswalker)this.Context.CurrentUser;
                model.Title =           "M:tgDb - Simple Deck Viewer";

                return View["Deck/Deck", model];
            };

            Post["/decks/viewer"] = parameters => {
                DeckModel model = this.Bind<DeckModel>();

                model.ActiveMenu =      "mydecks";
                model.Planeswalker =    (Planeswalker)this.Context.CurrentUser;
                model.Title =           "M:tgDb - Simple Deck Viewer";
               
                byte[] byteArray = Encoding.ASCII.GetBytes(model.DeckFile);
                MemoryStream stream = new MemoryStream(byteArray);

                model.Deck = MtgFile.ImportDec(stream);

                return View["Deck/Deck", model];
            };
        }
    }
}

