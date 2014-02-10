using System;
using SuperSimple.Auth;
using Nancy;
using Nancy.Security;
using System.Linq;
using MtgDb.Info.Driver;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class CardModule : NancyModule
    {
        public IRepository repository = new MongoRepository ("mongodb://localhost");
        public Db magicdb = new Db ();


        public CardModule ()
        {
            this.RequiresAuthentication ();

            Post ["/cards/{id}/amount/{count}"] = parameters => {
                int multiverseId = (int)parameters.id;
                int count = (int)parameters.count;
                Guid walkerId = ((Planeswalker)this.Context.CurrentUser).Id;

                repository.AddUserCard(walkerId,multiverseId,count);


                return count.ToString();
            };

            Get ["/{planeswalker}/blocks/{block}/cards/{setId?}"] = parameters => {
                PlaneswalkerModel model = new PlaneswalkerModel();
                model.ActiveMenu = "mycards";

                string setId = (string)parameters.setId;
                model.Block = (string)parameters.block;

                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;
                model.Counts = repository.GetSetCardCounts(model.Planeswalker.Id);
                model.TotalCards = model.Counts.Sum(x => x.Value);
                model.TotalAmount = repository.GetUserCards(model.Planeswalker.Id).Sum(x => x.Amount);
                
                string[] blocks;

                if(model.Counts.Count > 0)
                {
                    model.Sets = magicdb.GetSets(model.Counts
                        .AsEnumerable().Select(x => x.Key)
                        .ToArray());

                    blocks = model.Sets.Select(x => x.Block).Distinct().OrderBy(x => x).ToArray();

                    foreach(string block in blocks)
                    {
                        CardSet[] sets = model.Sets.Where(x => x.Block == block).ToArray();

                        int total = 0; 
                        foreach(CardSet set in sets)
                        {
                            total += model.Counts[set.Id];
                        }

                        model.Blocks.Add(block,total);
                    }

                    model.Sets = model.Sets
                        .Where(x => x.Block == model.Block)
                        .OrderBy(x => x.Name).ToArray();


                    if(setId == null)
                    {
                        setId = model.Sets.FirstOrDefault().Id;
                    }

                    model.SetId = setId;
                    model.UserCards = repository.GetUserCards(model.Planeswalker.Id, setId);


                    Card[] dbcards = null;
                    if(Request.Query.Show != null)
                    {
                        dbcards = magicdb.GetSetCards(model.SetId);
                        model.Show = true;
                    }
                    else
                    {
                        dbcards = magicdb.GetCards(model.UserCards
                            .AsEnumerable()
                            .Select(x => x.MultiverseId)
                            .ToArray());

                        model.Show = false;
                    }


                    List<CardInfo> cards = new List<CardInfo>();

                    CardInfo card = null;
                    foreach(Card c in dbcards)
                    {
                        card = new CardInfo();
                        card.Amount = model.UserCards
                            .AsEnumerable()
                            .Where(x => x.MultiverseId == c.Id)
                            .Select(x => x.Amount)
                            .FirstOrDefault();

                        card.Card = c;
                    
                        cards.Add(card);
                    }

                    model.Cards = cards.OrderBy(x => x.Card.SetNumber).ToArray();
                }
                else
                {
                    model.Messages.Add("You have no cards in your library.");
                }

                return View["MyCards", model];
            };

            Get ["/{planeswalker}/cards"] = parameters => {
                PageModel model = new PageModel();
                model.ActiveMenu = "mycards";

                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;

                if(model.Planeswalker.UserName.ToLower() != ((string)parameters.planeswalker).ToLower())
                {
                    model.Errors.Add(string.Format("Tsk Tsk! {0}, this profile is not yours.",
                        model.Planeswalker.UserName));

                    return View["Page", model];
                }

                string setId = repository.GetSetCardCounts(model.Planeswalker.Id)
                    .Select(x => x.Key)
                    .OrderBy(x => x).FirstOrDefault();

                if(setId != null)
                {
                    CardSet s = magicdb.GetSet(setId);

                    return Response.AsRedirect("~/" + model.Planeswalker.UserName + 
                        "/blocks/" + s.Block + "/cards");
                }

                model.Information.Add("You have no cards yet in your library. Browse the sets and start adding cards Planeswalker!");
                return View["Page", model];
            };
        }
    }
}

