using System;

namespace MtgDb.Info
{
    public class SearchModel : PageModel
    {
        public string Term { get; set; }
        public Card[] Cards { get; set; }
    }
}

