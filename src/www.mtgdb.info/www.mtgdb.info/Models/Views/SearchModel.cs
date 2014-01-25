using System;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class SearchModel : PageModel
    {
        public string Term { get; set; }
        public List<CardInfo> Cards { get; set; }

        public SearchModel(){
            Cards = new List<CardInfo> ();
        }
    }
}

