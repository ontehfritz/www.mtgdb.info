using System;
using Nancy;
using Nancy.Security;
using MtgDb.Info.Driver;
using System.Configuration;
using System.Linq;
using Nancy.ModelBinding;
using Nancy.Validation;

namespace MtgDb.Info
{
    public class OpenDbModule:  NancyModule
    {

        public IRepository repository = 
            new MongoRepository (ConfigurationManager.AppSettings.Get("db"));

        public Db magicdb = 
            new Db (ConfigurationManager.AppSettings.Get("api"));

        public OpenDbModule ()
        {
            this.RequiresAuthentication ();

            Get["/cr/{status?}"] = parameters => {
                ChangeRequestModel model =  new ChangeRequestModel();
                string status =             (string)parameters.status;
                model.Planeswalker =        (Planeswalker)this.Context.CurrentUser;
                model.Title =               "M:tgDb.Info Admin";

                if(status == null)
                {
                    model.Changes =     repository.GetChangeRequests().ToList();
                    model.NewCards =    repository.GetNewCards().ToList();
                    model.NewSets =     repository.GetNewSets().ToList();
                }
                else
                {
                    model.Changes =     repository.GetChangeRequests(status).ToList();
                    model.NewCards =    repository.GetNewCards(status).ToList();
                    model.NewSets =     repository.GetNewSets(status).ToList();
                }

                return View["Change/ChangeRequests", model];
            };

            Get["/sets/new", true] = async (parameters, ct) => {
                NewSet model = new NewSet();
                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;

                return View["Change/NewSet", model];
            };

            Post["/sets/new", true] = async (parameters, ct) => {
                NewSet model =          this.Bind<NewSet>();
                model.Planeswalker =    (Planeswalker)this.Context.CurrentUser;
                model.UserId =          model.Planeswalker.Id;

                var result = this.Validate(model);

                if (!result.IsValid)
                {
                    model.Errors = ErrorUtility.GetValidationErrors(result);
                    return View["Change/NewSet", model];
                }

                repository.AddSet(model);

                return Response.AsRedirect(string.Format("/sets/new/{0}",
                    model.Id));
            };

            Get["/sets/new/{id}", true] = async (parameters, ct) => {
                NewSet model = repository.GetSet((Guid)parameters.id);

                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;

                return View["Change/NewSetRead", model];
            };

            Post["/sets/new/{id}", true] = async (parameters, ct) => {
                NewSet model = repository.GetSet((Guid)parameters.id);
                model.Planeswalker =    (Planeswalker)this.Context.CurrentUser;
                Admin admin = new Admin(ConfigurationManager.AppSettings.Get("api"));

                if(!model.Planeswalker.InRole("admin"))
                {
                    return HttpStatusCode.Unauthorized;
                }
                    
                MtgDbAdminDriver.CardSet newSet = 
                    new MtgDbAdminDriver.CardSet()
                {
                    Id = model.SetId,
                    Name = model.Name,
                    Description = model.Description,
                    ReleasedAt = model.ReleasedAt,
                    Type = model.Type,
                    Block = model.Block,
                    Common = model.Common,
                    Uncommon = model.Uncommon,
                    Rare = model.Rare,
                    MythicRare = model.MythicRare,
                    BasicLand = model.BasicLand
                };

                admin.AddSet(model.Planeswalker.AuthToken, newSet);
                model = repository.UpdateNewSetStatus(model.Id, "Accepted");

                model.Messages.Add("Set has been sucessfully added.");

                return View["Change/NewSetRead", model];
            };
                
            Get["/cards/new/{id}", true] = async (parameters, ct) => {
                NewCard model = repository.GetCard((Guid)parameters.id);
                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;

                return View["Change/NewCardsRead", model];
            };

            Post["/cards/new/{id}", true] = async (parameters, ct) => {
                NewCard model = repository.GetCard((Guid)parameters.id);
                Admin admin = new Admin(ConfigurationManager.AppSettings.Get("api"));
                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;

                if(!model.Planeswalker.InRole("admin"))
                {
                    return HttpStatusCode.Unauthorized;
                }


                string [] colors = null;

                if(model.Colors == null || 
                    model.Colors.Length == 0)
                {
                    colors = new string[1];
                    colors[0] = "None";
                }
                else
                {
                    colors = model.Colors.Split(',');
                }
                    
                MtgDbAdminDriver.Card newCard = 
                    new MtgDbAdminDriver.Card()
                {
                    Id = model.Mvid,
                    Name = model.Name,
                    Description = model.Description,
                    Flavor = model.Flavor,
                    Power = model.Power,
                    Toughness = model.Toughness,
                    Loyalty = model.Loyalty,
                    Artist = model.Artist,
                    Type = model.Type,
                    SubType = model.SubType,
                    Token = model.Token,
                    Colors = colors,
                    CardSetId = model.CardSetId,
                    ManaCost = model.ManaCost,
                    ConvertedManaCost = model.ConvertedManaCost,
                    RelatedCardId = model.RelatedCardId,
                    Rarity = model.Rarity,
                    SetNumber = model.SetNumber
                };

                try
                {
                    admin.AddCard(model.Planeswalker.AuthToken,newCard);
                    repository.UpdateNewCardStatus(model.Id, "Accepted");
                    model = repository.GetCard((Guid)parameters.id);
                    model.Messages.Add("Card has been sucessfully added.");
                }
                catch(Exception e)
                {
                    model.Errors.Add(e.Message);
                }
                    
                return View["Change/NewCardsRead", model];
            };
                
            Get["/cards/new", true] = async (parameters, ct) => {
                NewCard model = new NewCard();

                if(Request.Query.SetId != null)
                {
                    model.CardSetId = (string)Request.Query.SetId;
                }

                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;
                model.Types = magicdb.GetCardTypes();
                model.SubTypes = magicdb.GetCardSubTypes();
                model.Rarities = magicdb.GetCardRarityTypes();

                return View["Change/NewCard", model];
            };

            Post["/cards/new", true] = async (parameters, ct) => {
                NewCard model = this.Bind<NewCard>();
                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;
                model.UserId = model.Planeswalker.Id;

                model.Types = magicdb.GetCardTypes();
                model.SubTypes = magicdb.GetCardSubTypes();
                model.Rarities = magicdb.GetCardRarityTypes();

                var result = this.Validate(model);

                if (!result.IsValid)
                {
                    model.Errors = ErrorUtility.GetValidationErrors(result);
                    return View["Change/NewCard", model];
                }
                    
                Guid id = repository.AddCard(model);
                model = repository.GetCard(id);
               
                return Response.AsRedirect(string.Format("/cards/new/{0}",
                    model.Id));
            };


            Get["/cards/{id}/logs"] = parameters => {
                CardLogsModel model =   new CardLogsModel();
                model.ActiveMenu =      "sets";
                model.Planeswalker =    (Planeswalker)this.Context.CurrentUser;
                model.Mvid =            (int)parameters.id;

                if(Request.Query.v != null)
                {
                    int version = 0; 
                    if(int.TryParse((string)Request.Query.v, out version))
                    {
                        model.NewVersion = version;
                    }
                }

                try
                {
                    model.Changes = repository.GetCardChangeRequests((int)parameters.id)
                        .OrderByDescending(x => x.Version) 
                        .ToList();
                }
                catch(Exception e)
                {
                    model.Errors.Add(e.Message);
                }

                return View["Change/CardLogs", model];
            };

            Get["/cards/{id}/logs/{logid}"] = parameters => {
                CardChange model = new CardChange();
                model.Planeswalker =    (Planeswalker)this.Context.CurrentUser;

                try
                {
                    model = repository.GetCardChangeRequest((Guid)parameters.logid);
                }
                catch(Exception e)
                {
                    model.Errors.Add(e.Message);
                }

                model.ActiveMenu =      "sets";
                model.Planeswalker =    (Planeswalker)this.Context.CurrentUser;
                model.Mvid =            (int)parameters.id;


                return View["Change/CardChange", model];
            };

            Post["/change/{id}/field/{field}"] = parameters => {
                Admin admin =               new Admin(ConfigurationManager.AppSettings.Get("api"));
                Planeswalker planeswalker = (Planeswalker)this.Context.CurrentUser;
                Guid changeId  =            Guid.Parse((string)parameters.id);
                string field =              (string)parameters.field;
                CardChange change =         null;

                if(!planeswalker.InRole("admin"))
                {
                    return HttpStatusCode.Unauthorized;
                }

                try
                {
                    change = repository.GetCardChangeRequest(changeId);

                    if(field == "close")
                    {
                        repository.UpdateCardChangeStatus(change.Id, "Closed");

                        return Response.AsRedirect(string.Format("/cards/{0}/logs/{1}",
                            change.Mvid, change.Id));
                    }

                    if(field == "open")
                    {
                        repository.UpdateCardChangeStatus(change.Id, "Pending");

                        return Response.AsRedirect(string.Format("/cards/{0}/logs/{1}",
                            change.Mvid, change.Id));
                    }


                    if(field == "formats")
                    {
                        admin.UpdateCardFormats(planeswalker.AuthToken,
                            change.Mvid, change.Formats);
                    }
                    else if(field == "rulings")
                    {
                        admin.UpdateCardRulings(planeswalker.AuthToken,
                            change.Mvid, change.Rulings);
                    }
                    else
                    {
                        string value = change.GetFieldValue(field);
                        admin.UpdateCardField(planeswalker.AuthToken,
                            change.Mvid, field, (string)value);
                    }

                    repository.UpdateCardChangeStatus(change.Id, "Accepted", field);

                }
                catch(Exception e)
                {
                    throw e;
                }

                return Response.AsRedirect(string.Format("/cards/{0}/logs/{1}",
                    change.Mvid, change.Id));
            };

            Get ["/cards/{id}/change"] = parameters => {
                CardChange model = new CardChange();
                Card card; 

                try
                {
                    card = magicdb.GetCard((int)parameters.id);
                    model = CardChange.MapCard(card);
                    model.Planeswalker = (Planeswalker)this.Context.CurrentUser;
                }
                catch(Exception e)
                {
                    throw e;
                }

                return View["Change/Card", model];
            };

            Post ["/cards/{id}/change"] = parameters => {
                CardChange model =      this.Bind<CardChange>();
                Card current =          magicdb.GetCard((int)parameters.id);
                model.CardSetId =       current.CardSetId;
                model.CardSetName =     current.CardSetName;
                model.Name =            current.Name;
                model.Mvid =            (int)parameters.id;
                model.Rulings =         Bind.Rulings(Request);
                model.Formats =         Bind.Formats(Request);
                model.Planeswalker =    (Planeswalker)this.Context.CurrentUser;
                model.UserId =          model.Planeswalker.Id;
                CardChange card =       null;

                var result = this.Validate(model);

                if (!result.IsValid)
                {
                    model.Errors = ErrorUtility.GetValidationErrors(result);
                    return View["Change/Card", model];
                }

                try
                {
                    card = repository.GetCardChangeRequest(
                        repository.AddCardChangeRequest(model)
                    );
                }
                catch(Exception e)
                {
                    model.Errors.Add(e.Message);
                    return View["Change/Card", model];
                }

                return Response.AsRedirect(string.Format("/cards/{0}/logs?v={1}", 
                    card.Mvid, card.Version));

                //return model.Description;
                //return View["Change/Card", model];

            };
        }
    }
}

