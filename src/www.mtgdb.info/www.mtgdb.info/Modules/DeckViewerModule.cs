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
using Nancy.Validation;

namespace MtgDb.Info
{
    public class DeckViewerModule : NancyModule
    {
        public IDeckRepository deckbuilder = 
            new MongoDeckRepository (ConfigurationManager.AppSettings.Get("db"));
            
        public DeckViewerModule () 
        {
            Get["/decks/viewer/{deckId?}"] = parameters => {

                DeckModel model =       new DeckModel();
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
                            model.Description = model.Description;
                            model.DeckFile = MtgFile.ExportDec(model.Deck);
                        }
                    }
                }

                return View["Deck/Deck", model];

            };

            Post["/decks/viewer/{deckId?}"] = parameters => {
                DeckModel model =       this.Bind<DeckModel>();
                model.ActiveMenu =      "mydecks";
                model.Planeswalker =    (Planeswalker)this.Context.CurrentUser;
                model.Title =           "M:tgDb - Simple Deck Viewer";

                var result = this.Validate(model);

                if (!result.IsValid)
                {
                    model.Errors = ErrorUtility.GetValidationErrors(result);
                    return View["Deck/Deck", model];
                }

               
                byte[] byteArray = Encoding.ASCII.GetBytes(model.DeckFile);
                MemoryStream stream = new MemoryStream(byteArray);

                model.Deck = MtgFile.ImportDec(stream);

                if(parameters.deckId != null)
                {
                    Guid id; 

                    if(Guid.TryParse((string)parameters.deckId, out id))
                    {
                        Deck deck = null;
                        try
                        {
                            deck = deckbuilder.GetDeck(id);
                        }
                        catch(Exception exc)
                        {
                            model.Errors.Add(exc.Message);
                        }

                        if(deck != null)
                        {
                            model.Deck.UserId = deck.UserId;
                            model.Deck.Id = deck.Id;
                            model.Deck.CreatedAt = deck.CreatedAt;
                        }
                    }
                }

                if(Request.Form.Save != null)
                {
                    if(model.Deck.Id != Guid.Empty && 
                        model.Planeswalker.Id == model.Deck.UserId)
                    {
                        model.Deck.UserId = model.Planeswalker.Id;
                        model.Deck.Name = model.Name;
                        model.Deck.Description = model.Description;
                        try
                        {
                            model.Deck = deckbuilder.UpdateDeck(model.Deck);
                        }
                        catch(Exception exc)
                        {
                            model.Errors.Add(exc.Message);
                        }
                    }
                    else
                    {
                        if(model.Planeswalker != null)
                        {
                            model.Deck.UserId = model.Planeswalker.Id;
                        }

                        model.Deck.Name = model.Name;
                        model.Deck.Description = model.Description;
                        try
                        {
                            model.Deck = deckbuilder.AddDeck(model.Deck);
                        }
                        catch(Exception exc)
                        {
                            model.Errors.Add(exc.Message);
                        }

                        if(model.Deck.UserId == Guid.Empty && !string.IsNullOrEmpty(model.Email))
                        {
                            try
                            {
                                Email.Send(model.Email,"M:tgDb - You created an anonymous deck!",
                                    string.Format("Deck link: https://www.mtgdb.info/decks/viewer/{0}",
                                        model.Deck.Id));
                            }
                            catch(Exception exc)
                            {
                                model.Errors.Add(exc.Message);
                            }
                        }
                    }
                        
                    return Response.AsRedirect(string.Format("/decks/viewer/{0}", 
                        model.Deck.Id.ToString()));
                }

                return View["Deck/Deck", model];

            };
        }
    }
}

