using System;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class DecksModel : PageModel
    {
        List<Deck> Decks { get; set; }

        public DecksModel () : base()
        {
            Decks = new List<Deck>();
        }
    }
}

