using System;
using Nancy;
using MtgDb.Info.Driver;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;

namespace MtgDb.Info
{
    public class IndexModule : NancyModule
    {
        public Db magicdb = 
            new Db (ConfigurationManager.AppSettings.Get("api"));

        public IRepository repository = 
            new MongoRepository (ConfigurationManager.AppSettings.Get("db"));

        private const int _pageSize = 9;

        public IndexModule ()
        {
            Get ["/about"] = parameters => {
                PageModel model = new PageModel();
                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;

                return View["about", model];
            };

			Get ["/FAQ"] = parameters => {
                PageModel model = new PageModel();
                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;
				model.ActiveMenu = "FAQ";

				return View["FAQ", model];
            };

            Get ["/terms"] = parameters => {
                PageModel model = new PageModel();
                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;
              
                return View["Terms", model];
            };

            Get ["/"] = parameters => {
                IndexModel model = new IndexModel();
                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;

                try
                {
                    model.SetList.Sets = magicdb.GetSets();
                }
                catch(Exception e)
                {
                    model.Errors.Add(e.Message);
                }


                return View["Index", model];
            };
         
            Get ["/sets/"] = parameters => {
                SetsModel model = new SetsModel();
                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;

                try
                {
                    model.Sets = magicdb.GetSets();
                    model.ActiveMenu = "sets";

                    if(model.Planeswalker != null)
                    {
                        model.UserCards = repository.GetSetCardCounts(model.Planeswalker.Id);
                    }
                }
                catch(Exception e)
                {
                    model.Errors.Add(e.Message);
                }

                return View["Sets", model];
            };

            Get ["/cards/{id}"] = parameters => {
                CardModel model = new CardModel();
                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;

                try
                {
                    model.Card = magicdb.GetCard((int)parameters.id);
                    model.Prints = magicdb.GetCards(model.Card.Name);
                    model.Page = (model.Card.SetNumber / _pageSize) + 1;
                    model.Set = magicdb.GetSet(model.Card.CardSetId);
                    model.ActiveMenu = "sets";
                    model.NextCard = magicdb.GetCard(model.Card.SetNumber + 1);
                    model.PrevCard = magicdb.GetCard(model.Card.SetNumber - 1);

                    if(model.Planeswalker != null)
                    {
                        UserCard uCard = repository.GetUserCards(model.Planeswalker.Id,
                            new int[]{model.Card.Id}).FirstOrDefault();

                        if(uCard != null)
                        {
                            model.Amount = uCard.Amount;
                        }
                        else
                        {
                            model.Amount = 0;
                        }
                    }
                }
                catch(Exception e)
                {
                    model.Errors.Add(e.Message);
                }
                    
                return View["Card", model];
            };
          
            Get ["/sets/{id}"] = parameters => {
                BookModel model = new BookModel();
                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;
                string setId = (string)parameters.id;
                model.ActiveMenu = "sets";

                int page = 1; 
                try
                {
                    model.SetList.Sets = magicdb.GetSets();
                    model.SetList.ActiveSet = setId;
                    model.Set = magicdb.GetSet(setId);

                    if(Request.Query.Page != null)
                    {
                        if(int.TryParse((string)Request.Query.Page, out page))
                        {
                            if(page < 1){ page = 1; }
                        }
                    }

                   
                    int end = page * _pageSize;
                    int start = page > 1 ? end - _pageSize : page;
                    Card[] cards = magicdb.GetSetCards(setId, page > 1 ? start + 1 : start, 
                        end);

                    UserCard [] walkerCards = null;

                    if(model.Planeswalker != null)
                    {
                        int [] cardIds = cards.AsEnumerable().Select(c => c.Id).ToArray();
                        walkerCards = repository.GetUserCards(model.Planeswalker.Id,cardIds);
                    }

                    foreach(var c in cards)
                    {
                        CardInfo cardInfo = new CardInfo();
                        if(walkerCards != null && walkerCards.Length > 0)
                        {
                            cardInfo.Amount = walkerCards.AsEnumerable()
                                .Where(info => info.MultiverseId == c.Id)
                                .Select(info => info.Amount).FirstOrDefault();

                        }
                        else
                        {
                            cardInfo.Amount = 0;
                        }

                        cardInfo.Card = c;
                        model.Cards.Add(cardInfo);
                    }

                    model.Page = page;
                    model.NextPage = page + 1;
                    model.PrevPage = page > 1 ? page - 1 : page;
                }
                catch(Exception e)
                {
                    model.Errors.Add(e.Message);
                }

                return View["Book", model];
            };

            Get ["/home"] = parameters => {
                IndexModel model = new IndexModel();
                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;

                return View["Index", model];
            };

          
            Get ["/pw/{planeswalker}"] = parameters => {
                PlaneswalkerModel model = new PlaneswalkerModel();
                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;

                string planeswalker = ((string)parameters.planeswalker).ToLower();

                try
                {
                    if(model.Planeswalker != null && 
                        model.Planeswalker.Profile.Name.ToLower() != planeswalker)
                    {
                        model.Profile = repository.GetProfile(planeswalker);

                        if(model.Profile == null)
                        {
                            model.Messages.Add("Planeswalker does not exist or is private profile.");
                        }
                        else
                        {
                            model.UserCards = repository.GetUserCards(model.Profile.Id);
                        }
                    }
                    else if(model.Planeswalker != null)
                    {
                        model.Profile = model.Planeswalker.Profile;
                        model.UserCards = repository.GetUserCards(model.Profile.Id);
                    }
                    else
                    {
                        model.Profile = repository.GetProfile(planeswalker);

                        if(model.Profile == null)
                        {
                            model.Messages.Add("Planeswalker does not exist or is private profile.");
                        }
                        else
                        {
                            model.UserCards = repository.GetUserCards(model.Profile.Id);
                        }
                    }
                }
                catch(Exception e)
                {
                    model.Errors.Add(e.Message);
                }

                return View["Planeswalker", model];
            };
        }
    }
}

