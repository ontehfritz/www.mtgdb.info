using System;
using MtgDb.Info.Driver;
using MtgDb.Info;

namespace mtgdb.info
{
    public class IndexModel : PageModel
    {
        public CardSet[] Sets { set; get; }
        public Card[] Cards { set; get; }
        public Db magicdb = new Db ("http://127.0.0.1:8082/");


        public IndexModel () : base()
        {

            Sets = magicdb.GetSets ();

        }

        public void SetCards(string setId)
        {

            Cards = magicdb.GetSetCards(setId,1,9);
        }

        public void SetAllCards()
        {

            Cards = magicdb.GetCards();
        }
    }
}

