using System;
using MtgDb.Info.Driver;
using System.Collections.Generic;
using System.Linq;

namespace MtgDb.Info
{
    public class IndexModel : PageModel
    {
        public CardSetList SetList  { get; set; }
        public List<CardInfo> Cards { set; get; }
       
        public IndexModel () : base()
        {
            SetList =   new CardSetList ();
            Cards =     new List<CardInfo> ();
        }
    }

    public class CardSetList
    {
        public MtgDb.Info.CardSet[] Sets    { set; get; }
        public string ActiveSet             { get; set; }

		public CardSet[] GetBlock(string cardSet)
		{
			return 
				(Sets.Where (n => n.Block == cardSet).OrderByDescending(n => n.ReleasedAt)).ToArray();
		}
    }
}

