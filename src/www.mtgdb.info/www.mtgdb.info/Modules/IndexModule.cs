using System;
using Nancy;

namespace mtgdb.info
{
    public class IndexModule : NancyModule
    {
        public IndexModule ()
        {
            Get ["/"] = parameters => {

                return View["Index"];
            };
        }
    }
}

