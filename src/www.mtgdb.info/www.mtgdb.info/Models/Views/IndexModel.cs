using System;
using MtgDb.Info.Driver;

namespace MtgDb.Info
{
    public class IndexModel : PageModel
    {
        public CardSetList SetList { get; set; }
        public Card[] Cards { set; get; }
       
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

