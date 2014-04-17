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

		public String[] GetSetTypes()
		{
			return 
				(Sets.Where(n => n.Type != null).OrderBy(n => n.ReleasedAt).Select(n => n.Type)).Distinct().ToArray();
		}

		public CardSet[] GetSetsInType(string typeName)
		{
			return 
				(Sets.Where(n => n.Type == typeName).OrderByDescending(n => n.ReleasedAt)).ToArray();
		}
    }
}

