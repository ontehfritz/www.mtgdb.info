using System;
using Nancy;

namespace mtgdb.info
{
    public class IndexModule : NancyModule
    {
        public IndexModule ()
        {
            Get ["/"] = parameters => {
                IndexModel model = new IndexModel();

                return View["Index", model];
            };
         
            Get ["/set/{id}"] = parameters => {
                IndexModel model = new IndexModel();
                string setId = (string)parameters.id;

                if(setId == "FULL")
                {
                    model.SetAllCards();
                }
                else
                {
                    model.SetCards(setId);
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

