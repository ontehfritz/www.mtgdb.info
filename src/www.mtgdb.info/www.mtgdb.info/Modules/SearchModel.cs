using System;
using Nancy;

namespace MtgDb.Info
{
    public class SearchModel : NancyModule
    {
        public SearchModel () 
        {
            Get ["/search"] = parameters => {
                return "SearchModel";
            };
        }
    }
}

