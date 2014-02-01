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

                string setId = (string)parameters.setId;
                model.Block = (string)parameters.block;

                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;
                model.Counts = repository.GetSetCardCounts(model.Planeswalker.Id);
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


                    Card[] dbcards = magicdb.GetCards(model.UserCards
                        .AsEnumerable()
                        .Select(x => x.MultiverseId)
                        .ToArray());

                    List<CardInfo> cards = new List<CardInfo>();

                    CardInfo card = null;
                    foreach(UserCard c in model.UserCards)
                    {
                        card = new CardInfo();
                        card.Amount = c.Amount;
                        card.Card = dbcards.AsEnumerable().Where(x => x.Id == c.MultiverseId).FirstOrDefault();

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

                model.Planeswalker = (Planeswalker)this.Context.CurrentUser;

                string setId = repository.GetSetCardCounts(model.Planeswalker.Id)
                    .Select(x => x.Key)
                    .OrderBy(x => x).FirstOrDefault();

                CardSet s = magicdb.GetSet(setId);

                if(setId != null)
                {
                    return Response.AsRedirect("~/" + model.Planeswalker.UserName + 
                        "/blocks/" + s.Block + "/cards");
                }

                return View["NoCards", model];
            };


        }
    }
}

