using System;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class PlaneswalkerModel : PageModel
    {
        public CardInfo[] Cards { get; set; }
        public Dictionary<string, int> Sets { get; set; }
        public Profile Profile { get; set; }

        public PlaneswalkerModel ()
        {
        }
    }


}

