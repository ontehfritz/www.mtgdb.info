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
        }
    }
}

