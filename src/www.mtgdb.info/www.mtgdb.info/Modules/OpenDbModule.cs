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
                ChangeRequestModel model = new ChangeRequestModel();
                string status = (string)parameters.status;
                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;
                model.Title = "M:tgDb.Info Admin";

                if(status == null)
                {
                    model.Changes = repository.GetChangeRequests().ToList();
                }
                else
                {
                    model.Changes = repository.GetChangeRequests(status).ToList();
                }

                return View["Change/ChangeRequests", model];
            };

            Get["/sets/new", true] = async (parameters, ct) => {
                NewSet model = new NewSet();
                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;

                return View["Change/NewSet", model];
            };

            Post["/sets/new", true] = async (parameters, ct) => {
                NewSet model = this.Bind<NewSet>();
                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;
                model.UserId = model.Planeswalker.Id;
                repository.AddSet(model);

                return View["Change/NewSet", model];
            };

            Get["/cards/new", true] = async (parameters, ct) => {
                NewCard model = new NewCard();
                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;

                return View["Change/NewCard", model];
            };

            Post["/cards/new", true] = async (parameters, ct) => {
                NewCard model = this.Bind<NewCard>();
                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;
                model.UserId = model.Planeswalker.Id;
                Guid id = repository.AddCard(model);
                model = repository.GetCard(id);
                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;

                return View["Change/NewCard", model];
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

