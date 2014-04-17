using System;
using MtgDb.Info.Driver;
using System.Collections.Generic;
using System.Linq;

namespace MtgDb.Info
{
    public class SetsModel : PageModel
    {
        public CardSet[] Sets                    { get; set; }
        public Dictionary<string, int> UserCards { get; set; }

        public SetsModel () : base(){}

		public String[] GetSetTypes()
		{
			return 
				(Sets.Where(n => n.Type != null).OrderBy(n => n.ReleasedAt).Select(n => n.Type)).Distinct().ToArray();
		}

		public String[] GetBlocksInSetType(string typeName)
		{
			return 
				(Sets.Where(n => n.Type == typeName).Where(n => n.Block != null).OrderByDescending(n => n.ReleasedAt).Select(n => n.Block)).Distinct().ToArray();
		}
			
		public CardSet[] GetSetsInTypeAndBlock(string typeName,string blockName)
        {
            return 
				(Sets.Where(n => n.Type == typeName).Where (n => n.Block == blockName).OrderByDescending(n => n.ReleasedAt)).ToArray();
        }
			
        public int GetSetCount(string setId)
        {
            if(UserCards != null && UserCards.Count > 0)
            {
                if (UserCards.ContainsKey (setId))
                    return UserCards [setId];

                return 0;
            }
           
            return 0;
        }
    }
}

