using System;
using Nancy;
using MtgDb.Info.Driver;

namespace MtgDb.Info
{
    public class IndexModule : NancyModule
    {
        public Db magicdb = new Db ("http://127.0.0.1:8082/");
        private const int _pageSize = 9;

        public IndexModule ()
        {
            Get ["/"] = parameters => {
                IndexModel model = new IndexModel();
                model.SetList.Sets = magicdb.GetSets();

                return View["Index", model];
            };
         
            Get ["/sets/"] = parameters => {
                SetsModel model = new SetsModel();
                model.Sets = magicdb.GetSets();
                

                return View["Sets", model];
            };

            Get ["/sets/{id}"] = parameters => {
                IndexModel model = new IndexModel();
                string setId = (string)parameters.id;

                int page = 1; 

                model.SetList.Sets = magicdb.GetSets();
                model.SetList.ActiveSet = setId;

                if(Request.Query.Page != null)
                {
                    if(int.TryParse((string)Request.Query.Page, out page))
                    {
                        if(page < 1){ page = 1; }
                    }
                }

                if(setId == "FULL")
                {
                    model.Cards = magicdb.GetCards();
                }
                else
                {
                    int end = page * _pageSize;
                    int start = page > 1 ? end - _pageSize : page;
                    model.Cards = magicdb.GetSetCards(setId, start, 
                        page > 1 ? end - 1 : end);
                }

                model.Page = page;
                model.NextPage = page + 1;
                model.PrevPage = page > 1 ? page - 1 : page;

                return View["Index", model];
            };

            Get ["/home"] = parameters => {
                IndexModel model = new IndexModel();

                return View["Index", model];
            };
        }
    }
}

