using System;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class BookModel : PageModel
    {
        public CardSetList SetList { get; set; }
        public List<CardInfo> Cards { set; get; }
        public int Page { set; get; }
        public int NextPage { set; get; }
        public int PrevPage { set; get; }

        public BookModel () : base()
        {
            SetList = new CardSetList ();
            Cards = new List<CardInfo> ();
        }
    }
}

