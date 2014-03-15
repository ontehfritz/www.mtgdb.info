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

        public SetsModel () : base()
        {

        }

        public CardSet[] GetBlock(string cardSet)
        {
            return 
                (Sets.Where (n => n.Block == cardSet)).ToArray();
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

