using System;
using Nancy;
using Nancy.ModelBinding;
using MtgDb.Info.Driver;
using System.Linq;

namespace MtgDb.Info
{
    public class SearchModule : NancyModule
    {
        public Db magicdb = new Db ();
        public IRepository repository = new MongoRepository ("mongodb://localhost");

        public SearchModule () 
        {
            Get ["/search"] = parameters => {
                SearchModel model = new SearchModel();
                model.Planeswalker = ((Planeswalker)this.Context.CurrentUser);
                model.ActiveMenu = "search";

                return View["Search", model];
            };

            Post ["/search"] = parameters => {
                SearchModel model = this.Bind<SearchModel>();
                model.Planeswalker = ((Planeswalker)this.Context.CurrentUser);
                UserCard [] walkerCards = null;
                Card[] cards = magicdb.Search(model.Term);
                model.ActiveMenu = "search";

                cards = cards
                    .AsEnumerable()
                    .OrderBy(x => x.Name)
                    .ThenByDescending(x => x.ReleasedAt).ToArray();

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
               
                return View["Search", model];
            };
        }
    }
}

