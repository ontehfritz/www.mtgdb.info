using System;
using Nancy;
using Nancy.ModelBinding;
using MtgDb.Info.Driver;

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
               
                return View["Search", model];
            };
        }
    }
}

