using System;
using MtgDb.Info.Driver;
using System.Collections.Generic;
using System.Linq;

namespace MtgDb.Info
{
    public class SetsModel : PageModel
    {
        public CardSet[] Sets { get; set; }

        public SetsModel () : base()
        {

        }

        public CardSet[] GetBlock(string cardSet){

            return 
                (Sets.Where (n => n.Block == cardSet)).ToArray();
        }
    }
}

