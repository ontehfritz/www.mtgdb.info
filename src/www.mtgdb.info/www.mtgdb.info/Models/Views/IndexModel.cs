using System;
using Mtgdb.Info.Wrapper;
using Mtgdb.Info;

namespace mtgdb.info
{
    public class IndexModel : PageModel
    {
        public CardSet[] Sets { set; get; }
        public Card[] Cards { set; get; }

        public IndexModel () : base()
        {
            Db magicdb = new Db ();
            Sets = magicdb.GetSets ();

        }

        public void SetCards(string setId)
        {
            Db magicdb = new Db();
            Cards = magicdb.GetSetCards(setId);
        }

        public void SetAllCards()
        {
            Db magicdb = new Db();
            Cards = magicdb.GetCards();
        }
    }
}

