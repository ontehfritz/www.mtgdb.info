using System;
using Nancy;
using MtgDb.Info.Driver;

namespace MtgDb.Info
{
    public class IndexModule : NancyModule
    {
        public Db magicdb = new Db ("http://127.0.0.1:8082/");

        public IndexModule ()
        {
            Get ["/"] = parameters => {
                IndexModel model = new IndexModel();
                model.SetList.Sets = magicdb.GetSets();

                return View["Index", model];
            };
         
            Get ["/set/{id}"] = parameters => {
                IndexModel model = new IndexModel();
                string setId = (string)parameters.id;

                model.SetList.Sets = magicdb.GetSets();
                model.SetList.ActiveSet = setId;

                if(setId == "FULL")
                {
                    model.Cards = magicdb.GetCards();
                }
                else
                {
                    model.Cards = magicdb.GetSetCards(setId,1,9);
                }


                return View["Index", model];
            };

            Get ["/home"] = parameters => {
                IndexModel model = new IndexModel();

                return View["Index", model];
            };
        }
    }
}

