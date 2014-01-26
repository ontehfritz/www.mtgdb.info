using System;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class PlaneswalkerModel : PageModel
    {
        public UserCard[] Cards { get; set; }
        public Dictionary<string, CardInfo[]> UserCards { get; set; }
        public Profile Profile { get; set; }

        public PlaneswalkerModel ()
        {
        }
    }


}

