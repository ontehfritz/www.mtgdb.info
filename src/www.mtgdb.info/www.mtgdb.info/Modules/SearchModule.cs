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

        public SearchModule () 
        {
            Get ["/search"] = parameters => {
                SearchModel model = new SearchModel();

                return View["Search", model];
            };

            Post ["/search"] = parameters => {
                SearchModel model = this.Bind<SearchModel>();
                model.Cards = magicdb.Search(model.Term);
                model.Cards = model.Cards
                    .AsEnumerable()
                    .OrderBy(x => x.Name)
                    .ThenByDescending(x => x.ReleasedAt).ToArray();
               
                return View["Search", model];
            };
        }
    }
}

