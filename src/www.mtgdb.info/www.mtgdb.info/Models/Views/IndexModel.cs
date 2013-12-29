using System;
using MtgDb.Info.Driver;

namespace MtgDb.Info
{
    public class IndexModel : PageModel
    {
        public CardSetList SetList { get; set; }
        public Card[] Cards { set; get; }
        public int Page { set; get; }
        public int NextPage { set; get; }
        public int PrevPage { set; get; }
       
        public IndexModel () : base()
        {
            SetList = new CardSetList ();
        }

    }

    public class CardSetList
    {
        public MtgDb.Info.CardSet[] Sets { set; get; }
        public string ActiveSet { get; set; }
    }
}

